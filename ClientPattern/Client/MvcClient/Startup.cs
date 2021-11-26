using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 1. nuget System.IdentityModel.Tokens.Jwt
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear(); // Jwt映射关闭

            // 2. 注册验证授权的服务
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = "https://localhost:5001"; // 获取发现文档，获取公钥
                options.RequireHttpsMetadata = true; // 使用https 必须使用，不是https会报错
                options.ClientId = "CodePattern";
                options.ClientSecret = "CodePatternSecret";
                options.ResponseType = "code";
                options.Scope.Clear();
                options.Scope.Add("Client-ApiScope");
                options.Scope.Add("openid");
                options.Scope.Add(OidcConstants.StandardScopes.Profile);
                //options.Scope.Add(OidcConstants.StandardScopes.Email);
                //options.Scope.Add(OidcConstants.StandardScopes.Phone);
                //options.Scope.Add(OidcConstants.StandardScopes.Address);
                //options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess); // 获取到刷新token
                options.SaveTokens = true; //  表示token要存储
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 使用鉴权中间件
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
