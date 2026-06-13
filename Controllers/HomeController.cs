using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Assignment.Task.Models;

namespace Assignment.Task.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NLog.Logger _nlogger = NLog.LogManager.GetLogger(nameof(HomeController));

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
        _nlogger.Error("Rendering Error page for RequestId {0}", model.RequestId);
        return View(model);
    }
}
