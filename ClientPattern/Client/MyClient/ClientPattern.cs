using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace MyClient
{
    class ClientPattern
    {
        public static void Invoke()
        {
            try
            {
                Console.WriteLine("================访问发现文档=============");
                HttpClient client = new HttpClient();
                DiscoveryDocumentResponse disco = client.GetDiscoveryDocumentAsync("https://localhost:5001/").Result;
                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return;
                }

                string accessToken = null;
                Console.WriteLine("==============获取Token==============");
                {
                    {
                        Console.WriteLine("客户端模式获取Token");
                        TokenResponse tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                        {
                            // 以下属性要和授权服务器完全一致
                            Address = disco.TokenEndpoint,
                            ClientId = "ClientPattern",
                            ClientSecret = "ClientPatternSecret",
                            Scope = "Client-ApiScope"
                        }).Result;
                        accessToken = tokenResponse.AccessToken;
                    }
                }

                {
                    Console.WriteLine("调用没有权限验证的Api资源");
                    HttpClient apiClient = new HttpClient();
                    //apiClient.SetBearerToken(accessToken);
                    HttpResponseMessage response = apiClient.GetAsync("https://localhost:6001/identity/GetInit").Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                    else
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(JArray.Parse(content));
                    }
                }

                {
                    Console.WriteLine("访问有权限验证的Api资源，但是没有携带Token");
                    HttpClient apiClient = new HttpClient();
                    // apiClient.SetBearerToken(accessToken);
                    HttpResponseMessage response = apiClient.GetAsync("https://localhost:6001/identity/GetUser").Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                    else
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(JArray.Parse(content));
                    }
                }

                {
                    Console.WriteLine("访问有权限验证的Api资源，访问时携带Token");
                    HttpClient apiClient = new HttpClient();
                    apiClient.SetBearerToken(accessToken);
                    HttpResponseMessage response = apiClient.GetAsync("https://localhost:6001/identity/GetUser").Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                    else
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(JArray.Parse(content));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
