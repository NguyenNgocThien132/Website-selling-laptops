using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _2001207118_NguyenNgocThien.Models;

namespace _2001207118_NguyenNgocThien.Controllers
{
    public class HomeController : Controller
    {
        ShopDBContext db = new ShopDBContext();
        // GET: Home

        public ActionResult TrangChu(int Page = 1)
        {
            List<SanPham> sp = db.SanPhams.ToList();
            //Paging
            int NoOfRecordPerPage = 15;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(sp.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (Page - 1) * NoOfRecordPerPage;
            ViewBag.Page = Page;
            ViewBag.NoOfPages = NoOfPages;

            sp = sp.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            return View(sp);
        }
        public ActionResult Index()
        {
            List<SanPham> SP = db.SanPhams.ToList();
            return View(SP);
        }

        

        public ActionResult Acer()
        {
            List<SanPham> SP = db.SanPhams.ToList();
            return View(SP);
        }

        public ActionResult ASUS()
        {
            List<SanPham> SP = db.SanPhams.ToList();
            return View(SP);
        }

        public ActionResult MSI()
        {
            List<SanPham> SP = db.SanPhams.ToList();
            return View(SP);
        }
        public ActionResult DELL()
        {
            List<SanPham> SP = db.SanPhams.ToList();
            return View(SP);
        }
        public ActionResult Apple()
        {
            List<SanPham> SP = db.SanPhams.ToList();
            return View(SP);
        }
        public ActionResult Error404()
        {
            return View();
        }
    }
}