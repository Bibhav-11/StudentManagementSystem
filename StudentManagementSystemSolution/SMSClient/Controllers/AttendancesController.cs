using Microsoft.AspNetCore.Mvc;
using SMSClient.Client;
using SMSClient.Models.ApiModel;
using SMSClient.Service.Students;

namespace SMSClient.Controllers
{
    public class AttendancesController: Controller
    {
        private readonly IAttendanceClient _attendanceClient;
        private readonly IStudentService _studentService;

        public AttendancesController(IAttendanceClient attendanceClient, IStudentService studentService)
        {
            _attendanceClient = attendanceClient;
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetStudents();
            return View(students);
        }

        public async Task<JsonResult> CreateAttendance(int[] studentIds)
        {
            var allStudentIds = (await _studentService.GetStudents()).Select(s => s.Id).ToList();

            List<AttendanceRequest> attendanceRequests = new List<AttendanceRequest>();

            foreach(var studentId in allStudentIds)
            {
                bool IsStudentPresent = studentIds.Contains(studentId);
                var attendanceRequest = new AttendanceRequest
                {
                    StudentId = studentId,
                    TeacherId = 1,
                    AttendanceDate = DateOnly.FromDateTime(DateTime.Now),
                    IsPresent = IsStudentPresent ? 1 : 0
                };
                attendanceRequests.Add(attendanceRequest);
            }

            HttpResponseMessage responseMessage = await _attendanceClient.PostListOfAttendanceAsync(attendanceRequests);
            if(responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Successfully added attendances." });
            }
            else
            {
                return Json(new { success = false, message = responseMessage.ReasonPhrase});
            }
        }
    }
}
