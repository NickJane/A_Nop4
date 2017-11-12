using System.IO;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Nop.Services;
using Nop.Web.Framework.Localization;

namespace Nop.Web.Framework.ViewEngines.Razor
{
    /// <summary>
    /// Web view page
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        //private ILocalizationService _localizationService;
        private Localizer _localizer;
        private IWorkContext _workContext;

        public override void InitHelpers()
        {
            base.InitHelpers();

            _workContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        /// <summary>
        /// Get a localized resources
        /// </summary>
        public Localizer T
        {
            get
            {
                if (_localizer == null)
                {
                    
                    _localizer = (format, args) =>
                                     {
                                         string resFormat = null;// _localizationService.GetResource(format);
                                         //if (args == null || args.Length == 0)
                                         //{
                                         //    resFormat = LocalizationHelper.GetString(LocalizationResourceType.Admin.ToString(), format,
                                         //        WorkContext.AdminLanguage.LanguageCulture);
                                         //}
                                         //else
                                         //{
                                         //    resFormat = LocalizationHelper.GetString(LocalizationResourceType.Admin.ToString(), format,
                                         //        WorkContext.AdminLanguage.LanguageCulture, args);
                                         //}
                                         //if (string.IsNullOrEmpty(resFormat))
                                         //{
                                         //    return new Web.Framework.Localization.LocalizedString(format);
                                         //}
                                         //return new Web.Framework.Localization.LocalizedString(resFormat);

                                         return new LocalizedString("这是一个webviewPage的自定义委托函数输出");
                                     };
                }
                return _localizer;
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