using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using _2001207118_NguyenNgocThien.Models;

namespace _2001207118_NguyenNgocThien.APIControllers
{
    public class ThuongHieuController : ApiController
    {

        public List<ThuongHieu> Get()
        {
            ShopDBContext db = new ShopDBContext();
            List<ThuongHieu> th = db.ThuongHieus.ToList();
            return th;
        }
        public ThuongHieu GetThuongHieuById(int id)
        {
            ShopDBContext db = new ShopDBContext();
            ThuongHieu thuongHieu = db.ThuongHieus.Where(row => row.ID == id).FirstOrDefault();
            return thuongHieu;
        }

    }
}
