﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNMPMonitor.PresentationLayer.Controllers
{
    public class MonitorTemplateController : Controller
    {
        public PartialViewResult Monitor_Agent()
        {
            return PartialView();
        }

        public PartialViewResult Monitor_Agent_Small()
        {
            return PartialView();
        }

        public PartialViewResult Monitor_1_3_6_1_2_1_25_3_3()
        {
            return PartialView();
        }

        public PartialViewResult Monitor_1_3_6_1_2_1_25_2_3()
        {
            return PartialView();
        }

        public PartialViewResult Monitor_Harddisk()
        {
            return PartialView();
        }

        public PartialViewResult Monitor_Memory()
        {
            return PartialView();
        }
    }
}