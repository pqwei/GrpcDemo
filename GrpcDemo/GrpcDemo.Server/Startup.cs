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
        /// Api�汾��Ϣ
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
                // ��ѡ��ΪtrueʱAPI����֧�ֵİ汾��Ϣ
                option.ReportApiVersions = true;
                // ���ṩ�汾ʱ��Ĭ��Ϊ1.0
                option.AssumeDefaultVersionWhenUnspecified = true;
                // ������δָ���汾ʱĬ��Ϊ1.0
                option.DefaultApiVersion = new ApiVersion(1, 0);
            }).AddVersionedApiExplorer(option =>
            {��������������������// �汾���ĸ�ʽ��v+�汾��
                option.GroupNameFormat = "'v'V";
                option.AssumeDefaultVersionWhenUnspecified = true;
            });

            this.provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            // ע��Swagger����
            services.AddSwaggerGen(c =>
            {
                // ��汾����
                foreach (var item in provider.ApiVersionDescriptions)
                {
                    // ����ĵ���Ϣ
                    c.SwaggerDoc(item.GroupName, new OpenApiInfo
                    {
                        Title = "CoreWebApi",
                        Version = item.ApiVersion.ToString(),
                        Description = "ASP.NET CORE WebApi",
                        Contact = new OpenApiContact
                        {
                            Name = "κ��ǿ",
                            Email = "1531258932@qq.com",
                            Url = new Uri("https://cn.bing.com/")
                        }
                    });
                }
                #region ��ȡxml��Ϣ

                //ʹ�÷����ȡxml�ļ�����������ļ���·��
               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ����xmlע��. �÷����ڶ����������ÿ�������ע�ͣ�Ĭ��Ϊfalse.
                c.IncludeXmlComments(xmlPath, true);
                #endregion

                #region ����swagger��֤����
                //���һ�������ȫ�ְ�ȫ��Ϣ����AddSecurityDefinition����ָ���ķ�������һ�¼��ɣ�CoreAPI��
                //var security = new Dictionary<string, IEnumerable<string>> { { "CoreAPI", new string[] { } }, };
                //c.AddSecurityRequirement(security);
                //c.AddSecurityDefinition("CoreAPI", new ApiKeyScheme
                //{
                //    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ���·�����Bearer {token} ����",
                //    Name = "Authorization",//jwtĬ�ϵĲ�������
                //    In = "header",//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
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
            // ����SwaggerUI
            app.UseSwaggerUI(c =>
            {
                foreach (var item in provider.ApiVersionDescriptions)
                {
                    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreAPI"); ���汾
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
