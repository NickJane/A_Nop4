
using Nop.Core.Infrastructure;
using Nop.Services;
using Nop.Web.Framework.UI;
using System.Text;

namespace System.Web.Mvc
{
    public static class LayoutExtensions
    { 
        /// <summary>
        /// Generate all CSS parts, 在页面打印css
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public static MvcHtmlString RenderCssFiles(this HtmlHelper html, UrlHelper urlHelper,
            ResourceLocation location)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return MvcHtmlString.Create(pageHeadBuilder.GenerateCssFiles(urlHelper, location));
        }

        /// <summary>
        /// Generate all script parts, 在页面打印js
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public static MvcHtmlString RenderScripts(this HtmlHelper html, UrlHelper urlHelper,
            ResourceLocation location)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return MvcHtmlString.Create(pageHeadBuilder.GenerateScripts(urlHelper, location));
        }



        /// <summary>
        /// js加入生成的队列中
        /// </summary>
        /// <param name="html"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static void AddScript(this HtmlHelper html, string js, ResourceLocation location)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddScriptParts(location,js);
        }

        /// <summary>
        /// css加入生成的队列中
        /// </summary>
        /// <param name="html"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static void AddCss(this HtmlHelper html, string css, ResourceLocation location)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddCssFileParts(location, css);
        }


        /// <summary>
        /// js加入生成的队列中
        /// </summary>
        /// <param name="html"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static void AppendScript(this HtmlHelper html, string js, ResourceLocation location)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendScriptParts(location, js);
        }

        /// <summary>
        /// css加入生成的队列中
        /// </summary>
        /// <param name="html"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static void AppendCss(this HtmlHelper html, string css, ResourceLocation location)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendCssFileParts(location, css);
        }




        public static MvcHtmlString AdminCssLocalizationDir(this HtmlHelper html)
        {
            var _workContext = EngineContext.Current.Resolve<IWorkContext>();
            var adminLanguageCulture = _workContext.RunTimeLanguage.LanguageCultrue.ToUpper();
            var cssLocalizationDir = "Chinese";
            if (!string.IsNullOrEmpty(adminLanguageCulture))
            {
                if (adminLanguageCulture.ToUpper() == "EN-US")
                {
                    cssLocalizationDir = "English";
                }
                if (adminLanguageCulture.ToUpper() == "ZH-CN")
                {
                    cssLocalizationDir = "Chinese";
                }
            }
            return MvcHtmlString.Create(cssLocalizationDir);
        }
        

        public static MvcHtmlString FaviconIconUrl(this HtmlHelper html)
        {
            //var _workContext = EngineContext.Current.Resolve<IWorkContext>();
            //var siteInfoService = EngineContext.Current.Resolve<ISiteInfoService>();
            //try
            //{
            //    var faviconIconUrl = siteInfoService.GetSiteFaviconIcon(_workContext.CurrentStore.SiteId);
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("<link rel=\"icon\" href=\"" + faviconIconUrl + "\"/>");
            //    sb.Append("<link rel=\"shortcut icon\" href=\"" + faviconIconUrl + "\"/>");
            //    sb.Append("<link rel=\"bookmark\" href=\"" + faviconIconUrl + "\"/>");
            //    return MvcHtmlString.Create(sb.ToString());
            //}
            //catch
            //{
            //    return MvcHtmlString.Create("");
            //}
            return MvcHtmlString.Create("");
        }



    }
}
