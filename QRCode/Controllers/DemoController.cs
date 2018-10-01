using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QRCode.Controllers
{
    [Route("demo")]
    public class DemoController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("generate")]
        public IActionResult Generate(string productId)
        {
            ViewBag.productId = productId;
            return View("Index");
        }
    }
}