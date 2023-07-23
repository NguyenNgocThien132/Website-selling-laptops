using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using _2001207118_NguyenNgocThien.Models;
using _2001207118_NguyenNgocThien.Filter;

namespace _2001207118_NguyenNgocThien.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        ShopDBContext db = new ShopDBContext();
        private const string CartSession = "CartSession";
        [MyAuthenFilter]
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart!=null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult DeleteAll()
        {
            Session[CartSession] = null;
            return RedirectToAction("index");
        }

        public ActionResult Delete(int id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.SanPhams.ID == id);
            Session[CartSession] = sessionCart;
            return RedirectToAction("index");
        }
        //public JsonResult Update(string cartModel)
        //{
        //    var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
        //    var sessionCart = (List<CartItem>)Session[CartSession];

        //    foreach (var item in sessionCart)
        //    {
        //        var jsonItem = jsonCart.SingleOrDefault(x => x.SanPhams.ID == item.SanPhams.ID);
        //        if (jsonItem != null)
        //        {
        //            item.Quantity = jsonItem.Quantity;
        //        }
        //    }
        //    Session[CartSession] = sessionCart;
        //    return Json(new
        //    {
        //        status = true
        //    });
        //}
        public ActionResult Update(int id,int quantity)
        {
            SanPham product = db.SanPhams.FirstOrDefault(c => c.ID == id);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.SanPhams.ID == id))
                {

                    foreach (var item in list)
                    {
                        if (item.SanPhams.ID == id)
                        {
                            item.Quantity = quantity;
                        }
                    }
                }
                //else
                //{
                //    //tạo mới đối tượng cart item
                //    var item = new CartItem();
                //    item.SanPhams = product;
                //    item.Quantity = quantity;
                //    list.Add(item);
                //}
                ////Gán vào session
                //Session[CartSession] = list;
            }
            //else
            //{
            //    //tạo mới đối tượng cart item
            //    var item = new CartItem();
            //    item.SanPhams = product;
            //    item.Quantity = quantity;
            //    var list = new List<CartItem>();
            //    list.Add(item);
            //    //Gán vào session
            //    Session[CartSession] = list;
            //}

            return RedirectToAction("Index");
        }

        public ActionResult AddItem(int productId, int quantity)
        {

            SanPham product = db.SanPhams.FirstOrDefault(c => c.ID == productId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.SanPhams.ID == productId))
                {

                    foreach (var item in list)
                    {
                        if (item.SanPhams.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.SanPhams = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.SanPhams = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
    }
}