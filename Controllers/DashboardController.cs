using Assignment.Task.Helpers;
using Assignment.Task.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Task.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly MaterialHelper _materialHelper;
        private readonly StockManagementHelper stockManagementHelper;
        private readonly NLog.Logger _logger = NLog.LogManager.GetLogger(nameof(DashboardController));

        public DashboardController(IConfiguration IConfig)
        {
            _materialHelper = new MaterialHelper(IConfig);
            stockManagementHelper = new StockManagementHelper(IConfig);

        }
        public IActionResult Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            _logger.Info("Loading dashboard data");
            dashboardViewModel.TotalMaterial = _materialHelper.GetMaterials().Count;
            dashboardViewModel.LowStockItem = _materialHelper.GetLowStockMaterials();
            dashboardViewModel.StockIn = stockManagementHelper.GetStockInTransactions();
            dashboardViewModel.StockOut = stockManagementHelper.GetStockOutTransactions();

            return View(dashboardViewModel);
        }
    }
}
