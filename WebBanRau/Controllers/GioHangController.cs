using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebBanRau.Models;

using System.Text;

namespace WebBanRau.Controllers
{
    public class GioHangController : Controller
    {
        QLRauCuDataContext data = new QLRauCuDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list == null)
            {
                list = new List<GioHang>();
                Session["GioHang"] = list;
            }
            return list;
        }

        public ActionResult ThemGioHang(int masp, string strUrl)
        {

            List<GioHang> gioHangs = LayGioHang();

            GioHang sp = gioHangs.Find(n => n.masp == masp);
            if (sp == null)
            {
                sp = new GioHang(masp);
                gioHangs.Add(sp);
                return Redirect(strUrl);
            }
            else
            {
                sp.soluong++;
                return Redirect(strUrl);
            }
        }

        private int TongSoLuong()
        {
            int Tongsoluong = 0;
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs != null)
            {
                Tongsoluong = gioHangs.Sum(n => n.soluong);
            }
            Session["TongSoLuong"] = Tongsoluong;
            return Tongsoluong;
        }

        private double TongTien()
        {
            double tongtien = 0;
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs != null)
            {
                tongtien = gioHangs.Sum(n => n.thanhtien);
            }
            return tongtien;
        }

        public ActionResult Giohang()
        {
            List<GioHang> gioHangs = LayGioHang();
            if (gioHangs.Count == 0)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(gioHangs);
        }

        public ActionResult SoLuongGioHang()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> gioHangs = LayGioHang();
            GioHang sessiongiohang = gioHangs.SingleOrDefault(n => n.masp == id);
            if (sessiongiohang != null)
            {
                gioHangs.RemoveAll(n => n.masp == id);
                return RedirectToAction("GioHang");
            }
            if (gioHangs.Count == 0)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int id, FormCollection f)
        {
            List<GioHang> gioHangs = LayGioHang();
            GioHang sessiongiohang = gioHangs.SingleOrDefault(n => n.masp == id);
            if (sessiongiohang != null)
            {
                sessiongiohang.soluong = int.Parse(f["Soluong"].ToString());

            }
            return RedirectToAction("Giohang");
        }

        public ActionResult RemoveAll()
        {
            List<GioHang> gioHangs = LayGioHang();
            gioHangs.Clear();
            return RedirectToAction("Index", "HienThiSanPham");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            List<GioHang> gioHangs = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(gioHangs);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {

            DONDATHANG dONDATHANG = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<GioHang> gioHangs = LayGioHang();
            dONDATHANG.MaKH = kh.MaKH;
            dONDATHANG.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dONDATHANG.Ngaygiao = DateTime.Parse(ngaygiao);
            dONDATHANG.TongTien = Decimal.Parse(TongTien().ToString());
            dONDATHANG.Tinhtranggiaohang = false;
            dONDATHANG.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(dONDATHANG);
            data.SubmitChanges();
            foreach (var item in gioHangs)
            {
                CHITIETDONHANG CT = new CHITIETDONHANG();
                CT.MaDonHang = dONDATHANG.MaDonHang;
                CT.MaSP = item.masp;
                CT.Soluong = item.soluong;
                CT.Dongia = (decimal)item.dongia;
                CT.ThanhTien = (decimal)item.thanhtien;
                data.CHITIETDONHANGs.InsertOnSubmit(CT);
            }
            data.SubmitChanges();

            //Mail xác nhận đặt hàng
            string subject = "Biên nhận";
            string mess = "Cảm ơn " + kh.HoTen + " đã đặt hàng!\n" +
                            "Mã đơn hàng: " + dONDATHANG.MaDonHang + "\n" +
                            "Ngày đặt hàng: " + String.Format("{0:dd/MM/yyyy}", dONDATHANG.Ngaydat) + "\n" +
                            "Ngày giao: " + String.Format("{0:dd/MM/yyyy}", dONDATHANG.Ngaygiao) + "\n" +
                            "Tổng tiền: " + String.Format("{0:0,0}", dONDATHANG.TongTien) + " vnđ";
            SendEmail(kh.Email, subject, mess);


            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }


        public static void SendEmail(string address, string subject, string message)
        {

            if (new EmailAddressAttribute().IsValid(address)) // check có đúng mail khách hàng
            {
                string email = "ranklatatca@gmail.com";
                var senderEmail = new MailAddress(email, "Oganica (tin nhắn tự động)");
                var receiverEmail = new MailAddress(address, "Receiver");
                var password = "sinh01227778163";
                var sub = subject;
                var body = message;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    //smtp.Send(mess);
                }
            }



        }

        /* public ActionResult thanhtoan()
         {
             KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
             CHITIETDONHANG CT = new CHITIETDONHANG();
             string SECURE_SECRET = "A3EFDFABA8653DF2342E8DAC29B51AF0";
             // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
             VPCRequest conn = new VPCRequest("https://mtf.onepay.vn/onecomm-pay/vpc.op");
             conn.SetSecureSecret(SECURE_SECRET);
             // Add the Digital Order Fields for the functionality you wish to use
             // Core Transaction Fields
             conn.AddDigitalOrderField("Title", "onepay paygate");
             conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
             conn.AddDigitalOrderField("vpc_Version", "2");
             conn.AddDigitalOrderField("vpc_Command", "pay");
             conn.AddDigitalOrderField("vpc_Merchant", "ONEPAY");
             conn.AddDigitalOrderField("vpc_AccessCode", "D67342C2");
             conn.AddDigitalOrderField("vpc_MerchTxnRef", "135");
             conn.AddDigitalOrderField("vpc_OrderInfo", CT.MaDonHang.ToString());
             conn.AddDigitalOrderField("vpc_Amount", CT.Soluong.ToString());
             conn.AddDigitalOrderField("vpc_Currency", "VND");
             conn.AddDigitalOrderField("vpc_ReturnURL", "https://localhost:44379/GioHang/DatHang");
             // Thong tin them ve khach hang. De trong neu khong co thong tin
             conn.AddDigitalOrderField("vpc_SHIP_Street01", kh.DiachiKH);
             conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
             conn.AddDigitalOrderField("vpc_SHIP_City", "");
             conn.AddDigitalOrderField("vpc_SHIP_Country", "Vietnam");
             conn.AddDigitalOrderField("vpc_Customer_Phone", kh.DienthoaiKH);
             conn.AddDigitalOrderField("vpc_Customer_Email", kh.Email);
             conn.AddDigitalOrderField("vpc_Customer_Id", "onepay_paygate");
             // Dia chi IP cua khach hang
             conn.AddDigitalOrderField("vpc_TicketNo", "");
             // Chuyen huong trinh duyet sang cong thanh toan
             String url = conn.Create3PartyQueryString();
             return Redirect(url);


         }*/
        public ActionResult XacNhanDonHang()
        {

            return View();



        }
    }
    
}