using DotNetCore.CAP;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Order.Api.Models;

using System;
using System.Threading.Tasks;

namespace Order.Api.Controller
{
    [Route("[Controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ICapPublisher capBus;
        private readonly OrderContext context;

        public OrdersController(IConfiguration configuration, ICapPublisher capBus, OrderContext context)
        {
            this.configuration = configuration;
            this.capBus = capBus;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string result = $"订单服务：{DateTime.Now:yyyy-MM-dd HH:mm:ss},-{Request.HttpContext.Connection.LocalIpAddress}:{configuration["ConsulSetting:ServicePort"]}";
            return Ok(result);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Models.Order order)
        {
            using (var trans = context.Database.BeginTransaction(capBus, autoCommit: true))
            {
                //业务代码
                order.CreateTime = DateTime.Now;
                context.Orders.Add(order);

                var result = await context.SaveChangesAsync() > 0;

                if (result)
                {
                    // 发布下单事件
                    await capBus.PublishAsync("order.services.createorder",
                        new CreateOrderMessageDto() { Count = order.Count, ProductID = order.ProductID });
                    return Ok();
                }
                return BadRequest();
            }
        }
    }
}
