using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Tuan4_NguyenHuuQuy.Models;

namespace Tuan4_NguyenHuuQuy.Controllers
{
    public class HomeController : Controller
    {
        quyDataContext data = new quyDataContext();
        public ActionResult Index(int ? Page)
        {
            if (Page == null) Page = 1;
            var all_sach = from s in data.Saches select s;
            int pagesize = 2;
            int pageNum = Page ?? 1;
            return View(all_sach);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
     
    
    }
}