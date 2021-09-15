using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_Lab_clans.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}