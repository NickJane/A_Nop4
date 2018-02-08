using Nop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Services.Statistic;

namespace Nop.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var _workContext = Nop.Core.Infrastructure.EngineContext.Current.Resolve<IWorkContext>();

            ViewBag.TestLocalized = Nop.Services.Localization.LocalizationHelper.GetString(Services.Localization.LocalizationDictionaryName.Admin,
                "CustomerName", _workContext.RunTimeLanguage.LanguageCultrue, new string[] { "佳林" });


            AccessStatistic.IncrementIPAsync(_workContext.CurrentSiteId, Request.UserHostAddress, DateTime.Now);
            AccessStatistic.IncrementPvAsync(_workContext.CurrentSiteId, DateTime.Now);

            ViewBag.PV = string.Join(",", AccessStatistic.GetPv(_workContext.CurrentSiteId, DateTime.Now.AddDays(-1), 
                DateTime.Now).Select(x=>x.Value));
            ViewBag.IP = string.Join(",", AccessStatistic.GetIp(_workContext.CurrentSiteId, DateTime.Now.AddDays(-1),
                DateTime.Now)
            .Select(x => x.Value));
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

        public ActionResult Server500()
        {
            throw new HttpException(500, "服务器内部错误");
            return View();
        }
        public ActionResult Server404()
        {
            throw new HttpException(404, "未找到");
            return View();
        }

    }
}