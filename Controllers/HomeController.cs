using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NAPASTUDENT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        [Route("TimKiem")]
        public ActionResult TimKiem(string searchTerm, SearchType searchType)
        {
            var searchViewModel = new SearchViewModel
            {
                SearchTerm = searchTerm,
                SearchType = searchType
            };
            return View("TimKiem", searchViewModel);
        }
    }

    public class SearchViewModel
    {
        public string SearchTerm { get; set; }
        public SearchType SearchType { get; set; }
    }

    public enum SearchType
    {
        HoatDong =1,
        BaiViet =2,
        SinhVien =3,
        Lop =4,
        DonVi =5,
    }
}