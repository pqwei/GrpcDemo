using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService1
{
    public class GreeterService : Greeter.GreeterBase
    {
        private static readonly string[] Summaries = new[]
               {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var rng = new Random();
            var list = Enumerable.Range(1, request.Count).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index).ToString(),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            var reply = new HelloReply()
            {
                Message = "Hello " + request.Name,
                Age = 10,
            };
            reply.WeatherForecasts.AddRange(list);
            return Task.FromResult(reply);
        }
    }
}
