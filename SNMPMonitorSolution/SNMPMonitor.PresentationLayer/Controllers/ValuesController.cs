using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNMPMonitor.PresentationLayer.Controllers
{
    public class ValuesController : Controller
    {

        SNMPDataRepo repo = new SNMPDataRepo();
        
        public IEnumerable<SNMPData> Get()
        {
            return repo.GetData();
        }
	}
}