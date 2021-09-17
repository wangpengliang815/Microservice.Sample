using Microsoft.AspNetCore.Mvc;
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
            ViewBag.ProductData = await serviceHelper.GetProduct();
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
    public interface IServiceHelper
    {
        Task<string> GetProduct();

        Task<string> GetOrder();
    }

    public class ServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder()
        {
            string[] serviceUrls = { "http://192.168.134.191:80", "http://192.168.134.191:81", "http://192.168.134.191:82" };

            //每次随机访问一个服务实例
            var Client = new RestClient(serviceUrls[new Random().Next(0, 3)]);

            var request = new RestRequest("/orders", Method.GET);
            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            string serviceUrl = "http://192.168.134.191:90";
            var Client = new RestClient(serviceUrl);
            var request = new RestRequest("/products", Method.GET);
            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
