using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using SMSClient.Authentication;
using SMSClient.Constants;
using SMSClient.Data.Identity;
using SMSClient.Models;
using SMSClient.Models.ViewModel;
using SMSClient.Service.Courses;
using SMSClient.Service.Departments;
using SMSClient.Service.Semesters;
using SMSClient.Service.Students;

namespace SMSClient.Controllers
{
    public class DepartmentsController: Controller
    {
        private readonly AspIdUsersContext _context;

        private readonly IDepartmentService _departmentService;
        private readonly ICourseService _courseService;
        private readonly ISemesterService _semesterService;
        private readonly IStudentService _studentService;

        public DepartmentsController(AspIdUsersContext context, IDepartmentService departmentService, ICourseService courseService, ISemesterService semesterService, IStudentService studentService)
        {
            _context = context;
            _departmentService = departmentService;
            _courseService = courseService;
            _semesterService = semesterService;
            _studentService = studentService;
        }


        [CustomAuthorize(AccessLevels.View, Modules.Department)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var departments = await _departmentService.GetDepartments();
                return View(departments);
            }
            catch(Exception ex)
            {
                Log.Error("An error occured while fetching departments: " + ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Department)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Department)]
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentForm)
        {
            try
            {
                if (!ModelState.IsValid) return View(departmentForm);
                var result = await _departmentService.AddDepartment(departmentForm);
                if (!result)
                {
                    Log.Error("An error occured while creating department");
                    return View(departmentForm);
                }
                Log.Information("Create department");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Department)]
        [HttpGet]
        public async Task<IActionResult> ShowEditPage(string id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentById(Int32.Parse(id));
                return PartialView("_EditDepartmentModal", department);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Department)]
        [HttpPost]
        public async Task<JsonResult> AjaxUpdate(Department department)
        {
            try
            {
                if (!ModelState.IsValid) return Json(false);
                var result = await _departmentService.UpdateDepartment(department);
                if (result)
                {
                    Log.Information("Successfully updated the department");
                    return Json(true);
                }
                else return Json(false);
            }
            catch(Exception ex)
            {
                Log.Error("Error while updating the department " + ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Department)]
        [HttpGet]
        public async Task<IActionResult> ShowDeletePage(string id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentById(Int32.Parse(id));
                return PartialView("_DeleteDepartmentModal", department);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Department)]
        [HttpPost]
        public async Task<JsonResult> AjaxDelete(string id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentById(Int32.Parse(id));
                if (department is null) return Json(false);
                var result = await _departmentService.DeleteDepartment(department);
                if (result)
                {
                    Log.Information("Successfully deleted the department");
                    return Json(true);
                }
                else return Json(false);
            }
            catch(Exception ex)
            {
                Log.Error("Error while deleting the department" + ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<object> GetUnassignedCourses(DataSourceLoadOptions loadOptions)
        {
            var courses = await _courseService.GetUnassignedCoursesofDepartment();
            return DataSourceLoader.Load(courses,loadOptions);
        }

        [HttpGet]
        public async Task<object> GetSemesters(DataSourceLoadOptions loadOptions)
        {
            var semesters = await _semesterService.GetSemesters();
            return DataSourceLoader.Load(semesters,loadOptions);
        }

        [HttpGet]
        public async Task<object> GetUnassignedStudents(DataSourceLoadOptions loadOptions)
        {
            var students = await _studentService.GetStudentsUnAssignedToAnyDepartment();
            return DataSourceLoader.Load(students, loadOptions);
        }
    }
}
