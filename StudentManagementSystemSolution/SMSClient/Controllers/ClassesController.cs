using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SMSClient.Authentication;
using SMSClient.Constants;
using SMSClient.Service.Departments;
using SMSClient.Service.Classes;
using SMSClient.Model;

namespace SMSClient.Controllers
{
    public class ClassesController: Controller
    {
        private readonly IClassService _classService;
        private readonly IDepartmentService _departmentService;


        public ClassesController(IClassService classService, IDepartmentService departmentService)
        {
            _classService = classService;
            _departmentService = departmentService;
        }

        [CustomAuthorize(AccessLevels.View, Modules.Class)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var classes = await _classService.GetClassesOfActiveDepartment();
            return View(classes);
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Class)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Class)]
        [HttpPost]
        public async Task<IActionResult> Create(Class classForm)
        {
            try
            {
                if (!ModelState.IsValid) return View(classForm);
                await _classService.AddClass(classForm);
                Log.Information("New Class created");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Class)]
        [HttpGet]
        public async Task<IActionResult> ShowEditPage(string id)
        {
            var classModal = await _classService.GetClassById(Int32.Parse(id));
            return PartialView("_EditClassModal", classModal);
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Class)]
        [HttpPost]
        public async Task<JsonResult> AjaxUpdate(Class classForm)
        {
            try
            {
                if (!ModelState.IsValid) return Json(false);
                await _classService.UpdateClass(classForm);
                Log.Information("Successfully updated the class");
                return Json(true);                
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Class)]
        [HttpGet]
        public async Task<IActionResult> ShowDeletePage(string id)
        {
            var existingClass = await _classService.GetClassById(Int32.Parse(id));
            return PartialView("_DeleteClassModal", existingClass);
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Class)]
        [HttpPost]
        public async Task<JsonResult> AjaxDelete(string id)
        {
            try
            {
                var existingClass = await _classService.GetClassWithRelatedEntities(Int32.Parse(id));
                if (existingClass is null) return Json(false);
                await _classService.DeleteClass(existingClass);
                    Log.Information("Successfully deleted the semester");
                    return Json(true);
            }
            catch (Exception ex)
            {
                Log.Error("Could not delete semester" + ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<object> GetDepartments(DataSourceLoadOptions loadOptions)
        {
            var departments = await _departmentService.GetActiveDepartments();
            return DataSourceLoader.Load(departments, loadOptions);
        }

        [HttpGet]
        public async Task<JsonResult> GetDepartmentsForDropdown()
        {
            var departments = await _departmentService.GetActiveDepartments();
            return Json(departments);
        }

    }
}
