using Assignment.Task.Helpers;
using Assignment.Task.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Task.Controllers
{
    public class StockTransactionsController : Controller
    {
        private readonly StockManagementHelper _stkManagementHelper;

        private readonly MaterialHelper _materialHelper;

        public StockTransactionsController(IConfiguration iConfig)
        {
			_stkManagementHelper = new StockManagementHelper(iConfig);
            _materialHelper = new MaterialHelper(iConfig);
        }
        public IActionResult Index()
        {

            List<StockTransaction> stockTransactions = _stkManagementHelper.GetPreviousTransactions(Top:false);

            return View(stockTransactions);
        }

        public IActionResult Create()
        {
            List<Material> materials = _materialHelper.GetMaterials();
            return View(materials);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StockTransaction model)
        {

            _stkManagementHelper.InsertStockTranasction(model);
			return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int Id)
        {
			_stkManagementHelper.DeleteStockTransaction(Id);
			return RedirectToAction(nameof(Index));
		}
    }
}
