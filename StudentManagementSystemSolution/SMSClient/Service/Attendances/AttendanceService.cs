using Microsoft.AspNetCore.Identity;
using SMSClient.Model;
using SMSClient.Model.ApiModel;
using SMSClient.Service.Students;
using SMSClient.Service.Teahcers;
using SMSClient.Service.Users;
using System.Security.Claims;

namespace SMSClient.Service.Attendances
{
    public class AttendanceService: IAttendanceService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IUsersService _usersService;

        public AttendanceService(UserManager<ApplicationUser> userManager, IStudentService studentService, ITeacherService teacherService, IUsersService usersService)
        {
            _userManager = userManager;
            _studentService = studentService;
            _teacherService = teacherService;
            _usersService = usersService;
        }

        public async Task<IEnumerable<AttendanceList>> AttendanceResponseToList(IEnumerable<AttendanceResponse> attendanceResponses)
        {
            List<AttendanceList> attendanceLists = new();
            foreach (var response in attendanceResponses)
            {

                var student = await _studentService.GetStudentById(response.StudentId);
                if (student is null) continue;
                string StudentName = $"{student.FirstName} {student.LastName}";
                var teacher = await _teacherService.GetTeacherById(response.TeacherId);
                string TeacherName = teacher != null ? $"{teacher.FirstName} {teacher.LastName}" : "";


                AttendanceList attendanceList = new()
                {
                    Id = response.Id,
                    Student = StudentName,
                    MarkedBy = TeacherName,
                    Date = response.AttendanceDate,
                    Status = response.Status,
                    ClassId = response.ClassId,
                };

                attendanceLists.Add(attendanceList);
            }

            return attendanceLists;
        }

        public async Task<IEnumerable<AttendanceRequest>> GenerateRequestFromPresentStudentList(int[] presentStudentsIds, int teacherId, int? classId)
        {
            List<AttendanceRequest> attendanceRequests = new();

            var allStudentIds = (await _studentService.GetStudentsByClassId(classId.Value)).Select(s => s.Id).ToList();

            foreach (var studentId in allStudentIds)
            {
                bool IsStudentPresent = presentStudentsIds.Contains(studentId);
                var attendanceRequest = new AttendanceRequest
                {
                    StudentId = studentId,
                    TeacherId = teacherId,
                    AttendanceDate = DateOnly.FromDateTime(DateTime.Now),
                    IsPresent = IsStudentPresent ? 1 : 0,
                    ClassId = classId
                };
                attendanceRequests.Add(attendanceRequest);
            }
            return attendanceRequests;
        }

        public async Task<int?> GetClassId(ClaimsPrincipal user)
        {
            var teacherUser = await _usersService.GetUserAsync(user);
            if (teacherUser == null) return null;
            var teacher = await _teacherService.GetTeacherByUserId(teacherUser.Id);
            if (teacher == null) return null;
            var classId = teacher.ClassId;
            
            return classId;
        }
    }
}
