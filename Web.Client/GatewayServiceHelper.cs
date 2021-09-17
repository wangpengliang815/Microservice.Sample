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
            var Client = new RestClient("http://localhost:5000");
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }


        public void GetServices()
        {
            throw new NotImplementedException();
        }
    }
}
