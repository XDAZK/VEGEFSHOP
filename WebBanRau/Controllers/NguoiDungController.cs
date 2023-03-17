using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBanRau.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using Facebook;
using System.Configuration;

namespace WebBanRau.Controllers
{
    public class NguoiDungController : Controller
    {
        QLRauCuDataContext data = new QLRauCuDataContext();
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(FormCollection collection, KHACHHANG kh)
        {
            //
            var hoten = collection["HOTEN"];
            var TK = collection["TaiKhoan"];
            var MK = collection["Pass"];
            var ConfirmMK = collection["ConfirmPass"];

            string Email = collection["Email"];
            string Address = collection["Address"];
            string SDT = collection["SDT"];
            string Date = String.Format("{0:MM/dd/yyyy}", collection["Date"]);

            if (MK.Equals(ConfirmMK))
            {
                if (hoten == null || TK == null || MK == null || Email == null || hoten == null || hoten == null || hoten == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    KHACHHANG h = data.KHACHHANGs.SingleOrDefault(a => a.Taikhoan == TK);
                    if (h != null)
                    {
                        ViewBag.Thongbao = "Tên Tài Khoản đã tồn tại";
                    }
                    else
                    {
                        MD5 md5 = new MD5CryptoServiceProvider();

                        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(ConfirmMK));

                        byte[] bytedata = md5.Hash;

                        StringBuilder builder = new StringBuilder();
                        for (int i = 0; i < bytedata.Length; i++)
                        {

                            builder.Append(bytedata[i].ToString("x2"));
                        }

                        string MaHoa = builder.ToString();
                        kh.HoTen = hoten;
                        kh.Taikhoan = TK;
                        kh.Matkhau = MaHoa;
                        kh.Email = Email;
                        kh.DiachiKH = Address;
                        kh.DienthoaiKH = SDT;
                        kh.Ngaysinh = DateTime.Parse(Date);
                        data.KHACHHANGs.InsertOnSubmit(kh);
                        data.SubmitChanges();
                        return RedirectToAction("Login", "NguoiDung");
                    }
                }
            }
            else
            {
                ViewBag.Thongbao = "Tên Tài Khoản đã tồn tại";
                return this.SignUp();
            }
            return this.SignUp();
        }
        private Uri RedirectUri
        {
            get
            {
                var uriBuider = new UriBuilder(Request.Url);
                uriBuider.Query = null;
                uriBuider.Fragment = null;
                uriBuider.Path = Url.Action("FaceBookCallback");
                return uriBuider.Uri;
            }
        }
        public ActionResult LoginFaceBook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
                
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            string TK = collection["TaiKhoan"];
            string MK = collection["Password"];

            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(MK));

            byte[] bytedata = md5.Hash;

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytedata.Length; i++)
            {

                builder.Append(bytedata[i].ToString("x2"));
            }

            string MaHoa = builder.ToString();

            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(a => a.Taikhoan == TK && a.Matkhau == MaHoa);
            if (kh != null)
            {
                Session["User"] = kh.HoTen;
                Session["Taikhoan"] = kh;
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                ViewBag.Thongbao = "Tên Tài Khoản Hoặc Mật Khẩu Không Đúng";
            }

            return View();
        }

        public ActionResult LogOut()
        {
            Session["User"] = null;
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "HienThiSanPham");
        }

        //Gửi mail
        public static void SendEmail(string address, string subject, string message)
        {
            if (new EmailAddressAttribute().IsValid(address)) // check có đúng mail khách hàng
            {
                string email = "ranklatatca@gmail.com";
                var senderEmail = new MailAddress(email, "The Girl Shop (tin nhắn tự động)");
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
    }
}