using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSite.ViewModels.ExamBankModels;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        public static CenterDataRequest _centerdata = new CenterDataRequest()
        {
            _id = "2",
            NameTH = "กรมแรงงาน",
            NameEN = "Department of Skill and Development",
            SiteId = "02",
            Address = "BKK",
        };
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
