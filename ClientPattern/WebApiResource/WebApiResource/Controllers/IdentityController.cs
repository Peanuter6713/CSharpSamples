using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiResource.Controllers
{
    [Route("identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// 无需授权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInit")]
        public IActionResult GetInit()
        {
            return new JsonResult(new string[]
            {
                "1","2","3","4","5"
            });
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetUser")]
        public IActionResult GetUser()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserNew")]
        [Authorize(policy: "policy")]
        public IActionResult GetUserNew()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
