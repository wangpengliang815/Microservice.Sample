using Consul;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using System;

namespace Order.Api
{
    public static class ConsulHelper
    {
        /// <summary>
        /// 服务注册到consul
        /// </summary>
        /// <param name="app"></param>
        /// <param name="lifetime"></param>
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app
            , IConfiguration configuration, IHostApplicationLifetime lifetime)
        {
            var consulClient = new ConsulClient(c =>
            {
                c.Address = new Uri(configuration["ConsulSetting:ConsulAddress"]);
            });

            var registration = new AgentServiceRegistration
            {
                // 服务实例唯一标识
                ID = Guid.NewGuid().ToString(),
                // 服务名称
                Name = configuration["ConsulSetting:ServiceName"],
                // 服务IP
                Address = configuration["ConsulSetting:ServiceIP"],
                // 服务端口 因为要运行多个实例，端口不能在appsettings.json里配置，在docker容器运行时传入
                Port = int.Parse(configuration["ConsulSetting:ServicePort"]),
                Check = new AgentServiceCheck()
                {
                    // 服务启动多久后注册
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    // 健康检查时间间隔
                    Interval = TimeSpan.FromSeconds(10),
                    //健康检查地址
                    HTTP = $"http://{configuration["ConsulSetting:ServiceIP"]}:{configuration["ConsulSetting:ServicePort"]}{configuration["ConsulSetting:ServiceHealthCheck"]}",
                    // 超时时间
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            Console.WriteLine($"=>{JsonConvert.SerializeObject(registration)}");

            //服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();

            //应用程序终止时，取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}
