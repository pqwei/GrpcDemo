using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcService1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GrpcDemo.Client.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
        public string Get()
        {
            //Http客户端
            var httpClientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback 
                = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };
            var httpClient = new HttpClient(httpClientHandler);

            httpClient = new HttpClient();

            //gRPC客户端
            var gRPCChannel = GrpcChannel.ForAddress("https://localhost:44355",
                new GrpcChannelOptions { HttpClient = httpClient });
            var gRPCClient = new Greeter.GreeterClient(gRPCChannel);

            List<int> timesList = new List<int> { 10, 100, 1000 };//调用次数
            int count = 10;//记录总数
            Console.WriteLine($"{count}条数据测试结果:\n");
            foreach (var times in timesList)
            {
                string httpsUrl = $"https://127.0.0.1:44355/WeatherForecast?count={count}";
                var gRPCRequest = new HelloRequest
                {
                    Name = "张三",
                    Count = count
                };
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                for (int i = 0; i < times; i++)
                {
                    var result = httpClient.GetStringAsync(httpsUrl).Result;
                    var list = JsonConvert.DeserializeObject<List<WeatherForecast>>(result);
                }
                stopWatch.Stop();
                Console.WriteLine($"循环调用{times}次WebApi:{stopWatch.ElapsedMilliseconds}ms");

                stopWatch.Restart();
                for (int i = 0; i < times; i++)
                {
                    var reply = gRPCClient.SayHello(gRPCRequest);
                }
                stopWatch.Stop();
                Console.WriteLine($"循环调用{times}次gRPC:{stopWatch.ElapsedMilliseconds}ms");

                stopWatch.Restart();
                Parallel.For(0, times, o =>
                {
                    var result = httpClient.GetStringAsync(httpsUrl).Result;
                    var list = JsonConvert.DeserializeObject<List<WeatherForecast>>(result);
                });
                stopWatch.Stop();
                Console.WriteLine($"并发调用{times}次WebApi:{stopWatch.ElapsedMilliseconds}ms");

                stopWatch.Restart();
                Parallel.For(0, times, o =>
                {
                    var reply = gRPCClient.SayHello(gRPCRequest);
                });
                stopWatch.Stop();
                Console.WriteLine($"并发调用{times}次gRPC:{stopWatch.ElapsedMilliseconds}ms\n\n");
            }

            return "结束啦!";
        }

        public class WeatherForecast
        {
            public DateTime Date { get; set; }

            public int TemperatureC { get; set; }

            public string Summary { get; set; }
        }
    }
}
