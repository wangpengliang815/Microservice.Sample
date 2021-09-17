using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System;

namespace Order.Api.Controller
{
    [Route("[Controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration configuration;

        OrdersController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string result = $"订单服务：{DateTime.Now:yyyy-MM-dd HH:mm:ss},-{Request.HttpContext.Connection.LocalIpAddress}:{configuration["ConsulSetting:ServicePort"]}";
            return Ok(result);
        }

        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
