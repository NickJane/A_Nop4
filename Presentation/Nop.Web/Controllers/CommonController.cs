using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Web.Controllers
{
    public class CommonController : Controller
    {
         

        public ActionResult CustomErrors()
        {
            return View();
        }

        public ActionResult error404()
        {
            return View();
        }
    }
}