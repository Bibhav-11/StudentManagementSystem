using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMSClient.Client;
using SMSClient.Model.ApiModel;
using SMSClient.Service.Attendances;
using SMSClient.Service.Students;
using SMSClient.Service.Teahcers;
using SMSClient.Service.Users;

namespace SMSClient.Controllers
{
    public class AttendancesController: Controller
    {
        private readonly IAttendanceClient _attendanceClient;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IAttendanceService _attendanceService;
        private readonly IUsersService _userService;

        public AttendancesController(IAttendanceClient attendanceClient, IStudentService studentService, ITeacherService teacherService, IAttendanceService attendanceService, IUsersService userService)
        {
            _attendanceClient = attendanceClient;
            _studentService = studentService;
            _teacherService = teacherService;
            _attendanceService = attendanceService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Index()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            IEnumerable<AttendanceResponse>? attendanceResponses = await _attendanceClient.GetAttendancesOfAClass(accessToken);
            IEnumerable<AttendanceList> attendanceLists = await _attendanceService.AttendanceResponseToList(attendanceResponses);
            return View(attendanceLists);
        }

        [HttpGet]
        [Authorize(Roles = "teacher")]
        public IActionResult StudentAttendance()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles ="teacher")]
        public async Task<IActionResult> Create()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            var classId = await _attendanceService.GetClassId(HttpContext.User);
            if (classId is null) return View();
            int classIdValue = classId.Value;


            var attendanceAlreadyMarked = await _attendanceClient.CheckIfAlreadyExists(accessToken);
            if(attendanceAlreadyMarked) { ViewBag.AttendanceAlreadyMarked = true; }
            else ViewBag.AttendanceAlreadyMarked = false;
            var students = await _studentService.GetStudentsByClassId(classIdValue);
            return View(students);
        }

        [HttpGet]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> MyAttendance()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            var attendanceResponses = await _attendanceClient.GetAttendancesForAStudent(accessToken);
            var attendanceLists = await _attendanceService.AttendanceResponseToList(attendanceResponses);
            return View(attendanceLists);
        }

        [HttpPost]
        [Authorize(Roles ="teacher")]
        public async Task<JsonResult> CreateAttendance(int[] studentIds)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            var currentUser = HttpContext.User;
            var user = await _userService.GetUserAsync(currentUser);

            var teacher = await _teacherService.GetTeacherByUserId(user.Id);
            if(teacher is null) return Json(new { success = false, message = "You don't have access to create attendance" });

            var classId = await _attendanceService.GetClassId(HttpContext.User);

            if(classId is null) return Json(new { success = false, message = "Class doesnot exist." });

            var attendanceRequests = await _attendanceService.GenerateRequestFromPresentStudentList(studentIds, teacher.Id, classId);

            HttpResponseMessage responseMessage = await _attendanceClient.PostListOfAttendanceAsync(attendanceRequests, accessToken);
            if(responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Successfully added attendances." });
            }
            else
            {
                return Json(new { success = false, message = responseMessage.ReasonPhrase});
            }
        }

        [HttpGet]
        [Authorize(Roles = "teacher")]
        public async Task<object> ListStudents(DataSourceLoadOptions loadOptions, int studentId)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            if (studentId == 0) return new List<object>();            
            var attendanceResponses = await _attendanceClient.GetAttendancesOfAStudent(studentId, accessToken);
            var attendanceLists = await _attendanceService.AttendanceResponseToList(attendanceResponses);
            
            return DataSourceLoader.Load(attendanceLists, loadOptions);
        }

        [HttpGet]
        public async Task<object> GetStudents(DataSourceLoadOptions loadOptions)
        {
            var classId = await _attendanceService.GetClassId(HttpContext.User);
            var students = await _studentService.GetStudentsByClassId(classId.Value);
            return DataSourceLoader.Load(students, loadOptions);
        }

        

    }
}
