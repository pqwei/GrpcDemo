using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcDeal;
using GrpcService1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GrpcDemo.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //try
            //{
            //         AppContext.SetSwitch(
            //"System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            //var httpClientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };
            //var httpClient = new HttpClient(httpClientHandler);
            //var channel = GrpcChannel.ForAddress("https://localhost:44354", new GrpcChannelOptions { HttpClient = httpClient });

            var channel = GrpcChannel.ForAddress("https://localhost:44354");

            //var client = new dealService.dealServiceClient(channel);
            //var reply = client.GetDeal(
            //    new DealIdRequest { Id = 1 });
            //Console.WriteLine("Greeter 服务返回数据: " + reply.Name+reply.Remark);


            //HttpClient httpClient = new HttpClient(new HttpClientHandler());
            //var result = httpClient.GetStringAsync("http://10.100.0.97:44354/WeatherForecast").Result;
            //var list= JsonConvert.DeserializeObject<List<WeatherForecast>>(result);
            //var str = JsonConvert.SerializeObject(list);
              
            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(
                new HelloRequest { Name = "张三" });
            Console.WriteLine("Greeter 服务返回数据: " + reply.Message);

            return reply.WeatherForecasts.Select(o => new WeatherForecast
            {
                Summary=o.Summary,
                 TemperatureC=o.TemperatureC
            });
            //}
            //catch (Exception ex)
            //{
            //    return ex.ToString();
            //}
        }
    }
}
