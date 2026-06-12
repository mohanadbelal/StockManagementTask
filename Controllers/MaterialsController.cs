using Microsoft.AspNetCore.Mvc;

namespace Assignment.Task.Controllers
{
    public class MaterialsController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Fetch materials list from database
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(object model)
        {
            // TODO: Implement create logic
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            // TODO: Fetch material by id and display in view
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, object model)
        {
            // TODO: Implement edit logic
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            // TODO: Fetch material by id for confirmation
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // TODO: Implement delete logic
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            // TODO: Fetch and display material details
            return View();
        }
    }
}
