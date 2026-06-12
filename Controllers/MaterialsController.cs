using Assignment.Task.Helpers;
using Assignment.Task.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Task.Controllers
{
    public class MaterialsController : Controller
    {

        private readonly MaterialHelper _materialHelper;

        public MaterialsController(IConfiguration iConfig)
        {
            _materialHelper = new MaterialHelper(iConfig);
        }


        public IActionResult Index()
        {
            List<Material> materials = _materialHelper.GetMaterials();
            return View(materials);
        }

        [Authorize(Roles = ("Admin"))]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Material model)
        {
            _materialHelper.Upsert(model);


            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = ("Admin"))]
        public IActionResult Edit(int id)
        {
            Material? material = _materialHelper.GetMaterialById(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Material model)
        {
            _materialHelper.Upsert(model);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = ("Admin"))]
        public IActionResult Delete(int id)
        {
            _materialHelper.DeleteMaterial(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
