using Assignment.Task.Helpers;
using Assignment.Task.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Assignment.Task.Controllers
{
    public class StockTransactionsController : Controller
    {
        private readonly StockManagementHelper _stkManagementHelper;

        private readonly MaterialHelper _materialHelper;

		private readonly NLog.Logger _nlogger = NLog.LogManager.GetLogger(nameof(StockTransactionsController));

		public StockTransactionsController(IConfiguration iConfig)
        {
			_stkManagementHelper = new StockManagementHelper(iConfig);
            _materialHelper = new MaterialHelper(iConfig);
        }
        public IActionResult Index()
        {

            List<StockTransaction> stockTransactions = _stkManagementHelper.GetPreviousTransactions(Top:false);
			_nlogger.Info("{0} StockTransactions retreived by UserId : {1}", stockTransactions.Count, User.FindFirstValue("userId"));

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

            if (_stkManagementHelper.InsertStockTranasction(model))
            {
                _nlogger.Info("Stock Transction was created with the following data : {0} by UserId : {1}", model.ToString(), User.FindFirstValue("userId"));

            }
            else
            {
                _nlogger.Info("Faield to create Stock Transction with the following data : {0} by UserId : {1}", model.ToString(), User.FindFirstValue("userId"));

            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int Id)
        {
			_stkManagementHelper.DeleteStockTransaction(Id);
			_nlogger.Info("Stock Transction was Deleted with the following Id : {0} by UserId : {1}", Id, User.FindFirstValue("userId"));

			return RedirectToAction(nameof(Index));
		}
    }
}
