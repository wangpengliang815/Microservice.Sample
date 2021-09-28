﻿using DotNetCore.CAP;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using Product.Api.Models;

using System;
using System.Threading.Tasks;

namespace Product.Api.Controller
{
    [Route("[Controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ICapPublisher capBus;
        private readonly ProductContext context;

#if cap
        public ProductsController(IConfiguration configuration, ICapPublisher capBus, ProductContext context)
        {
            this.configuration = configuration;
            this.capBus = capBus;
            this.context = context;
        }
#else
        public ProductsController(IConfiguration configuration, ProductContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string result = $"产品服务：{DateTime.Now:yyyy-MM-dd HH:mm:ss},-{Request.HttpContext.Connection.LocalIpAddress}:{configuration["ConsulSetting:ServicePort"]}";
            return Ok(result);
        }

#if cap
        [NonAction]
        [CapSubscribe("order.services.createorder")]
        public async Task ReduceStock(CreateOrderMessageDto message)
        {
            Console.WriteLine("message:" + JsonConvert.SerializeObject(message));
            var product = await context.Products.FirstOrDefaultAsync(p => p.ID == message.ProductID);
            product.Stock -= message.Count;
            await context.SaveChangesAsync();
        }
#endif
    }
#endif
}
