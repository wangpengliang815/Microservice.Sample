using Consul;

using Microsoft.Extensions.Configuration;

using RestSharp;

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Client
{
    public interface IServiceHelper
    {
        Task<string> GetOrder();

        void GetServices();
    }

    public class ServiceHelper : IServiceHelper
    {
        private readonly IConfiguration configuration;
        private readonly ConsulClient consulClient;
        private ConcurrentBag<string> orderServiceUrls;

        public ServiceHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.consulClient = new ConsulClient(c =>
            {
                // consul地址
                c.Address = new Uri(configuration["ConsulSetting:ConsulAddress"]);
            });
        }

        public async Task<string> GetOrder()
        {
            if (orderServiceUrls == null)
                return await Task.FromResult("【订单服务】正在初始化服务列表...");

            var Client = new RestClient(orderServiceUrls.ElementAt(new Random().Next(0, orderServiceUrls.Count())));
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            var serviceNames = new string[] { "order.service" };
            Array.ForEach(serviceNames, p =>
            {
                Task.Run(() =>
                {
                    // WaitTime默认为5分钟
                    var queryOptions = new QueryOptions { WaitTime = TimeSpan.FromMinutes(10) };
                    while (true)
                    {
                        GetServices(queryOptions, p);
                    }
                });
            });
        }
        private void GetServices(QueryOptions queryOptions, string serviceName)
        {
            var res = consulClient.Health.Service(serviceName, null, true, queryOptions).Result;

            // 获取服务列表的响应时间等信息
            Console.WriteLine($"{DateTime.Now}获取{serviceName}：queryOptions.WaitIndex：{queryOptions.WaitIndex}  LastIndex：{res.LastIndex}");

            // 版本号不一致 说明服务列表发生变化
            if (queryOptions.WaitIndex != res.LastIndex)
            {
                queryOptions.WaitIndex = res.LastIndex;

                //服务地址列表
                var serviceUrls = res.Response.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

                if (serviceName == "order.service")
                    orderServiceUrls = new ConcurrentBag<string>(serviceUrls);
            }
        }
    }
}
