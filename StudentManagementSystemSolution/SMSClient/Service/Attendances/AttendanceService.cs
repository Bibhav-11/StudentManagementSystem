using Microsoft.AspNetCore.Identity;
using SMSClient.Model;
using SMSClient.Model.ApiModel;
using SMSClient.Service.Students;
using SMSClient.Service.Teahcers;
using SMSClient.Service.Users;

namespace SMSClient.Service.Attendances
{
    public class AttendanceService: IAttendanceService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;

        public AttendanceService(UserManager<ApplicationUser> userManager, IStudentService studentService, ITeacherService teacherService)
        {
            _userManager = userManager;
            _studentService = studentService;
            _teacherService = teacherService;
        }

        public async Task<IEnumerable<AttendanceList>> AttendanceResponseToList(IEnumerable<AttendanceResponse> attendanceResponses)
        {
            List<AttendanceList> attendanceLists = new();
            foreach (var response in attendanceResponses)
            {

                var student = await _studentService.GetStudentById(response.StudentId);
                string StudentName = student != null ? $"{student.FirstName} {student.LastName}" : "";
                var teacher = await _teacherService.GetTeacherById(response.TeacherId);
                string TeacherName = teacher != null ? $"{teacher.FirstName} {teacher.LastName}" : "";


                AttendanceList attendanceList = new()
                {
                    Id = response.Id,
                    Student = StudentName,
                    MarkedBy = TeacherName,
                    Date = response.AttendanceDate,
                    Status = response.Status,
                };

                attendanceLists.Add(attendanceList);
            }

            return attendanceLists;
        }

        public async Task<IEnumerable<AttendanceRequest>> GenerateRequestFromPresentStudentList(int[] presentStudentsIds, int teacherId)
        {
            List<AttendanceRequest> attendanceRequests = new();

            var allStudentIds = (await _studentService.GetStudents()).Select(s => s.Id).ToList();

            foreach (var studentId in allStudentIds)
            {
                bool IsStudentPresent = presentStudentsIds.Contains(studentId);
                var attendanceRequest = new AttendanceRequest
                {
                    StudentId = studentId,
                    TeacherId = teacherId,
                    AttendanceDate = DateOnly.FromDateTime(DateTime.Now),
                    IsPresent = IsStudentPresent ? 1 : 0
                };
                attendanceRequests.Add(attendanceRequest);
            }
            return attendanceRequests;
        }
    }
}
