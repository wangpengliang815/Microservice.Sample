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
        /// <summary>
        /// 获取产品数据
        /// </summary>
        /// <returns></returns>
        Task<string> GetProduct();

        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <returns></returns>
        Task<string> GetOrder();

        /// <summary>
        /// 获取服务列表
        /// </summary>
        void GetServices();
    }

    public class ServiceHelper : IServiceHelper
    {
        private readonly IConfiguration configuration;
        private readonly ConsulClient consulClient;
        private ConcurrentBag<string> orderServiceUrls;
        private ConcurrentBag<string> productServiceUrls;

        public ServiceHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
            consulClient = new ConsulClient(c =>
            {
                c.Address = new Uri(this.configuration["ConsulSetting:ConsulAddress"]);
            });
        }

        public async Task<string> GetOrder()
        {
            if (orderServiceUrls == null)
                return await Task.FromResult("【订单服务】正在初始化服务列表...");

            //每次随机访问一个服务实例
            var Client = new RestClient(orderServiceUrls.ElementAt(new Random().Next(0, orderServiceUrls.Count())));
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            if (productServiceUrls == null)
                return await Task.FromResult("【产品服务】正在初始化服务列表...");

            //每次随机访问一个服务实例
            var Client = new RestClient(productServiceUrls.ElementAt(new Random().Next(0, productServiceUrls.Count())));
            var request = new RestRequest("/products", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            var serviceNames = new string[] { "order.service", "product.service" };
            Array.ForEach(serviceNames, p =>
            {
                Task.Run(() =>
                {
                    //WaitTime默认为5分钟
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

            //控制台打印一下获取服务列表的响应时间等信息
            Console.WriteLine($"{DateTime.Now}获取{serviceName}：queryOptions.WaitIndex：{queryOptions.WaitIndex}  LastIndex：{res.LastIndex}");

            //版本号不一致 说明服务列表发生了变化
            if (queryOptions.WaitIndex != res.LastIndex)
            {
                queryOptions.WaitIndex = res.LastIndex;

                //服务地址列表
                var serviceUrls = res.Response.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

                if (serviceName == "order.service")
                    orderServiceUrls = new ConcurrentBag<string>(serviceUrls);
                else if (serviceName == "ProductService")
                    productServiceUrls = new ConcurrentBag<string>(serviceUrls);
            }
        }
    }
}
