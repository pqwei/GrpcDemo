﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcDeal;
using GrpcDemo.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public string Get()
        {
            try
            {

                var httpClientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };
                var httpClient = new HttpClient(httpClientHandler);
                var channel = GrpcChannel.ForAddress("https://localhost:8080", new GrpcChannelOptions { HttpClient = httpClient });

                //var channel = GrpcChannel.ForAddress("https://localhost:8080");

                //var client = new dealService.dealServiceClient(channel);
                //var reply = client.GetDeal(
                //    new DealIdRequest { Id = 1 });
                //Console.WriteLine("Greeter 服务返回数据: " + reply.Name+reply.Remark);

                var client = new Greeter.GreeterClient(channel);
                var reply = client.SayHello(
                    new HelloRequest { Name = "张三" });
                Console.WriteLine("Greeter 服务返回数据: " + reply.Message);

                return reply.Message;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
