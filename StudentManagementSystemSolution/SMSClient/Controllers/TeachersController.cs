using Microsoft.AspNetCore.Mvc;
using Serilog;
using SMSClient.Authentication;
using SMSClient.Constants;
using SMSClient.Model;
using SMSClient.Service.Departments;
using SMSClient.Service.Classes;
using SMSClient.Service.Teahcers;

namespace SMSClient.Controllers
{
    public class TeachersController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ITeacherService _teacherService;
        private readonly IClassService _classService;

        public TeachersController(IDepartmentService departmentService, ITeacherService teacherService, IClassService classService)
        {
            _departmentService = departmentService;
            _teacherService = teacherService;
            _classService = classService;
        }

        [CustomAuthorize(AccessLevels.View, Modules.Teacher)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetTeachersWithClassInfo();
            return View(teachers);
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Teacher)]
        [HttpGet]
        public IActionResult ShowAddPage()
        {
            return PartialView("_AddTeacherModal");
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Teacher)]
        [HttpPost]
        public async Task<JsonResult> AjaxCreate(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                Log.Error("Could not create the teacher");
                return Json(false);
            }
            await _teacherService.AddTeacher(teacher);
            Log.Information("New Teacher created");
            return Json(true);
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Teacher)]
        [HttpGet]
        public async Task<IActionResult> ShowEditPage(string id)
        {
            var teacher = await _teacherService.GetTeacherById(Int32.Parse(id));
            return PartialView("_EditTeacherModal", teacher);
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Teacher)]
        [HttpPost]
        public async Task<JsonResult> AjaxUpdate(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                Log.Error("Could not create the teacher");
                return Json(false);
            }
            try
            {
                await _teacherService.UpdateTeacher(teacher);
                Log.Information("Teacher updated");
                return Json(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Teacher)]
        [HttpGet]
        public async Task<IActionResult> ShowDeletePage(string id)
        {
            var teacher = await _teacherService.GetTeacherById(Int32.Parse(id));
            return PartialView("_DeleteTeacherModal", teacher);
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Teacher)]
        [HttpPost]
        public async Task<JsonResult> AjaxDelete(string id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherById(Int32.Parse(id));
                if (teacher is null)
                {
                    return Json(false);
                }
                await _teacherService.DeleteTeacher(teacher);
                    Log.Information("Successfully deleted the student");
                    return Json(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetDepartments()
        {
            return Json(await _departmentService.GetDepartments());
        }

        [HttpGet]
        public async Task<JsonResult> GetClasses()
        {
            return Json(await _classService.GetClasses());
        }

    }
}
