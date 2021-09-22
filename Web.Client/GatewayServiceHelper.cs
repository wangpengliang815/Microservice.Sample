using RestSharp;

using System;
using System.Threading.Tasks;

namespace Web.Client
{
    /// <summary>
    /// 通过OcelotGateway调用服务
    /// </summary>
    public class GatewayServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder()
        {
            var Client = new RestClient("http://192.168.102.191:8080/");
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            var Client = new RestClient("http://192.168.102.191:8080/");
            var request = new RestRequest("/products", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            throw new NotImplementedException();
        }
    }
}
