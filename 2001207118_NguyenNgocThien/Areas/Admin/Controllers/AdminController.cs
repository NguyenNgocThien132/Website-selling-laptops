using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using _2001207118_NguyenNgocThien.Models;
using _2001207118_NguyenNgocThien.Identity;
using _2001207118_NguyenNgocThien.Filter;
using System.Web.Helpers;
using _2001207118_NguyenNgocThien.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace _2001207118_NguyenNgocThien.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class AdminController : Controller
    {
        ShopDBContext db = new ShopDBContext();
        public ActionResult QuanLy(string search = "", string SortColumn = "ID", string IconClass = "fa-sort-asc", int Page = 1)
        {
            //List<SanPham> sp = db.SanPhams.ToList();

            //search
            List<SanPham> sp = db.SanPhams.Where(row => row.TenSP.Contains(search)).ToList();
            ViewBag.Search = search;
            //sort  
            ViewBag.SortColum = SortColumn;
            ViewBag.IconClass = IconClass;
            if (SortColumn == "ID")
            {
                if (IconClass == "fa-sort-asc")
                {
                    sp = sp.OrderBy(row => row.ID).ToList();
                }
                else
                {
                    sp = sp.OrderByDescending(row => row.ID).ToList();
                }
            }
            else if (SortColumn == "TenSP")
            {
                if (IconClass == "fa-sort-asc")
                {
                    sp = sp.OrderBy(row => row.TenSP).ToList();
                }
                else
                {
                    sp = sp.OrderByDescending(row => row.TenSP).ToList();
                }
            }
            else if (SortColumn == "GiaTien")
            {
                if (IconClass == "fa-sort-asc")
                {
                    sp = sp.OrderBy(row => row.GiaTien).ToList();
                }
                else
                {
                    sp = sp.OrderByDescending(row => row.GiaTien).ToList();
                }
            }

            //Paging
            int NoOfRecordPerPage = 20;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(sp.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (Page - 1) * NoOfRecordPerPage;
            ViewBag.Page = Page;
            ViewBag.NoOfPages = NoOfPages;

            sp = sp.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            return View(sp);
        }
        public ActionResult ChiTietSP(int id)
        {
            ChiTietSanPham ctsp = db.ChiTietSanPhams.Where(row => row.IDSanPham == id).FirstOrDefault();
            SanPham sp = db.SanPhams.Where(row => row.ID == id).FirstOrDefault();
            ViewBag.sp = sp;
            return View(ctsp);
        }
        public ActionResult Insert()
        {
            List<ThuongHieu> th = db.ThuongHieus.ToList();
            List<TheLoai> tl = db.TheLoais.ToList();
            ViewBag.tl = tl;
            ViewBag.th = th;
            return View();
        }
        [HttpPost]
        public ActionResult Insert(SanPham sanpham, HttpPostedFileBase HinhAnh)
        {
            List<ThuongHieu> th = db.ThuongHieus.ToList();
            ViewBag.th = th;
            string FileName = Path.GetFileName(HinhAnh.FileName);
            string path = Path.Combine(Server.MapPath("~/img/All"), FileName);
            HinhAnh.SaveAs(path);
            sanpham.HinhAnh = FileName;
            db.SanPhams.Add(sanpham);
            db.SaveChanges();
            return RedirectToAction("quanly");
        }
        public ActionResult DeleteSanPham(int id)
        {
            SanPham sp = db.SanPhams.Where(row => row.ID == id).FirstOrDefault();
            ChiTietSanPham ctsp = db.ChiTietSanPhams.Where(row => row.IDSanPham == id).FirstOrDefault();
            return View(sp);
        }
        [HttpPost]
        public ActionResult DeleteSanPham(int id, SanPham p)
        {
            SanPham sp = db.SanPhams.Where(row => row.ID == id).FirstOrDefault();
            ChiTietSanPham ctsp = db.ChiTietSanPhams.Where(row => row.IDSanPham == id).FirstOrDefault();
            if (ctsp != null)
            {
                db.ChiTietSanPhams.Remove(ctsp);
                db.SaveChanges();
            }
            db.SanPhams.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("quanly");
        }
        public ActionResult Edit(int id)
        {
            SanPham sp = db.SanPhams.Where(row => row.ID == id).FirstOrDefault();
            List<ThuongHieu> th = db.ThuongHieus.ToList();
            ViewBag.th = th;
            List<TheLoai> tl = db.TheLoais.ToList();
            ViewBag.tl = tl;
            return View(sp);
        }
        [HttpPost]
        public ActionResult Edit(int id, SanPham sanpham, HttpPostedFileBase HinhAnh)
        {
            SanPham sp = db.SanPhams.Where(row => row.ID == id).FirstOrDefault();
            List<ThuongHieu> th = db.ThuongHieus.ToList();
            ViewBag.th = th;
            //update SanPham
            sp.TenSP = sanpham.TenSP;
            sp.SoLuong = sanpham.SoLuong;
            sp.GiaTien = sanpham.GiaTien;
            sp.GiaGiam = sanpham.GiaGiam;
            sp.IDTheLoai = sanpham.IDTheLoai;
            //sp.HinhAnh = sanpham.HinhAnh;
            if (HinhAnh != null && HinhAnh.ContentLength > 0)
            {

                string FileName = Path.GetFileName(HinhAnh.FileName);

                string path = Path.Combine(Server.MapPath("~/img/All"), FileName);
                HinhAnh.SaveAs(path);

                sp.HinhAnh = FileName;
                db.SaveChanges();
            }
            else
            {
                sp.HinhAnh = sanpham.HinhAnh;
            }
            db.SaveChanges();
            return RedirectToAction("quanly");
        }
        public ActionResult EditSanPham(int id)
        {
            ChiTietSanPham ctsp = db.ChiTietSanPhams.Where(row => row.IDSanPham == id).FirstOrDefault();
            SanPham sp = db.SanPhams.Where(row => row.ID == id).FirstOrDefault();
            ViewBag.sp = sp;
            return View(ctsp);
        }
        [HttpPost]
        public ActionResult EditSanPham(int ID, ChiTietSanPham chitietsp)
        {
            ChiTietSanPham ctsp = db.ChiTietSanPhams.Where(row => row.IDSanPham == ID).FirstOrDefault();
            //SanPham sp = db.SanPhams.Where(row => row.ID == id).FirstOrDefault();
            //update ChiTietSanPham
            ctsp.CPU = chitietsp.CPU;
            ctsp.ManHinh = chitietsp.ManHinh;
            ctsp.RAM = chitietsp.RAM;
            ctsp.DoHoa = chitietsp.DoHoa;
            ctsp.LuuTru = chitietsp.LuuTru;
            ctsp.HeDieuHanh = chitietsp.HeDieuHanh;
            ctsp.Pin = chitietsp.Pin;
            ctsp.KhoiLuong = chitietsp.KhoiLuong;

            db.SaveChanges();
            return RedirectToAction("quanly");
        }
        AppDbContext AppDbContext = new AppDbContext();
        public ActionResult QuanLyTaiKhoan()
        {
            List<AppUser> user = AppDbContext.Users.ToList();
            return View(user);
        }
        public ActionResult Create_TK()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create_TK(RegisterVM rvm)
        {
            if (ModelState.IsValid)
            {
                var appDbContext = new AppDbContext();
                var userStore = new AppUserStore(appDbContext);
                var userManager = new AppUserManager(userStore);
                var passwdHash = Crypto.HashPassword(rvm.Password);
                var user = new AppUser()
                {
                    Email = rvm.Email,
                    UserName = rvm.Username,
                    PasswordHash = passwdHash,
                    City = rvm.City,
                    BirthDay = rvm.DateOfBirth,
                    Address = rvm.Address,
                    PhoneNumber = rvm.Mobile
                };
                IdentityResult identityResult = userManager.Create(user);
                if (identityResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Customer");

                    var authenManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                }
                return RedirectToAction("QuanLyTaiKhoan", "Admin");
            }
            else
            {
                ModelState.AddModelError("New Error", "Invalid Data");
                return View();
            }
        }
        public ActionResult EditUser(string name)
        {
            AppUser user = AppDbContext.Users.Where(row => row.UserName == name).FirstOrDefault();
            ViewBag.user = user;
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(string name, AppUser appUser)
        {
            AppUser user = AppDbContext.Users.Where(row => row.UserName == name).FirstOrDefault();

            user.Email = appUser.Email;
            user.PhoneNumber = appUser.PhoneNumber;
            user.City = appUser.City;
            user.Address = appUser.Address;
            AppDbContext.SaveChanges();

            return RedirectToAction("QuanLyTaiKhoan");
        }

        public ActionResult DeleteUser(string name)
        {
            AppUser user = AppDbContext.Users.Where(row => row.UserName == name).FirstOrDefault();
            ViewBag.user = user;
            return View(user);
        }
        [HttpPost]
        public ActionResult DeleteUser(string name, AppUser appUser)
        {
            AppUser user = AppDbContext.Users.Where(row => row.UserName == name).FirstOrDefault();

            AppDbContext.Users.Remove(user);
            AppDbContext.SaveChanges();

            return RedirectToAction("QuanLyTaiKhoan");
        }
        

        public ActionResult Create_ThuongHieu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create_ThuongHieu(ThuongHieu th)
        {
            db.ThuongHieus.Add(th);
            db.SaveChanges();
            return RedirectToAction("ThuongHieu");
        }
        public ActionResult Delete_ThuongHieu(int id)
        {
            ThuongHieu th = db.ThuongHieus.Where(row => row.ID == id).FirstOrDefault();
            return View(th);
        }
        [HttpPost]
        public ActionResult Delete_ThuongHieu(int id, ThuongHieu thuonghieu)
        {
            ThuongHieu th = db.ThuongHieus.Where(row => row.ID == id).FirstOrDefault();
            db.ThuongHieus.Remove(th);
            db.SaveChanges();
            return RedirectToAction("ThuongHieu");
        }

        public ActionResult Them_ChiTietSanPham(int id)
        {
            SanPham sp = db.SanPhams.Where(row => row.ID == id).FirstOrDefault();
            return View(sp);
        }
        [HttpPost]
        public ActionResult Them_ChiTietSanPham(int id, ChiTietSanPham chitietsanpham)
        {
            SanPham sp = db.SanPhams.Where(row => row.ID == id).FirstOrDefault();
            ViewBag.sp = sp;
            db.ChiTietSanPhams.Add(chitietsanpham);
            db.SaveChanges();
            return RedirectToAction("QuanLy");
        }

        public ActionResult ThuongHieu()
        {
            List<ThuongHieu> thuongHieus = db.ThuongHieus.ToList();
            return View(thuongHieus);
        }
    }
}