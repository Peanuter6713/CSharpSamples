using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.DataBase;
using XieCheng.Services;

namespace XieCheng
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                //setupAction.OutputFormatters.Add(
                //        new XmlDataContractSerializerOutputFormatter()
                //    );
            })
            // 绑定JSONPatch 和 JSON 
            .AddNewtonsoftJson(setupAction =>
            {
                // 配置框架
                setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters() // 接收 响应请求时，支持XML格式
            // 验证数据是否非法
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "都行",
                        Title = "数据验证失败",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "请看详细说明",
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetail)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>();

            services.AddDbContext<AppDbContext>(option =>
            {
                //option.UseSqlServer(connectionString);
                //option.UseSqlServer(Configuration["DbContext:SqlServerConnectionString"]);
                option.UseMySql(Configuration["DbContext:MySqlConnectionString"], ServerVersion.AutoDetect(Configuration["DbContext:MySqlConnectionString"]));
            });

            // 扫描 profile 文件
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Make sure the CORS middleware is ahead of SignalR.
            //app.UseCors(builder =>
            //{
            //    builder.WithOrigins("https://localhost")
            //        .AllowAnyHeader()
            //        .WithMethods("GET", "POST", "DELETE")
            //        .AllowCredentials();
            //});

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});

                endpoints.MapControllers();

            });
        }
    }
}
