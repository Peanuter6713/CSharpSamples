using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XieCheng.DtoS;

namespace XieCheng.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthenticateController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // 验证用户名和密码

            // create jwt
            // header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            // payload
            var claims = new[]
            {
                // sub
                new Claim(JwtRegisteredClaimNames.Sub, "fake-used-id"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            // signature  SecretKey 需足够长、复杂
            var secretByte = Encoding.UTF8.GetBytes(configuration["Authentication:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

            var token = new JwtSecurityToken
                (
                    // 发布者    
                    issuer: configuration["Authentication:Issuer"],
                    // token 发布给谁
                    audience: configuration["Authentication:Audience"],
                    claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials
                ) ;

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            // return 200 OK + jwt
            return Ok(tokenStr);
        }

    }
}
