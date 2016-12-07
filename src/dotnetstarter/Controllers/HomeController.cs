using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly string dbConnection; 
        public HomeController()
        {
            dbConnection = Startup.ConnectionString;
        }

        public IActionResult Index()
        {
            var entry        = new AccessLog()
            {
                AccessUrl     = string.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString),
                AccessIPAddr  = GetRequestIP(),
                AccessDate    = DateTime.UtcNow.AddHours(9),
                BrowserType   = Request.Headers["User-Agent"].ToString()
            };

            try
            {
                using (var context = AccessLogContextFactory.Create(dbConnection))
                {
                    context.Add(entry);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.StackTrace;
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProductDetail()
        {
            var reqProductNo = Convert.ToInt32(Request.Query["productNo"]);
            var product      = new Product();

            try
            {
                using (var context = ProductContextFactory.Create(dbConnection))
                {
                    product = context.tproduct.Where(P => P.ProductNo.Equals(reqProductNo)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.StackTrace;
            }

            return Json(product);
        }

        public string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            var forIPAddr = HttpContext.Request.Headers["X-Client-Ip"].FirstOrDefault();
            if (string.IsNullOrEmpty(forIPAddr))
            {
                forIPAddr = HttpContext.Connection.RemoteIpAddress.ToString();
            }

            return forIPAddr;
        }
    }
}
