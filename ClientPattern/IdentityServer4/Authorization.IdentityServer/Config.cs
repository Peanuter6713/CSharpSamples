// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Authorization.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(), // 内置的信息
                new IdentityResources.Profile(), // 内置的信息
                new IdentityResources.Email
                {
                    Enabled = true, // 是否启用，默认为TRUE
                    DisplayName = "修改过得DisplayName", // 显示的名称，如在同意界面中将使用此值
                    Name = "修改过的身份资源的唯一名称",
                    Description = "修改过的Description", // 显示的描述，
                    Required = true // 指定用户是否可以在同意界面中取消选择范围。FALSE表示取消，TRUE为必须。默认FALSE
                },
                new IdentityResources.Phone(), // inner setting info
                new IdentityResources.Address(),
                new IdentityResource("roles","角色信息", new List<string>{JwtClaimTypes.Role}) // 自定义信息
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource()
                {
                    Name = "Api1",
                    DisplayName = "test api1",
                    Scopes = new List<string>
                    {
                        "Client-ApiScope"
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            { 
                new ApiScope()
                {
                    Name = "Client-ApiScope"
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // 客户端模式
                new Client()
                {
                    ClientId = "ClientPattern",
                    // 无交互用户，请使用client/secret进行身份验证
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // 身份验证的机密
                    ClientSecrets =
                    {
                        new Secret("ClientPatternSecret".Sha256())
                    },
                    // scopes that client has access to
                    // 客户端有权访问的作用域 可以访问资源 必须在这里定义， 才能访问，才能体现在token中
                    AllowedScopes =
                    {
                        "Client-ApiScope" // 必须是这里声明了Api的scope，在获取Token的时候获取到
                    }
                },
                // 密码模式
                new Client
                {
                    ClientId = "PwdPattern",
                    // 无交互用户
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("PwdPatternSecret".Sha256())
                    },
                    // scopes that client has access to
                    AllowedScopes =
                    {
                        "Client-ApiScope", // 必须是这里声明了Api的scope，在获取Token的时候获取到
                        IdentityServerConstants.StandardScopes.OpenId, //必须在IdentityResources中声明过的，才能在这里使用
                        IdentityServerConstants.StandardScopes.Profile, // 必须在IdentityResources中声明过的，才能在这里使用
                        IdentityServerConstants.StandardScopes.Email, // 必须在IdentityResources中声明过的，才能在这里使用
                        IdentityServerConstants.StandardScopes.Address, // 必须在IdentityResources中声明过的，才能在这里使用
                    }
                },
                // 授权码模式
                new Client
                {
                    ClientId = "CodePattern",
                    ClientName = "MvcApplication",
                    AllowedGrantTypes = GrantTypes.Code, // 认证模式---授权码模式
                    RedirectUris =
                    {
                        "https://localhost:7001/signin-oidc", // 跳转登录到的客户端的地址
                    },
                    // RedirectUris = {"https://localhost:4001/auth.html"} // 跳转登出到的客户端的地址
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:4001/signout-callback-oidc",
                    },
                    ClientSecrets = {new Secret("CodePatternSecret".Sha256())},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Client-ApiScope"
                    },
                    // 允许将token通过浏览器传递
                    AllowAccessTokensViaBrowser = true,
                    // 是否需要同意授权，默认false
                    RequireConsent = true
                }
            };

        public static List<TestUser> Users =>
            new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "1",
                    //ProviderSubjectId = "pwdClient",
                    Username = "Richard",
                    Password = "Richard",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Richard"),
                        new Claim(JwtClaimTypes.GivenName, "Richard"),
                        new Claim(JwtClaimTypes.FamilyName, "Richard-FamilyName"),
                        new Claim(JwtClaimTypes.Email, "wetrft@qq.com"),
                        new Claim(JwtClaimTypes.WebSite, "http://www.thatc13.cn"),
                        new Claim(JwtClaimTypes.Address, @"{'street_address', 'One Hacker Way',
                        'locality':'Heidf','postal-code':69118, 'country':'Germany'}", 
                            IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
    }
}