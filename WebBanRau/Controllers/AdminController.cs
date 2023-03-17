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
    public class AdminController : Controller
    {
        QLRauCuDataContext context = new QLRauCuDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            string user = collection["form-username"];
            string pass = collection["form-password"];

            ADMIN ad = context.ADMINs.SingleOrDefault(a => a.UserAdmin == user && a.PassAdmin == pass);
            if (ad == null)
            {
                ViewBag.ThongBaoAdmin = "Tài Khoản Hoặc Mật Khẩu Sai";
                return this.Login();
            }
            Session["TKAdmin"] = ad;
            return RedirectToAction("Index", "Admin");
        }
    }
}
