﻿using Microsoft.AspNetCore.Mvc;

namespace Product.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Pong.");
        }
    }
}
