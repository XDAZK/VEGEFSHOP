using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRau.Models;
using PagedList;
using PagedList.Mvc;

namespace WebBanRau.Controllers
{
    public class HienThiSanPhamController : Controller
    {
        QLRauCuDataContext context = new QLRauCuDataContext();
        private List<SANPHAM> ListSP(int count)
        {
            return context.SANPHAMs.OrderByDescending(a => a.MaSP).Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var Sachmoi = ListSP(20);
            return View(Sachmoi.ToPagedList(pageNum, pagesize));
        }
        //Loai SP
        public ActionResult LoaiSP()
        {
            var loaisp = from sp in context.LOAISANPHAMs select sp;
            return View(loaisp);
        }

        public ActionResult Loai(int? id, int? page)
        {

            if (id == null)
            {
                return HttpNotFound();
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var SPLoai = from sp in context.SANPHAMs where sp.MaloaiSP == id select sp;
            return View(SPLoai.ToPagedList(pageNum, pagesize));
        }


        public ActionResult AllSP(int? page)
        {
            var allSP = from sp in context.SANPHAMs select sp;

            int pagesize = 8;
            int pageNum = (page ?? 1);
            return View(allSP.ToPagedList(pageNum, pagesize));
        }
        //Het Loai SP

        //Nhan hieu
        public ActionResult NhanHieu()
        {
            var loaisp = from sp in context.NCCs select sp;
            return View(loaisp);
        }
        public ActionResult ThuocNhanHieu(int? id, int? page)
        {

            if (id == null)
            {
                return HttpNotFound();
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var ThuocNCC = from sp in context.SANPHAMs where sp.MaNCC == id select sp;
            return View(ThuocNCC.ToPagedList(pageNum, pagesize));
        }
        //Het Nhan hieu
        public ActionResult ChitietSP(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var chitietSP = from s in context.SANPHAMs where s.MaSP == id select s;
            return View(chitietSP.Single());
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(FormCollection collection)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "NguoiDung");
            }
            else
            {


                HOTRO ht = new HOTRO();
                KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];

                ht.Email = kh.Email;
                ht.MaKH = kh.MaKH;
                ht.HoTen = kh.HoTen;



                string lydo = collection["LyDo"];
                ht.LyDo = lydo;
                if (lydo == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    context.HOTROs.InsertOnSubmit(ht);
                    context.SubmitChanges();
                    return RedirectToAction("Contact", "HienThiSanPham");
                }


            }
        }

        
        public ActionResult News()
        {
            return View();
        }
        
        
    }
}