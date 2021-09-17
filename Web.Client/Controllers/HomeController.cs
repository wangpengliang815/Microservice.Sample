using Consul;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Web.Client.Models;

namespace Web.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IServiceHelper serviceHelper;

        public HomeController(ILogger<HomeController> logger, IServiceHelper serviceHelper)
        {
            this.logger = logger;
            this.serviceHelper = serviceHelper;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.OrderData = await serviceHelper.GetOrder();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
  
}
