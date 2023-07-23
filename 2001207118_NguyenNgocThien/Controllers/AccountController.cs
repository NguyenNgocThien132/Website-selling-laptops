using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using _2001207118_NguyenNgocThien.ViewModel;
using _2001207118_NguyenNgocThien.Identity;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.IO;

using Microsoft.AspNet.Identity.EntityFramework;

namespace _2001207118_NguyenNgocThien.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext appDbContext = new AppDbContext();
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM rvm)
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
                    authenManager.SignIn(new AuthenticationProperties(), userIdentity);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("New Error", "Invalid Data");
                return View();
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM lvm)
        {
            var appDbContext = new AppDbContext();
            var userStore = new AppUserStore(appDbContext);
            var userManager = new AppUserManager(userStore);
            var user = userManager.Find(lvm.Username, lvm.Password);

            
            if (user != null)
            {
                var authenManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenManager.SignIn(new AuthenticationProperties(), userIdentity);
                if (userManager.IsInRole(user.Id, "Admin"))
                {
                    return RedirectToAction("index","home", new { area = "Admin"});
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            else
            {
                ModelState.AddModelError("MyError", "Invalid username and password");
                return View();
            }
        }

        public ActionResult Logout()
        {
            var authenManager = HttpContext.GetOwinContext().Authentication;
            authenManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        
        public ActionResult InfoUser(string UserName="")
        {
            
            AppUser user = appDbContext.Users.Where(row => row.UserName == UserName).FirstOrDefault();
            ViewBag.user = user;
            return View(user);
        }
        

        public ActionResult EditProfile(string UserName = "")
        {
            AppUser user = appDbContext.Users.Where(row => row.UserName == UserName).FirstOrDefault();
            ViewBag.user = user;
            return View(user);
        }
        [HttpPost]
        public ActionResult EditProfile(AppUser usertemp, HttpPostedFileBase Image, string UserName = "")
        {
            AppUser user = appDbContext.Users.Where(row => row.UserName == UserName).FirstOrDefault();

            if (Image!=null && Image.ContentLength > 0)
            {
                string FileName = Path.GetFileName(Image.FileName);
                string path = Path.Combine(Server.MapPath("~/img/User"), FileName);
                Image.SaveAs(path);
                user.Image = FileName;
            }
            else
            {
                user.Image = usertemp.Image;
            }
            user.Email = usertemp.Email;
            user.BirthDay = usertemp.BirthDay;
            user.PhoneNumber = usertemp.PhoneNumber;
            user.City = usertemp.City;
            user.Address = usertemp.Address;

            appDbContext.SaveChanges();
            return RedirectToAction("index","home");
        }
    }
}