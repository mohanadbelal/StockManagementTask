using Assignment.Task.Helpers;
using Assignment.Task.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using NLog;
using System.Security.Claims;

namespace Assignment.Task.Controllers
{
    [Authorize]
    public class MaterialsController : Controller
    {

        private readonly MaterialHelper _materialHelper;
		private readonly NLog.Logger _nlogger = NLog.LogManager.GetLogger(nameof(MaterialsController));

		public MaterialsController(IConfiguration iConfig,ILogger<MaterialsController> logger)
        {
            _materialHelper = new MaterialHelper(iConfig);
			

		}


		public IActionResult Index()
        {
            List<Material> materials = _materialHelper.GetMaterials();

            _nlogger.Info("{0} Materials retreived by UserId : {1}", materials.Count, User.FindFirstValue("userId"));


            return View(materials);
        }

 
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Material model)
        {

            if (_materialHelper.Upsert(model))
            {
                _nlogger.Info("Material was created with the following data : {0} by UserId : {1}", model.ToString(), User.FindFirstValue("userId"));

            }
            else
            {
                _nlogger.Info("Failed to create Material with the following data : {0} by UserId : {1}", model.ToString(), User.FindFirstValue("userId"));

            }


            return RedirectToAction(nameof(Index));
        }

    
        public IActionResult Edit(int id)
        {
            Material? material = _materialHelper.GetMaterialById(id);
            if (material == null)
            {
                return NotFound();
            }
			_nlogger.Info("Material edit requst by UserId :  {1} , with the following data : {0}  :", material.ToString(), User.FindFirstValue("userId"));

			return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Material model)
        {
            if (_materialHelper.Upsert(model))
            {
                _nlogger.Info("Material was Edited with the following data : {0} by UserId : {1}", model.ToString(), User.FindFirstValue("userId"));

            }
            else
            {
                _nlogger.Info("Failed to Edit Material with the following data : {0} by UserId : {1}", model.ToString(), User.FindFirstValue("userId"));

            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            _materialHelper.DeleteMaterial(id);
			_nlogger.Info("Material was Deleted with the following data : MaterialId :{0} by UserId : {1}", id, User.FindFirstValue("userId"));
			return RedirectToAction(nameof(Index));
        }

    }
}
