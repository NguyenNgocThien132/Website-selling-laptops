using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _2001207118_NguyenNgocThien.Filter;

namespace _2001207118_NguyenNgocThien.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [AdminAuthorization]
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}