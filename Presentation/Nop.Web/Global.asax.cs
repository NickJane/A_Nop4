using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.ViewEngines;
using System.Data.Entity;

namespace Nop.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //ef和数据库的映射有很多种, 一：数据库不存在时重新创建数据库 二：每次启动应用程序时创建数据库 三：模型更改时重新创建数据库
            //这里我们选择用户自己创建数据库  
            Database.SetInitializer<Nop.Data.NopObjectContext>(null);

            //初始化Autofac的依赖注入管理
            EngineContext.Initialize(false);

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //remove all view engines
            ViewEngines.Engines.Clear(); 
            ViewEngines.Engines.Add(new NopViewEngine());
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //log error
            //LogException(exception);

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                //if (!webHelper.IsStaticResource(Request))//确定不是静态资源请求
                //{
                Response.Clear();
                Server.ClearError();//404引发的错误, 清除服务器端错误信息, 这样windows日志探查器不会有日志
                Response.TrySkipIisCustomErrors = true;

                Response.Clear();
                Server.ClearError();
                Response.TrySkipIisCustomErrors = true;
                Response.WriteFile(CommonHelper.MapPath("~/htmlerror.html"));
                Response.End();
                //}
            }
            else
            {
                //var storeNotFindException = exception as StoreNotFindException;
                //if (storeNotFindException != null)
                //{
                //    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                //    if (!webHelper.IsStaticResource(Request))
                //    {
                //        //如果需要输出特殊的html页面作为响应页面
                //Response.Clear();
                //Server.ClearError();
                //Response.TrySkipIisCustomErrors = true;
                //Response.WriteFile(CommonHelper.MapPath("~/htmlerror.html"));
                //Response.End();

                //    }
                //}
            }
        }
    }
}
