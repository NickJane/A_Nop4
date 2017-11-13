using System.IO;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Nop.Services;
using Nop.Web.Framework.Localization;
using Nop.Services.Localization;

namespace Nop.Web.Framework.ViewEngines.Razor
{
    /// <summary>
    /// Web view page
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

        /// <summary>
        /// Return a value indicating whether the working language and theme support RTL (right-to-left)
        /// </summary>
        /// <returns></returns>
        //public bool ShouldUseRtlTheme()
        //{
        //    var workContext = EngineContext.Current.Resolve<IWorkContext>();
        //    var supportRtl = workContext.WorkingLanguage.Rtl;
        //    if (supportRtl)
        //    {
        //        //ensure that the active theme also supports it
        //        var themeProvider = EngineContext.Current.Resolve<IThemeProvider>();
        //        var themeContext = EngineContext.Current.Resolve<IThemeContext>();
        //        supportRtl = themeProvider.GetThemeConfiguration(themeContext.WorkingThemeName).SupportRtl;
        //    }
        //    return supportRtl;
        //}

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