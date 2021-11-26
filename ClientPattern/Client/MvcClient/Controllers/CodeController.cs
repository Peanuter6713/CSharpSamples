using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    public class CodeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
