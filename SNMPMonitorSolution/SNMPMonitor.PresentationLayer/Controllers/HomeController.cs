using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNMPMonitor.PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Dashboard()
        {
            return PartialView();
        }

        public PartialViewResult Monitor(int id = 0)
        {
            return PartialView(id);
        }

        public PartialViewResult Logs()
        {
            return PartialView();
        }

        public PartialViewResult AddAgent()
        {
            return PartialView();
        }

        public PartialViewResult ModifyAgent(int id = 0)
        {
            return PartialView(id);
        }

        [HttpGet]
        public PartialViewResult SideBarContent()
        {
            return PartialView();
        }
    }
}