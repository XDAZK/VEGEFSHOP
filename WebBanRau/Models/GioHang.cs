using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanRau.Models;

namespace WebBanRau.Models
{
    public class GioHang
    {
        QLRauCuDataContext context = new QLRauCuDataContext();

        public int masp { set; get; }
        public string tensp { set; get; }
        public string mota { set; get; }
        public double dongia { set; get; }
        public string anhbia { set; get; }
        public int soluong { set; get; }
        public string DVT { set; get; }
        public double thanhtien { get { return dongia * soluong; } }

        public GioHang(int MaSP)
        {
            masp = MaSP;
            SANPHAM sp = context.SANPHAMs.Single(s => s.MaSP == masp);
            mota = sp.Mota;
            tensp = sp.TenSP;
            anhbia = sp.Anhbia;
            dongia = double.Parse(sp.Giaban.ToString());
            soluong = 1;
            DVT = sp.DVT;
        }
    }
}