using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Web.Framework.ViewEngines
{
    /*
     area新增二个步骤
     1: 在这里添加寻路路径
     2: NopVirtualPathProviderViewEngine 的findView增加area判断.
         */


    public class NopViewEngine : NopVirtualPathProviderViewEngine
    {
        public NopViewEngine()
        {

            AreaViewLocationFormats = new[]
                                          {
                                              //default
                                              "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                              "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                          };

            AreaMasterLocationFormats = new[]
                                            {
                                                //default
                                                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                            };

            AreaPartialViewLocationFormats = new[]
                                                 {
                                                    //default
                                                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
                                                 };

            ViewLocationFormats = new[]
                                      {
                                            //default
                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/Shared/{0}.cshtml",

                                             //Designer
                                            "~/Designer/Views/{1}/{0}.cshtml",
                                            "~/Designer/Views/Shared/{0}.cshtml",

                                            //Admin
                                            "~/Admin/Views/{1}/{0}.cshtml",
                                            "~/Admin/Views/Shared/{0}.cshtml",

                                            //WebAPI
                                            "~/WebAPI/Views/{1}/{0}.cshtml",
                                            "~/WebAPI/Views/Shared/{0}.cshtml",

                                            //Test
                                            "~/Test/Views/{1}/{0}.cshtml",
                                            "~/Test/Views/Shared/{0}.cshtml",
                                      };

            MasterLocationFormats = new[]
                                        {
                                            //default
                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/Shared/{0}.cshtml",

                                             //Designer
                                            "~/Designer/Views/{1}/{0}.cshtml",
                                            "~/Designer/Views/Shared/{0}.cshtml",

                                            //Admin
                                            "~/Admin/Views/{1}/{0}.cshtml",
                                            "~/Admin/Views/Shared/{0}.cshtml",

                                             //WebAPI
                                            "~/WebAPI/Views/{1}/{0}.cshtml",
                                            "~/WebAPI/Views/Shared/{0}.cshtml",

                                            //Test
                                            "~/Test/Views/{1}/{0}.cshtml",
                                            "~/Test/Views/Shared/{0}.cshtml",
                                        };

            PartialViewLocationFormats = new[]
                                             {
                                                //default
                                                "~/Views/{1}/{0}.cshtml",
                                                "~/Views/Shared/{0}.cshtml", 

                                                   //Designer
                                            "~/Designer/Views/{1}/{0}.cshtml",
                                            "~/Designer/Views/Shared/{0}.cshtml",

                                                //Admin
                                                "~/Admin/Views/{1}/{0}.cshtml",
                                                "~/Admin/Views/Shared/{0}.cshtml",

                                                 //WebAPI
                                            "~/WebAPI/Views/{1}/{0}.cshtml",
                                            "~/WebAPI/Views/Shared/{0}.cshtml",

                                            //Test
                                            "~/Test/Views/{1}/{0}.cshtml",
                                            "~/Test/Views/Shared/{0}.cshtml",
                                             };

            FileExtensions = new[] { "cshtml" };
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }


        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, partialPath, null, false, fileExtensions);
            //return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions, base.ViewPageActivator);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, viewPath, masterPath, true, fileExtensions);
        }

    }
}
