using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using SMSClient.Authentication;
using SMSClient.Constants;
using SMSClient.Data.Identity;
using SMSClient.Models;
using SMSClient.Models.Identity;
using SMSClient.Models.ViewModel;
using SMSClient.Service.Departments;
using SMSClient.Service.Semesters;
using SMSClient.Service.Students;
using SMSClient.Service.Users;

namespace SMSClient.Controllers
{
    public class StudentsController: Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IUsersService _usersService;
        private readonly IStudentService _studentService;
        private readonly ISemesterService _semesterService;

        public StudentsController(IDepartmentService departmentService, IUsersService usersService, IStudentService studentService, ISemesterService semesterService)
        {
            _departmentService = departmentService;
            _usersService = usersService;
            _studentService = studentService;
            _semesterService = semesterService;
        }

        [CustomAuthorize(AccessLevels.View, Modules.Student)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetStudentsWithDepartmentAndSemesterInfo();
            return View(students);
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Student)]
        [HttpGet]
        public IActionResult ShowAddPage()
        {
            return PartialView("_AddStudentModal");
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Student)]
        [HttpPost]
        public async Task<JsonResult> AjaxCreate(Student student)
        {
            if (!ModelState.IsValid) {
                Log.Error("Could not create the semester");
                return Json(false);
            }
            var result = await _studentService.AddStudent(student);
            if (result) {
                Log.Information("Successfully created the semester");
                return Json(true);
            }
            else {
                Log.Error("Could not create the semester");
                return Json(false);
            }
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Student)]
        [HttpGet]
        public async Task<IActionResult> ShowEditPage(string id)
        {
            var student = await _studentService.GetStudentById(Int32.Parse(id));
            return PartialView("_EditStudentModal", student);
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Student)]
        [HttpPost]
        public async Task<JsonResult> AjaxUpdate(Student student)
        {
            try
            {
                var result = await _studentService.UpdateStudent(student);
                if (result == true) Log.Information("Successfully updated the student");
                return Json(result);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Student)]
        [HttpGet]
        public async Task<IActionResult> ShowDeletePage(string id)
        {
            var student = await _studentService.GetStudentById(Int32.Parse(id));
            return PartialView("_DeleteStudentModal", student);
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Student)]
        [HttpPost]
        public async Task<JsonResult> AjaxDelete(string id)
        {
            try
            {
                var student = await _studentService.GetStudentById(Int32.Parse(id));
                if (student is null)
                {
                    return Json(false);
                }
                var result = await _studentService.DeleteStudent(student);
                if (result)
                {
                    Log.Information("Successfully deleted the student");
                    return Json(true);
                }
                else
                {
                    Log.Error("Could not delete the student");
                    return Json(false);
                }
            }
            catch(Exception ex)
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
        public async Task<JsonResult> GetSemesters()
        {
            return Json(await _semesterService.GetSemesters());
        }

    }
}
