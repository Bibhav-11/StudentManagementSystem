using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SMSClient.Authentication;
using SMSClient.Constants;
using SMSClient.Models;
using SMSClient.Models.ViewModel;
using SMSClient.Service.Courses;
using SMSClient.Service.Departments;
using SMSClient.Service.Semesters;
using SMSClient.Service.Students;

namespace SMSClient.Controllers
{
    public class SemestersController: Controller
    {
        private readonly ISemesterService _semesterService;
        private readonly IDepartmentService _departmentService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public SemestersController(ISemesterService semesterService, IDepartmentService departmentService, IStudentService studentService, ICourseService courseService)
        {
            _semesterService = semesterService;
            _departmentService = departmentService;
            _studentService = studentService;
            _courseService = courseService;
        }

        [CustomAuthorize(AccessLevels.View, Modules.Semester)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var semesters = await _semesterService.GetSemesters();
            return View(semesters);
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Semester)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Semester)]
        [HttpPost]
        public async Task<IActionResult> Create(SemesterViewModel semesterForm)
        {
            try
            {
                if (!ModelState.IsValid) return View(semesterForm);
                var result = await _semesterService.AddSemester(semesterForm);
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while creating the department.");
                    Log.Error("Could not create the semester");
                    return View(semesterForm);
                }
                Log.Information("Successfully created the semester");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Semester)]
        [HttpGet]
        public async Task<IActionResult> ShowEditPage(string id)
        {
            var semester = await _semesterService.GetSemesterById(Int32.Parse(id));
            return PartialView("_EditSemesterModal", semester);
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Semester)]
        [HttpPost]
        public async Task<JsonResult> AjaxUpdate(Semester semester)
        {
            try
            {
                if (!ModelState.IsValid) return Json(false);
                var result = await _semesterService.UpdateSemester(semester);
                if (result)
                {
                    Log.Information("Successfully updated the semester");
                    return Json(true);
                }
                else return Json(false);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Semester)]
        [HttpGet]
        public async Task<IActionResult> ShowDeletePage(string id)
        {
            var semester = await _semesterService.GetSemesterById(Int32.Parse(id));
            return PartialView("_DeleteSemesterModal", semester);
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Semester)]
        [HttpPost]
        public async Task<JsonResult> AjaxDelete(string id)
        {
            try
            {
                var semester = await _semesterService.GetSemesterById(Int32.Parse(id));
                if (semester is null) return Json(false);
                var result = await _semesterService.DeleteSemester(semester);
                if (result)
                {
                    Log.Information("Successfully deleted the semester");
                    return Json(true);
                }
                else
                {
                    Log.Error("Could not delete semester");
                    return Json(false);
                }
            }
            catch(Exception ex)
            {
                Log.Error("Could not delete semester" + ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<object> GetDepartments(DataSourceLoadOptions loadOptions)
        {
            var departments = await _departmentService.GetDepartments();
            return DataSourceLoader.Load(departments, loadOptions);
        }

        [HttpGet]
        public async Task<object> GetUnassignedStudents(DataSourceLoadOptions loadOptions)
        {
            var students = await _studentService.GetStudentsUnAssignedToAnySemester();
            return DataSourceLoader.Load(students, loadOptions);
        }

        [HttpGet]
        public async Task<object> GetCourses(DataSourceLoadOptions loadOptions)
        {
            var courses = await _courseService.GetCourses();
            return DataSourceLoader.Load(courses, loadOptions);
        }
    }
}
