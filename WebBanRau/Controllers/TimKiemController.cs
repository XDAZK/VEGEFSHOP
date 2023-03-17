using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRau.Models;

namespace WebBanRau.Controllers
{

    public class TimKiemController : Controller
    {
        QLRauCuDataContext context = new QLRauCuDataContext();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KQTimkiem(string tukhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 5;
            int pageNum = (page ?? 1);
            var listTimkiem = context.SANPHAMs.Where(n => n.TenSP.Contains(tukhoa));
            ViewBag.Tukhoa = tukhoa;

            return View(listTimkiem.OrderBy(n => n.TenSP).ToPagedList(pageNum, pagesize));
        }
        [HttpPost]
        public ActionResult LayKQTimkiem(string tukhoa)
        {
            

            return RedirectToAction("KQTimkiem", new { @tukhoa = tukhoa });
        }
    }
}