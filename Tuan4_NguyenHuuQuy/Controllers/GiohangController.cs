using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan4_NguyenHuuQuy.Models;
using Tuan4_NguyenHuuQuy.Controllers;

namespace Tuan4_NguyenHuuQuy.Controllers
{
    public class GiohangController : Controller
    {
        // GET: GioHang
        quyDataContext data = new quyDataContext();
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int id , String strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.masach == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            } else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }    
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.iSoLuong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if(lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang !=null )
            {
                tt = lstGiohang.Sum(n => n.dThanhTien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult XoaGioHang(int id)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
            if(sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Giohang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                Sach sach = data.Saches.FirstOrDefault(n => n.masach == id);
                int soluong = int.Parse(collection["txtSoLg"].ToString());
                if (soluong > sach.soluongton)
                {
                    Session["Message"] = "Không đủ ";
                    Session["AlertStatus"] = "danger";
                    return RedirectToAction("GioHang");
                }
                //sanpham.iSoluong = int.Parse(collection["txtSoLg"].ToString());
                sanpham.iSoLuong = soluong; ;
            }
            return RedirectToAction("GioHang");
        }

       /* public ActionResult DatHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            foreach (var item in lstGiohang)
            {
                var sach = data.Saches.FirstOrDefault(m => m.masach == item.masach);
                sach.soluongton -= item.iSoLuong;

            }
            data.SubmitChanges();
            Session["Message"] = "Đặt hàng thành công";
            Session["AlerStatus"] = "success";
            lstGiohang.Clear();
            return RedirectToAction("Giohang");

        }*/

       [HttpGet]
       public ActionResult DatHang()
        {
            if(Session["TaiKhoan"]==null || Session["TaiKhoan"].ToString()=="")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if(Session["Giohang"]==null)
            {
                return RedirectToAction("Inex", "Sach");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();
            List<Giohang> gh = Laygiohang();
            var ngaygiao = string.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = item.masach;
                ctdh.gia = (decimal)item.giaban;
                s = data.Saches.Single(n => n.masach == item.masach);
                s.soluongton -= ctdh.soluong;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);

            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacnhanDonHang", "Giohang");
        }
        public ActionResult XacnhanDonHang()
        {
           
            return View();
        }

    }
}