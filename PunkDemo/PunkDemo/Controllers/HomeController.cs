
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace PunkDemo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return @"Example routes:
            Task 1: api/demo/rate?id={4}
            Task 2: api/demo/search?query=buz";
        }
    }
}
