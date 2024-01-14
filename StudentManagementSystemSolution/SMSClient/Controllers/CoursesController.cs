using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using SMSClient.Authentication;
using SMSClient.Constants;
using SMSClient.Model;
using SMSClient.Service.Classes;
using SMSClient.Service.Courses;

namespace SMSClient.Controllers
{
    public class CoursesController: Controller
    {
        private readonly ICourseService _courseService;
        private readonly IClassService _classService;

        public CoursesController(ICourseService courseService, IClassService classService)
        {
            _courseService = courseService;
            _classService = classService;
        }


        [CustomAuthorize(AccessLevels.View, Modules.Course)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var courses = await _courseService.GetCoursesOfActiveClasses();
                return View(courses);
            }
            catch(Exception ex)
            {
                Log.Error("Error fetching courses " + ex.Message);
                throw;
            }
        }


        [CustomAuthorize(AccessLevels.Create, Modules.Course)]
        [HttpGet]
        public IActionResult ShowCreateCourseModal()
        {
            return PartialView("_CreateCourseModal");
        }


        [CustomAuthorize(AccessLevels.Create, Modules.Course)]
        [HttpPost]
        public async Task<JsonResult> AjaxPost(Course courseForm)
        {
            try
            {
                await _courseService.CreateCourse(courseForm);
                Log.Information("Added new course");
                return Json(new { success = true, message = "Added Successfully" });
            }
            catch (Exception ex)
            {
                Log.Error("Error while adding course " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Course)]
        [HttpGet]
        public async Task<IActionResult> EditCourse(string courseCode)
        {
            try
            {
                var course = await _courseService.GetCourseById(courseCode);
                if (course is null) return NotFound();
                return View(course);
            }
            catch(Exception ex)
            {
                Log.Error("Error while fetching course " + ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> ShowEditCourseModal(string courseCode)
        {
            try
            {
                var course = await _courseService.GetCourseById(courseCode);
                if (course is null) return NotFound();
                return PartialView("_EditCourseModal", course);
            }
            catch (Exception ex)
            {
                Log.Error("Error while fetching course " + ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Course)]
        [HttpPost]
        public async Task<IActionResult> EditCourse(Course courseForm)
        {
            try
            {
                await _courseService.UpdateCourse(courseForm);
                Log.Information("Successfully updated the course");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Log.Error("Error while updating the course " + ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Course)]
        [HttpPost]
        public async Task<JsonResult> AjaxUpdate(Course courseForm)
        {
            try
            {
                await _courseService.UpdateCourse(courseForm);
                Log.Information("Successfully updated the course");
                return Json(true);
            }
            catch (Exception ex)
            {
                Log.Error("Error while updating the course " + ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Course)]
        [HttpGet]
        public async Task<IActionResult> ShowDeleteCourseModal(string courseCode)
        {
            try
            {
                var course = await _courseService.GetCourseById(courseCode);
                return PartialView("_DeleteCourseModal", course);
            }
            catch(Exception ex)
            {
                Log.Error("Error while fetching the course " + ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Course)]
        [HttpPost]
        public async Task<JsonResult> AjaxDelete(string courseCode)
        {
            try
            {
                var course = await _courseService.GetCourseById(courseCode);
                if(course!=null) await _courseService.DeleteCourse(course);
                Log.Information("Successfully deleted the department");
                return Json(true);
            }
            catch (Exception ex)
            {
                Log.Error("Error while deleting the course " + ex.Message);
                throw;

            }
        }


        [HttpGet]
        public async Task<JsonResult> CheckIfCourseExists(string courseCode)
        {
            var course = await _courseService.GetCourseById(courseCode);
            if (course is null) return Json(false);
            else return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> GetClassesDropdown()
        {
            var classes = await _classService.GetClassesOfActiveDepartment();
            return PartialView("_ClassDropdown", classes);
        }

    }
}
