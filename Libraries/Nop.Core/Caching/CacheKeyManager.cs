using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Caching
{
    public class ScdCacheCategory
    {
        public static string Site { get { return "site."; } }
        public static string Language { get { return "lan."; } }
        public static string News { get { return "news."; } }
        public static string Products { get { return "prds."; } }
        public static string EBusiness { get { return "eBusiness."; } }
        public static string Picture { get { return "pic."; } }
        public static string FileUpload { get { return "file."; } }
        public static string Publish { get { return "pub."; } }
        public static string Domain { get { return "domain."; } }
        public static string Auth { get { return "auth."; } }
        public static string FlipCloud { get { return "flipCloud."; } }
        public static string Customer { get { return "customer."; } }
        /// <summary>
        /// 控件相关缓存
        /// </summary>
        public static string Control { get { return "control."; } }

        /// <summary>
        /// webapi相关的缓存
        /// </summary>
        public static string WebApi { get { return "webapi."; } }

        /// <summary>
        /// 配置文件相关的缓存
        /// </summary>
        public static string ConfigFile { get { return "configFile."; } }

        public static string VM { get { return "vm."; } }

        public static string WeiXin { get { return "weixin."; } }

        public static string SmallProgram { get { return "smallprogram."; } }
    }

    public class ScdCacheFormat
    {
        public static string Test { get { return ScdCacheCategory.Site + "test-{0}-{1}"; } }

        #region Site
        /// <summary>
        /// 站点详情缓存{0}代表siteId
        /// </summary>
        public static string SiteDetail { get { return ScdCacheCategory.Site + "SiteDetail-{0}"; } }

        /// <summary>
        /// 留言提醒邮件验证码
        /// </summary>
        public static string RemindLeavewordEmailCaptcha
        {
            get
            {
                return ScdCacheCategory.Site + "RemindLeavewordEmailCaptcha_{0}";
            }
        }

        /// <summary>
        /// 站点的缓存Key
        /// </summary>
        public static string SiteInfo
        {
            get
            {
                return ScdCacheCategory.Site + "SiteInfo-{0}";
            }
        }
        /// <summary>
        /// 404站点缓存
        /// </summary>
        public static string Site404
        {
            get { return ScdCacheCategory.Site + "404-{0}"; }
        }
        /// <summary>
        /// 站点域名缓存,{0}代表域名
        /// </summary>
        public static string SiteDomain { get { return ScdCacheCategory.Site + "SiteDomain-{0}"; } }

        /// <summary>
        /// 站点二级域名缓存,{0}代表SiteId
        /// </summary>
        public static string SecondSiteDomain { get { return ScdCacheCategory.Site + "SecondSiteDomain-{0}"; } }


        /// <summary>
        /// 站点还原,参数{0}代表SiteId
        /// </summary>
        public static string SiteBackup { get { return ScdCacheCategory.Site + "SiteBackup-{0}"; } }

        /// <summary>
        /// 站点备份 ,参数{0}代表SiteId
        /// </summary>
        public static string SiteRestore { get { return ScdCacheCategory.Site + "SiteRestore-{0}"; } }

        /// <summary>
        /// 小程序站点备份,参数{0}代表SiteId
        /// </summary>
        public static string SpSiteBackup { get { return ScdCacheCategory.Site + "SpSiteBackup-{0}"; } }

        /// <summary>
        /// 小程序站点还原 ,参数{0}代表SiteId
        /// </summary>
        public static string SpSiteRestore { get { return ScdCacheCategory.Site + "SpSiteRestore-{0}"; } }

        public static string Captcha
        {
            get
            {
                return "Captcha_{0}_{1}";
            }
        }

        /// <summary>
        /// "Captcha_{random}_{siteid}", "Captcha_{email}_{siteid}"
        /// </summary>
        public static string CaptchaFindMyPassword
        {
            get
            {
                return "Captcha_{0}_{1}";
            }
        }
        /// <summary>
        /// "Captcha_{email}_{siteid}_code"
        /// </summary>
        public static string CaptchaFindMyPasswordEmailCode
        {
            get
            {
                return "Captcha_{0}_{1}_{2}";
            }
        }

        public static string SiteAdminLogoFileName
        {
            get
            {
                return "SiteAdminLogoFileName_{0}";
            }
        }

        public static string SiteFaviconIcoFileName
        {
            get
            {
                return "SiteFaviconIcoFileName_{0}";
            }
        }
        #endregion

        #region Language

        /// <summary>
        /// 平台所有语言缓存
        /// </summary>
        public static string AllLanguage
        {
            get
            {
                return ScdCacheCategory.Language + "AllLanguage";
            }
        }

        public static string SiteLanguagePrimary
        {
            get
            {
                return ScdCacheCategory.Site + "SitePrimaryLanguage-{0}";
            }
        }
        public static string SiteLanguageList
        {
            get
            {
                return ScdCacheCategory.Site + "SiteLanguageList-{0}";
            }
        }
        #endregion

        #region News
        public static string AllNewsCategories
        {
            get
            {
                return ScdCacheCategory.Site + "AllNewsCategories-{0}-{1}";
            }
        }

        public static string SiteNewsItemCount
        {
            get
            {
                return ScdCacheCategory.Site + "SiteNewsItemCount-{0}";
            }
        }
        public static string SiteNewsCategoryCount
        {
            get
            {
                return ScdCacheCategory.Site + "SiteNewsCategoryCount-{0}";
            }
        }
        #endregion

        #region Products
        public static string UnDeletedProductsCount
        {
            get
            {
                return ScdCacheCategory.Products + "_SiteId_{0}";
            }
        }
        public static string SiteProductCategoryCount
        {
            get
            {
                return ScdCacheCategory.Site + "SiteProductCategoryCount-{0}";
            }
        }
        #endregion


    }

    public class ScdCacheKey
    {
        /// <summary>
        /// ScdCacheFormat
        /// </summary>
        private string _keyFormat;
        private string[] _args;

        /// <summary>
        /// ScdCachekey
        /// </summary>
        /// <param name="keyFormat">keyFormart 如"site.user_{0}_{1}"</param>
        /// <param name="args">keyFormat的参数，可选</param>
        public ScdCacheKey(string keyFormat, params string[] args)
        {
            this._keyFormat = keyFormat;
            this._args = args;
        }

        /// <summary>
        /// ScdCachekey
        /// </summary>
        /// <param name="keyFormat">keyFormart 如"site.user_{0}_{1}"</param>
        /// <param name="args">keyFormat的参数，可选</param>
        public static ScdCacheKey GetCacheKey(string keyFormat, params string[] args)
        {
            var cachekey = new ScdCacheKey(keyFormat, args);
            return cachekey;
        }

        public override string ToString()
        {
            return string.Format(_keyFormat, _args);
        }
    }
}
