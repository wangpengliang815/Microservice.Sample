using Microsoft.AspNetCore.Mvc;

using System;

namespace Order.Api.Controller
{
    [Route("[Controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            string result = $"产品服务：{DateTime.Now:yyyy-MM-dd HH:mm:ss},-{Request.HttpContext.Connection.LocalIpAddress}:{Request.HttpContext.Connection.LocalPort}";
            return Ok(result);
        }
    }
}
