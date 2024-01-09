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
using SMSClient.Model;
using SMSClient.Service.Courses;
using SMSClient.Service.Departments;
using SMSClient.Service.Classes;
using SMSClient.Service.Students;

namespace SMSClient.Controllers
{
    public class DepartmentsController: Controller
    {

        private readonly IDepartmentService _departmentService;
        private readonly ICourseService _courseService;

        public DepartmentsController(IDepartmentService departmentService, ICourseService courseService)
        {
            _departmentService = departmentService;
            _courseService = courseService;
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
        public async Task<IActionResult> Create(Department department)
        {
            try
            {
                if (!ModelState.IsValid) return View(department);
                await _departmentService.AddDepartment(department);
                Log.Information("New Department Created");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while saving the department" + ex.Message);
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
                await _departmentService.UpdateDepartment(department);
                Log.Information("Successfully updated the department");
                return Json(true);
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
                var department = await _departmentService.GetDepartmentWithClassInfo(Int32.Parse(id));
                if (department is null) return Json(false);
                await _departmentService.DeleteDepartment(department);
                Log.Information("Successfully deleted the department");
                return Json(true);
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
            var courses = _courseService.GetUnassignedCoursesofDepartment();
            return DataSourceLoader.Load(courses,loadOptions);
        }

        //[HttpGet]
        //public async Task<object> GetSemesters(DataSourceLoadOptions loadOptions)
        //{
        //    var semesters = await _semesterService.GetSemesters();
        //    return DataSourceLoader.Load(semesters,loadOptions);
        //}

        //[HttpGet]
        //public async Task<object> GetUnassignedStudents(DataSourceLoadOptions loadOptions)
        //{
        //    var students = await _studentService.GetStudentsUnAssignedToAnyDepartment();
        //    return DataSourceLoader.Load(students, loadOptions);
        //}
    }
}
