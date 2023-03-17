using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Net.Mail;
using WebBanRau.Models;
using PagedList;
using System.Data;
using System.Data.OleDb;
using LinqToExcel;
using System.Data.Entity.Validation;

namespace WebBanRau.Controllers
{
    public class ChucNangController : Controller
    {
        QLRauCuDataContext context = new QLRauCuDataContext();

        //------------------------------ Sản Phẩm ---------------------------------------
        public ActionResult DSsanpham(int? page)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            int pagesize = 10;
            int pageNum = (page ?? 1);
            var list = context.SANPHAMs.OrderByDescending(s => s.MaSP).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        public ActionResult loai(int id)
        {
            var list = context.LOAISANPHAMs.Where(n => n.MaloaiSP == id);
            return View(list.Single());
        }
        public ActionResult NCC(int id)
        {
            var list = context.NCCs.Where(n => n.MaNCC == id);
            return View(list.Single());
        }


        [HttpGet]
        //[Authorize(System.Security.Policy="")]
        public ActionResult Themsanpham()
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            ViewBag.MaNCC = new SelectList(context.NCCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.Loai = new SelectList(context.LOAISANPHAMs.ToList().OrderBy(n => n.MaloaiSP), "MaloaiSP", "Tensanpham");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themsanpham(SANPHAM sp, FormCollection collection, HttpPostedFileBase fileUpload)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            ViewBag.MaNCC = new SelectList(context.NCCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.Loai = new SelectList(context.LOAISANPHAMs.ToList().OrderBy(n => n.MaloaiSP), "MaloaiSP", "Tensanpham");
            var TenSp = collection["Ten"];
            var gia = collection["Gia"];
            var Mota = collection["textarea"];
            var Anhbia = collection["Anhbia"];
            var Date = collection["Date"];
            var SoLuong = collection["SoLuong"];
            var DVT = collection["DVT"];
            var loai = collection["Loai"];
            var ncc = collection["MaNCC"];
           
            


            sp.TenSP = TenSp;
            sp.Giaban = decimal.Parse(gia);
            sp.Mota = Mota;
            sp.Anhbia = Anhbia;
            sp.Ngaycapnhat = DateTime.Parse(Date);
            sp.Soluongton = Int32.Parse(SoLuong);
            sp.DVT = DVT;
            sp.MaloaiSP = Int32.Parse(loai);
            sp.MaNCC = Int32.Parse(ncc);      
            context.SANPHAMs.InsertOnSubmit(sp);
            context.SubmitChanges();
            return RedirectToAction("DSsanpham", "ChucNang");
        }

        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                SANPHAM sp = context.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
                ViewBag.MaSP = sp.MaSP;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sp);
            }
        }
        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult XacNhanXoasanpham(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                SANPHAM sp = context.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
                ViewBag.MaSP = sp.MaSP;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                context.SANPHAMs.DeleteOnSubmit(sp);
                context.SubmitChanges();
                return RedirectToAction("DSsanpham");
            }
        }


        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            SANPHAM sp = context.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaNCC = new SelectList(context.NCCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaloaiSP = new SelectList(context.LOAISANPHAMs.ToList().OrderBy(n => n.MaloaiSP), "MaloaiSP", "Tensanpham", sp.MaloaiSP);

            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpPost, ActionName("Suasanpham")]
        public ActionResult XacNhanSuasanpham(FormCollection collection, int id, HttpPostedFileBase fileUpload)
        {
            var img = "";
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            if (fileUpload != null)
            {
                img = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Image"), img);
                if (!System.IO.File.Exists(path))//Sản Phẩm Chưa Tồn Tại
                {
                    fileUpload.SaveAs(path);
                }
            }
            else
            {
                img = collection["Anh"];
            }
            SANPHAM sp = context.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            sp.Anhbia = img;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(sp);
            context.SubmitChanges();
            return RedirectToAction("DSsanpham");

        }


        public ActionResult Details(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }


            SANPHAM ncc = context.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }

        //----------------------------------- Loại Sản Phẩm ------------------------------------
        public ActionResult DSLSP(int? page)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            int pagesize = 10;
            int pageNum = (page ?? 1);
            var list = context.LOAISANPHAMs.OrderBy(s => s.MaloaiSP).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        [HttpGet]
        public ActionResult ThemLSP()
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult ThemLSP(LOAISANPHAM lsp, FormCollection collection)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                var Tensanpham = collection["TenLoai"];
                lsp.Tensanpham = Tensanpham;
                context.LOAISANPHAMs.InsertOnSubmit(lsp);                            
                context.SubmitChanges();
                return RedirectToAction("DSLSP", "ChucNang");
            }
        }

        public ActionResult SuaLSP(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                LOAISANPHAM lsp = context.LOAISANPHAMs.SingleOrDefault(n => n.MaloaiSP == id);
                if (lsp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(lsp);
            }
        }

        [HttpPost, ActionName("SuaLSP")]
        public ActionResult XacNhanSuaLSP(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                LOAISANPHAM ncc = context.LOAISANPHAMs.SingleOrDefault(n => n.MaloaiSP == id);
                ViewBag.MaNCC = ncc.MaloaiSP;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                UpdateModel(ncc);
                context.SubmitChanges();
                return RedirectToAction("DSLSP");
            }
        }

        [HttpGet]
        public ActionResult XoaLSP(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                LOAISANPHAM ncc = context.LOAISANPHAMs.SingleOrDefault(n => n.MaloaiSP == id);
                ViewBag.MaloaiSP = ncc.MaloaiSP;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ncc);
            }
        }
        [HttpPost, ActionName("XoaLSP")]
        public ActionResult XacNhanXoaLSP(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                LOAISANPHAM ncc = context.LOAISANPHAMs.SingleOrDefault(n => n.MaloaiSP == id);
                ViewBag.MaloaiSP = ncc.MaloaiSP;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                context.LOAISANPHAMs.DeleteOnSubmit(ncc);
                context.SubmitChanges();
                return RedirectToAction("DSLSP");
            }
        }



        //-----------------------------------Nhà Cung Cấp---------------------------------------
        public ActionResult DSNCC(int? page)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                int pagesize = 10;
                int pageNum = (page ?? 1);
                var list = context.NCCs.OrderByDescending(s => s.MaNCC).ToList();
                return View(list.ToPagedList(pageNum, pagesize));
            }
        }
        [HttpGet]
        public ActionResult ThemNCC()
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult ThemNCC(NCC ncc, FormCollection collection)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                var tenNCC = collection["tenNCC"];
                var Diachi = collection["Diachi"];
                var Dienthoai = collection["Dienthoai"];
                ncc.TenNCC = tenNCC;
                ncc.Diachi = Diachi;
                ncc.Dienthoai = Dienthoai;
                context.NCCs.InsertOnSubmit(ncc);
                context.SubmitChanges();
                return RedirectToAction("DSNCC", "ChucNang");
            }
        }

        [HttpGet]
        public ActionResult XoaNCC(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                NCC ncc = context.NCCs.SingleOrDefault(n => n.MaNCC == id);
                ViewBag.MaNCC = ncc.MaNCC;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ncc);
            }
        }
        [HttpPost, ActionName("XoaNCC")]
        public ActionResult XacNhanXoa(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                NCC ncc = context.NCCs.SingleOrDefault(n => n.MaNCC == id);
                ViewBag.MaNCC = ncc.MaNCC;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                context.NCCs.DeleteOnSubmit(ncc);
                context.SubmitChanges();
                return RedirectToAction("DSNCC");
            }
        }

        [HttpGet]
        public ActionResult SuaNCC(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                NCC ncc = context.NCCs.SingleOrDefault(n => n.MaNCC == id);
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ncc);
            }
        }

        [HttpPost, ActionName("SuaNCC")]
        public ActionResult XacNhanSua(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                NCC ncc = context.NCCs.SingleOrDefault(n => n.MaNCC == id);
                ViewBag.MaNCC = ncc.MaNCC;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                UpdateModel(ncc);
                context.SubmitChanges();
                return RedirectToAction("DSNCC");
            }
        }
        //------------------------------ Góp Ý ------------------------------------
        public ActionResult DSGOPY(int? page)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                int pagesize = 10;
                int pageNum = (page ?? 1);
                var list = context.HOTROs.OrderByDescending(s => s.MaHotro).ToList();
                return View(list.ToPagedList(pageNum, pagesize));
            }
        }

        public ActionResult GOPY_CHITIET(int id)
        {
            HOTRO ht = context.HOTROs.SingleOrDefault(n => n.MaHotro == id);
            ViewBag.MaHotro = ht.MaHotro;
            if (ht == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ht);
        }

        public ActionResult XoaGopy(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                HOTRO ncc = context.HOTROs.SingleOrDefault(n => n.MaHotro == id);
                ViewBag.MaHotro = ncc.MaHotro;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ncc);
            }
        }
        [HttpPost, ActionName("XoaGopy")]
        public ActionResult XacNhanXoaGopY(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                HOTRO ncc = context.HOTROs.SingleOrDefault(n => n.MaHotro == id);
                ViewBag.MaHotro = ncc.MaHotro;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                context.HOTROs.DeleteOnSubmit(ncc);
                context.SubmitChanges();
                return RedirectToAction("DSGOPY");
            }
        }


        //--------------------------- Khách Hàng ----------------------------------
        public ActionResult DSKH(int? page)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                int pagesize = 10;
                int pageNum = (page ?? 1);
                var list = context.KHACHHANGs.OrderByDescending(s => s.MaKH).ToList();
                return View(list.ToPagedList(pageNum, pagesize));
            }
        }

        [HttpGet]
        public ActionResult DonDatHang(int? page)
        {
            int pagesize = 5;
            int pageNum = (page ?? 1);
            var GioHienTai = DateTime.Today;
            var list = context.DONDATHANGs.Where(s => s.Ngaydat >= GioHienTai).OrderByDescending(i => i.Ngaydat).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        [HttpPost]
        public ActionResult DonDatHang(string date, string date2, int? page)
        {
            int pagesize = 5;
            int pageNum = (page ?? 1);
            var Date = DateTime.Parse(date);

            if (date2 == "")
            {
                var listdate = context.DONDATHANGs.Where(s => s.Ngaydat >= Date).OrderByDescending(i => i.Ngaydat).ToList();
                return View(listdate.ToPagedList(pageNum, pagesize));
            }
            var Date2 = DateTime.Parse(date2);
            var list = context.DONDATHANGs.Where(s => s.Ngaydat >= Date && s.Ngaydat <= Date2).OrderByDescending(i => i.Ngaydat).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        public ActionResult ChiTietDonDatHang(int? id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            if (id == null)
            {
                return HttpNotFound();
            }
            var list = context.CHITIETDONHANGs.Where(s => s.MaDonHang == id).OrderByDescending(s => s.MaSP).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult SuaDonDatHang(int id)
        {
            //if (Session["TKAdmin"] == null)
            //{
            //    return RedirectToAction("Index", "HienThiSanPham");
            //}

            DONDATHANG ncc = context.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }

        [HttpPost, ActionName("SuaDonDatHang")]
        public ActionResult XacNhanSuaDonDatHang(int id)
        {
            if (Session["TKAdmin"] == null)
            {
                return RedirectToAction("Index", "HienThiSanPham");
            }
            else
            {
                DONDATHANG ncc = context.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                UpdateModel(ncc);
                context.SubmitChanges();
                return RedirectToAction("DonDatHang");
            }
        }
        //----------------------Thông kê-----------------------------------------------------------------------
        public ActionResult ThongKe()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Lay so luong nguoi truy cap application da duoc tao
            ViewBag.SoNguoiDangonl = HttpContext.Application["SoNguoiDangonl"].ToString();//Lay so luong nguoi truy cap application da duoc tao
            ViewBag.TongdanhThu = ThongKeTongDanhThu();//Lay so luong nguoi truy cap application da duoc tao
            ViewBag.TongDDH = ttongdonhang();//Lay so luong nguoi truy cap application da duoc tao
            ViewBag.TongThanhVien = thongkethanhvien();//Lay so luong nguoi truy cap application da duoc tao
            //ViewBag.TKtheothang = ThongKeTongDanhThutheothang(int Thang,int Nam);//Lay so luong nguoi truy cap application da duoc tao


            return View();
        }
        public decimal ThongKeTongDanhThu()
        {          
            decimal TongdanhThu = context.CHITIETDONHANGs.Sum(n => n.Soluong * n.Dongia).Value;
                return TongdanhThu;
                             
        }
        public double ttongdonhang()
        {
            double slDH = context.DONDATHANGs.Count();
            return slDH;
        }
        public double thongkethanhvien()
        {
            double slTV = context.KHACHHANGs.Count();
            return slTV;
        }
        public decimal ThongKeTongDanhThutheothang(int Thang, int Nam)
        {
            var LSDT = context.DONDATHANGs.Where(n => n.Ngaydat.Value.Month == Thang && n.Ngaydat.Value.Year == Nam);
            decimal Tongtien = 0;
            foreach (var item in LSDT)
            {
                Tongtien += decimal.Parse(item.CHITIETDONHANGs.Sum(n => n.Soluong * n.Dongia).Value.ToString());

            }

            return Tongtien;
        }
        [HttpGet]
        public ActionResult ThemSanPhamExcel()
        {
            if (Session["TKAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ViewBag.Ma_NCC = new SelectList(context.NCCs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
                ViewBag.Ma_LSP = new SelectList(context.LOAISANPHAMs.ToList().OrderBy(n => n.Tensanpham), "MaloaiSP", "Tensanpham");
                return View();
            }

        }
        [HttpPost]
        public ActionResult ThemSanPhamExcel(FormCollection collection, HttpPostedFileBase FileUpload)
        {
            string filename = "";
            string targetpath = Server.MapPath("~/Assets/doc/");
            var path = Path.Combine(targetpath, FileUpload.FileName);
            if (System.IO.File.Exists(path))
            {
                string extensionName = Path.GetExtension(FileUpload.FileName);
                filename = FileUpload.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                path = Path.Combine(targetpath, filename);
                FileUpload.SaveAs(path);
            }
            else
            {
                filename = FileUpload.FileName;
                FileUpload.SaveAs(targetpath + filename);
            }

            string pathToExcelFile = targetpath + filename;
            var connectionString = "";
            if (filename.EndsWith(".xls"))
            {
                connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
            }
            else if (filename.EndsWith(".xlsx"))
            {
                connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
            }

            var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
            var ds = new DataSet();
            adapter.Fill(ds, "ExcelTable");
            DataTable dtable = ds.Tables["ExcelTable"];
            string sheetName = "Sheet1";
            var excelFile = new ExcelQueryFactory(pathToExcelFile);
            var artistAlbums = from a in excelFile.Worksheet<SANPHAM>(sheetName) select a;
            foreach (var a in artistAlbums)
            {
                try
                {
                    if (a.MaSP != -1 && Check_MaSP(a.MaSP) == -1 && a.TenSP != "" && a.Anhbia != "" && a.Giaban != 0 && a.Soluongton != 0)
                    {
                        SANPHAM sp = new SANPHAM();

                        var nhacungcap = int.Parse(collection["Ma_NCC"]);
                        var loaisanpham = int.Parse(collection["Ma_LSP"]);
                        sp.MaSP = a.MaSP;
                        sp.TenSP = a.TenSP;
                        sp.Giaban = a.Giaban;
                        sp.Mota = a.Mota;
                        sp.Anhbia = a.Anhbia;
                        sp.Ngaycapnhat = DateTime.Now;
                        sp.Soluongton = a.Soluongton;
                        sp.DVT = a.DVT;
                        sp.MaloaiSP = loaisanpham;
                        sp.MaNCC = nhacungcap;
                        context.SANPHAMs.InsertOnSubmit(sp);
                        context.SubmitChanges();
                    }
                    else
                    {

                        string message = "";
                        TempData["message"] = "Nhập danh sách trái cây KHÔNG thành công. Vui lòng kiểm tra lại";
                        TempData["alert"] = "alert-danger";
                        return RedirectToAction("DSsanpham", "ChucNang");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }
            return RedirectToAction("DSsanpham", "ChucNang");

        }
        public int Check_MaSP(int MaSP)
        {
            if (context.SANPHAMs.Count(x => x.MaSP == MaSP) > 0)
            {
                return context.SANPHAMs.FirstOrDefault(x => x.MaSP == MaSP).MaSP;
            }
            else
            {
                return -1;
            }
        }

    }
}
    
