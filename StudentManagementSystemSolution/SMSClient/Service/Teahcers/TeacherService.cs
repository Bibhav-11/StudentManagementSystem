using Microsoft.AspNetCore.Identity;
using SMSClient.Data.Identity;
using SMSClient.Model;
using SMSClient.Models;
using SMSClient.Repository;
using SMSClient.Repository.Teachers;
using System.Security.Claims;

namespace SMSClient.Service.Teahcers
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly AspIdUsersContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherService(ITeacherRepository teacherRepository, AspIdUsersContext context, UserManager<ApplicationUser> userManager)
        {
            _teacherRepository = teacherRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task AddTeacher(Teacher teacher)
        {
            var user = new ApplicationUser
            {
                Email = teacher.Email,
                UserName = teacher.Email,
            };

            var result = await _userManager.CreateAsync(user, "Pass123$");
            await _userManager.AddToRoleAsync(user, "teacher");
            if (result.Succeeded)
            {
                var createdUser = await _userManager.FindByNameAsync(teacher.Email);
                teacher.ApplicationUserId = createdUser.Id;
                _teacherRepository.Add(teacher);
                await _context.SaveChangesAsync();

                var createdTeacher = (await _teacherRepository.FindAll(t => t.Email == teacher.Email)).SingleOrDefault();
                if (createdTeacher != null)
                {
                    if (createdTeacher.ClassId != null)
                    {
                        var teacherClaim = new Claim("class", createdTeacher.ClassId.ToString());
                        await _userManager.AddClaimAsync(createdUser, teacherClaim);
                    }
                }
            }
        }

        public async Task DeleteTeacher(Teacher teacher)
        {
            var user = await _userManager.FindByIdAsync(teacher.ApplicationUserId);
            _teacherRepository.Remove(teacher);
            await _context.SaveChangesAsync();
            await _userManager.DeleteAsync(user);
        }


        public async Task<Teacher?> GetTeacherById(int id)
        {
            return await _teacherRepository.GetById(id);
        }

        public async Task<Teacher?> GetTeacherByUserId(string userId)
        {
            return (await _teacherRepository.FindAll(t => t.ApplicationUserId == userId)).FirstOrDefault();
        }

        public async Task<IEnumerable<Teacher>> GetTeachers()
        {
            return await _teacherRepository.GetAll();
        }

        public async Task<IEnumerable<Teacher>> GetTeachersWithClassInfo()
        {
            return await _teacherRepository.GetTeachersWithClassInfo();
        }

        public async Task UpdateTeacher(Teacher teacher)
        {
            _teacherRepository.Update(teacher);
            var user = await _userManager.FindByIdAsync(teacher.ApplicationUserId);
            if(user != null)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                foreach (var claim in userClaims)
                {
                    if(claim.Type == "class") await _userManager.RemoveClaimAsync(user, claim);
                }
                await _userManager.AddClaimAsync(user, new Claim("class", teacher.ClassId.ToString()));
            }
            await _context.SaveChangesAsync();
        }
    }
}
