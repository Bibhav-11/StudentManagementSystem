using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMSClient.Data.Identity;
using SMSClient.Model;
using SMSClient.Repository.Students;
using SMSClient.Service.Students;
using SMSClient.Service.Teahcers;
using System.Security.Claims;

namespace SMSClient.Service.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly AspIdUsersContext _context;

        public UsersService(UserManager<ApplicationUser> userManager, AspIdUsersContext context, IStudentRepository studentRepository, ITeacherService teacherService, IStudentService studentService)
        {
            _userManager = userManager;
            _context = context;
            _studentRepository = studentRepository;
            _teacherService = teacherService;
            _studentService = studentService;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }
        public async Task<IList<ApplicationUser>> GetUsersinRole(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync("Student");
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<UserInfo?> GetUserInfoValue(string userId)
        {
            var userInfo = await _context.UserInfos.FirstOrDefaultAsync(uf => uf.ApplicationUserId == userId);
            return userInfo;
        }

        public async Task<List<object>> GetUserInfo(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<object>();

            var userInfo = _context.UserInfos.FirstOrDefault(ui => ui.ApplicationUserId == userId);

            var userItems = new List<string>();
            userItems.Add($"UserName: {user.UserName}");
            userItems.Add($"Email: {user.Email}");

            var userInfoItems = new List<string>();
            if (userInfo != null)
            {
                userInfoItems.Add($"FullName: {userInfo.FullName}");
                userInfoItems.Add($"Gender: {userInfo.Gender}");
                userInfoItems.Add($"DateOfBirth: {userInfo.DateOfBirth}");
            }

            var result = new List<object>
           {
                new
                {
                    key = "User Table",
                    items = userItems
                },
                new
                {
                    key = "UserInfo Table",
                    items = userInfoItems
                }
            };

            return result;

        }

        public async Task<IEnumerable<object>> GetAdditionalInfo(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            var results = await GetUserInfo(userId);

            if (await _userManager.IsInRoleAsync(user, "student"))
            {
                var students = await _studentRepository.GetAll();
                var student = students.FirstOrDefault(s => s.ApplicationUserId == userId);
                var studentInfo = new List<string>();
                if (student != null)
                {
                    studentInfo.Add($"FirstName: {student.FirstName}");
                    studentInfo.Add($"LastName: {student.LastName}");
                    studentInfo.Add($"Email: {student.Email}");
                    studentInfo.Add($"Phone: {student.Phone}");
                    studentInfo.Add($"DateOfBirth: {student.DateOfBirth}");
                };
                var studentResult = new { key = "StudentTable", items = studentInfo };
                results.Add(studentResult);
            }
            return results;
        }


        public async Task<bool> CreateUserAsync(UserFormViewModel userForm)
        {

            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = userForm.UserName,
                        Email = userForm.Email,
                        EmailConfirmed = true,
                    };

                    var result = await _userManager.CreateAsync(user, userForm.Password);

                    if (result.Succeeded)
                    {
                        var userId = (await _userManager.FindByNameAsync(userForm.UserName));
                        UserInfo userInfo = new UserInfo
                        {
                            DateOfBirth = userForm.DateOfBirth,
                            FullName = userForm.FullName,
                            Gender = userForm.Gender,
                            ApplicationUserId = userId.Id
                        };
                        _context.UserInfos.Add(userInfo);

                        //var roles = userForm.Roles;
                        var role = userForm.Roles;
                        if (role != null) await _userManager.AddToRolesAsync(userId, role);

                        await transaction.CommitAsync();
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                }
                catch(Exception ex)
                {
                    Log.Error(ex.Message);  
                    await transaction.RollbackAsync();
                    return false;
                }
                
            }        

        }


        public async Task UpdateUserAsync(UserEditViewModel userForm)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingUser = await _userManager.FindByIdAsync(userForm.Id);
                    existingUser.Email = userForm.Email;
                    existingUser.UserName = userForm.UserName;

                    var result = await _userManager.UpdateAsync(existingUser);

                    if (result.Succeeded)
                    {
                        var userId = (await _userManager.FindByNameAsync(userForm.UserName));
                        UserInfo userInfo = new UserInfo
                        {
                            Id = userForm.UserInfoId,
                            DateOfBirth = userForm.DateOfBirth,
                            FullName = userForm.FullName,
                            Gender = userForm.Gender,
                            ApplicationUserId = userId.Id
                        };
                        _context.UserInfos.Update(userInfo);


                        await transaction.CommitAsync();
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    await transaction.RollbackAsync();
                }

            }
        }


        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (await _userManager.IsInRoleAsync(user, "student"))
            {
                var student = await _studentService.GetStudentByUserId(userId);
                await _studentService.DeleteStudent(student);
                return;
            }

            if(await _userManager.IsInRoleAsync(user, "teacher"))
            {
                var teacher = await _teacherService.GetTeacherByUserId(userId);
                await _teacherService.DeleteTeacher(teacher);
                return;
            }

            if (user is null) { return; }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            var userInfo = _context.UserInfos.FirstOrDefault(ui => ui.ApplicationUserId == userId);
            if (userInfo != null) _context.UserInfos.Remove(userInfo);
            var result2 = await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }
    }
}
