using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GrpcDemo.Server.Services;
using GrpcService1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GrpcDemo.Server
{
    public class Startup
    {
        /// <summary>
        /// Api版本信息
        /// </summary>
        private IApiVersionDescriptionProvider provider;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(option =>
            {
                // 可选，为true时API返回支持的版本信息
                option.ReportApiVersions = true;
                // 不提供版本时，默认为1.0
                option.AssumeDefaultVersionWhenUnspecified = true;
                // 请求中未指定版本时默认为1.0
                option.DefaultApiVersion = new ApiVersion(1, 0);
            }).AddVersionedApiExplorer(option =>
            {　　　　　　　　　　// 版本名的格式：v+版本号
                option.GroupNameFormat = "'v'V";
                option.AssumeDefaultVersionWhenUnspecified = true;
            });

            this.provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            // 注册Swagger服务
            services.AddSwaggerGen(c =>
            {
                // 多版本控制
                foreach (var item in provider.ApiVersionDescriptions)
                {
                    // 添加文档信息
                    c.SwaggerDoc(item.GroupName, new OpenApiInfo
                    {
                        Title = "CoreWebApi",
                        Version = item.ApiVersion.ToString(),
                        Description = "ASP.NET CORE WebApi",
                        Contact = new OpenApiContact
                        {
                            Name = "魏朋强",
                            Email = "1531258932@qq.com",
                            Url = new Uri("https://cn.bing.com/")
                        }
                    });
                }
                #region 读取xml信息

                //使用反射获取xml文件。并构造出文件的路径
               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                c.IncludeXmlComments(xmlPath, true);
                #endregion

                #region 启用swagger验证功能
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称一致即可，CoreAPI。
                //var security = new Dictionary<string, IEnumerable<string>> { { "CoreAPI", new string[] { } }, };
                //c.AddSecurityRequirement(security);
                //c.AddSecurityDefinition("CoreAPI", new ApiKeyScheme
                //{
                //    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = "apiKey"
                //});
                #endregion

            });


            services.AddGrpc();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            // 配置SwaggerUI
            app.UseSwaggerUI(c =>
            {
                foreach (var item in provider.ApiVersionDescriptions)
                {
                    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreAPI"); 单版本
                    c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", "CoreAPI" + item.ApiVersion);
                }
                c.RoutePrefix = string.Empty;
            });
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API-v1");
            //    c.RoutePrefix = string.Empty;
            //});

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DealService>();
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("GRPC Server Success!");
                });
            });
        }
    }
}
