using System.IO;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Nop.Services;
using Nop.Web.Framework.Localization;
using Nop.Services.Localization;

namespace Nop.Web.Framework.ViewEngines.Razor
{
    /// <summary>
    /// 可以在cshtml页面中使用自定义的方法和属性.
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        //private ILocalizationService _localizationService;
        private Localizer _localizerAdmin;
        private Localizer _localizerWeb;
        private IWorkContext _workContext;

        public override void InitHelpers()
        {
            base.InitHelpers();

            _workContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// 后台资源文件输出
        /// </summary>
        public Localizer T
        {
            get
            {
                if (_localizerAdmin == null)
                {

                    _localizerAdmin = (format, args) =>
                                     {
                                         string resFormat = null;// _localizationService.GetResource(format);
                                         if (args == null || args.Length == 0)
                                         {
                                             resFormat = LocalizationHelper.GetString(LocalizationDictionaryName.Admin, format,
                                                 "zh-cn");
                                         }
                                         else
                                         {
                                             resFormat = LocalizationHelper.GetString(LocalizationDictionaryName.Admin, format,
                                                 "zh-cn", args);
                                         }
                                         if (string.IsNullOrEmpty(resFormat))
                                         {
                                             return new Web.Framework.Localization.LocalizedString(format);
                                         }
                                         return new Web.Framework.Localization.LocalizedString(resFormat); 
                                     };
                }
                return _localizerAdmin;
            }
        }
        /// <summary>
        /// 前端资源文件输出
        /// </summary>
        public Localizer L
        {
            get
            {
                if (_localizerWeb == null)
                {

                    _localizerWeb = (format, args) =>
                    {
                        string resFormat = null;// _localizationService.GetResource(format);
                        if (args == null || args.Length == 0)
                        {
                            resFormat = LocalizationHelper.GetString(LocalizationDictionaryName.Web, format,
                                "zh-cn");
                        }
                        else
                        {
                            resFormat = LocalizationHelper.GetString(LocalizationDictionaryName.Web, format,
                                "zh-cn", args);
                        }
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new Web.Framework.Localization.LocalizedString(format);
                        }
                        return new Web.Framework.Localization.LocalizedString(resFormat);
                    };
                }
                return _localizerWeb;
            }
        }

        //nopcommerce
        public override string Layout
        {
            get
            {
                var layout = base.Layout;

                if (!string.IsNullOrEmpty(layout))
                {
                    var filename = Path.GetFileNameWithoutExtension(layout);
                    ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ViewContext.Controller.ControllerContext, filename, "");

                    if (viewResult.View != null && viewResult.View is RazorView)
                    {
                        layout = (viewResult.View as RazorView).ViewPath;
                    }
                }

                return layout;
            }
            set
            {
                base.Layout = value;
            }
        }

        public IWorkContext WorkContext
        {
            get
            {
                return _workContext;
            }
        }
    }

    /// <summary>
    /// Web view page
    /// </summary>
    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}