using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
using XieCheng.Models;
using XieCheng.Services;

namespace XieCheng.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ITouristRouteRepository touristRouteRepository;

        public AuthenticateController(IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITouristRouteRepository touristRouteRepository)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.touristRouteRepository = touristRouteRepository;
        }


        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // 验证用户名和密码
            var loginResult = await signInManager.PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                false,
                false
                );

            if (!loginResult.Succeeded)
            {
                return BadRequest();
            }

            var user = await userManager.FindByNameAsync(loginDto.Email);

            // create jwt
            // header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            // payload
            var claims = new List<Claim>
            {
                // sub
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                //new Claim(ClaimTypes.Role, "Admin")
            };

            var roleNames = await userManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                var roleClaim = new Claim(ClaimTypes.Role, roleName);
                claims.Add(roleClaim);
            }

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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // 1. 使用用户名创建用户对象
            var user = new ApplicationUser()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            // 2. hash密码，保存用户
            var result = await userManager.CreateAsync(user, registerDto.Password);

            // 3. Initialize Shopping Cart
            var shoppingCart = new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id
            };
            await this.touristRouteRepository.CreateShoppingCartAsync(shoppingCart);
            await this.touristRouteRepository.SaveAsync();

            // 4. return
            return result.Succeeded ? Ok() : BadRequest();
        }

    }
}
