using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tuan4_NguyenHuuQuy.Models
{
    public class Giohang
    {
        quyDataContext data = new quyDataContext();
        public int masach { get; set; }

        public String tensach { get; set; }

        public String hinh { get; set; }

        public double giaban { get; set; }

        public int iSoLuong { get; set; }

        public double dThanhTien
        {
            get { return iSoLuong * giaban ; }
        }

        public Giohang(int id)
        {
            masach = id;
            Sach sach = data.Saches.Single(n => n.masach == masach);
            tensach = sach.tensach;
            hinh = sach.hinh ;
            giaban = double.Parse(sach.giaban.ToString());
            iSoLuong = 1;
        }

    }
}