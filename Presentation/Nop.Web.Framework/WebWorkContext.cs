using System;
using System.Linq;
using System.Web;
using Nop.Services;
using Nop.Services.Customer;
using Nop.Data.Domain.Site;
using Nop.Core.Fakes;

namespace Nop.Web.Framework
{
    public class WebWorkContext : IWorkContext
    {
        private const string CustomerCookieName = "Scd.customer";
        
        private readonly HttpContextBase _httpContext; 
        private DeviceMode _deviceType; 
        private CustomerInfo _cachedCustomer;

        public WebWorkContext(HttpContextBase httpContext 
            )
        {
            _httpContext = httpContext; 
            _deviceType = DeviceMode.PC; 
        }

        #region Utilities GetCustomerCookie  SetCustomerCookie

        protected virtual HttpCookie GetCustomerCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[CustomerCookieName];
        }

        protected virtual void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(CustomerCookieName);
                cookie.HttpOnly = true;
                cookie.Value = customerGuid.ToString();
                if (customerGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(CustomerCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }
        #endregion

        #region 工具方法 IsMobile, SaveLanguageToCookie, GetLanguage
        private string GetCookieName(string cacheKey)
        {
            return "yibu_" + cacheKey;
        }

        private void SaveLanguageToCookie(Language language, string cookieName)
        {
            var cookie = new HttpCookie(cookieName);
            cookie.HttpOnly = true;
            cookie.Path = "/";
            cookie.Value = language.LanguageCulture;
            int cookieExpires = 24 * 365; //TODO make configurable
            cookie.Expires = DateTime.Now.AddHours(cookieExpires);
            if (_httpContext.Request.Cookies[cookieName] != null)
            {
                _httpContext.Request.Cookies[cookieName].Value = cookie.Value;
            }
            if (_httpContext.Response.Cookies[cookieName] != null)
            {
                _httpContext.Response.Cookies[cookieName].Value = cookie.Value;
            }
            else
            {
                //_httpContext.Request.Cookies.Remove("yibu_deviceMode");
                //_httpContext.Response.Cookies.Remove("yibu_deviceMode");
                _httpContext.Response.Cookies.Add(cookie);
            }

        }

        /// <summary>
        /// 获得当前用户访问时候的语言, 
        /// 1 找cookie
        /// 2 找系统设置的主语言
        /// 3 找客户当前浏览器的语言
        /// 4 返回中文
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private Language GetLanguage(string cacheKey)
        {
            if (_httpContext == null || _httpContext.Request == null || _httpContext is FakeHttpContext)
            {
                //return _languageService.GetAll().FirstOrDefault();
            }
            if (_httpContext.Items[cacheKey] != null)
            {
                return (Language)_httpContext.Items[cacheKey];//缓存中没有, 就取cookie, 并写入缓存
            }

            string cookieName = "yibu_" + cacheKey;
            HttpCookie cookie = _httpContext.Request.Cookies[cookieName];
            string languageName;
            //从Cookie中找到了
            if (cookie != null)
            {
                languageName = cookie.Value.ToLower();
                Language language = null;// _languageService.GetAll().Where(item => item.LanguageCulture.ToLower() == languageName).FirstOrDefault();
                _httpContext.Items[cacheKey] = language;
                return language;
            }
            else 
            {
                //取客户设置的主语言
                //SiteLanguage siteLanguage = _siteLanguageService.GetPrimaryLanguage(CurrentStore.SiteId);
                //if (siteLanguage != null)
                //{
                //    Language language = _languageService.FindBy(siteLanguage.LanguageId);
                //    _httpContext.Items[cacheKey] = language;
                //    SaveLanguageToCookie(language, cookieName);
                //    return language;
                //}
                //else
                //{
                //    languageName = "zh-CN";
                //    //解决远程请求UserLanguages为null的问题
                //    if (_httpContext.Request.UserLanguages != null)
                //    {
                //        languageName = _httpContext.Request.UserLanguages.Length > 0 ? _httpContext.Request.UserLanguages[0] : "";
                //    }
                //    languageName = languageName.ToLower();
                //    Language language = _languageService.GetAll().Where(item => item.LanguageCulture.ToLower() == languageName).FirstOrDefault();
                //    if (language == null)
                //    {
                //        language = _languageService.GetAll().First();
                //    }
                //    _httpContext.Items[cacheKey] = language;
                //    SaveLanguageToCookie(language, cookieName);
                //    return language;
                //}
                return null;
            }
        }

        private bool IsMobile()
        {
            if (HttpContext.Current == null)
            {
                return false;
            }
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies["yibu_rt_deviceInfo"];
            if (cookie != null && cookie.Value != "")
            {
                var cookieValue = cookie.Value.ToLower();
                if (cookieValue == "0")
                {
                    return false;
                }
                return true;
            }
            int isMobileDevice = 0;

            if (HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT"] != null && context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().IndexOf("application/vnd.wap.xhtml+xml") >= 0 || !string.IsNullOrEmpty(context.Request.ServerVariables["HTTP_X_WAP_PROFILE"]) || !string.IsNullOrEmpty(context.Request.ServerVariables["HTTP_PROFILE"]))
            {
                isMobileDevice++;
            }

            string mobileUa = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (!string.IsNullOrEmpty(mobileUa))
            {
                mobileUa = mobileUa.Substring(0, 4).ToLower();
            }
            string[] mobileAgents = new string[]{
                "w3c ","acs-","alav","alca","amoi","audi","avan","benq","bird","blac",
                "blaz","brew","cell","cldc","cmd-","dang","doco","eric","hipt","inno",
                "ipaq","java","jigs","kddi","keji","leno","lg-c","lg-d","lg-g","lge-",
                "maui","maxo","midp","mits","mmef","mobi","mot-","moto","mwbp","nec-",
                "newt","noki","oper","palm","pana","pant","phil","play","port","prox",
                "qwap","sage","sams","sany","sch-","sec-","send","seri","sgh-","shar",
                "sie-","siem","smal","smar","sony","sph-","symb","t-mo","teli","tim-",
                "tosh","tsm-","upg1","upsi","vk-v","voda","wap-","wapa","wapi","wapp",
                "wapr","webc","winw","winw","xda ","xda-"};

            if (mobileAgents.Contains(mobileUa))
            {
                isMobileDevice++;
            }

            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null && context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().IndexOf("operamini") >= 0)
            {
                isMobileDevice++;
            }
            // 添加微信移动端浏览器支持
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null && context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().IndexOf("micromessenger") >= 0)
            {
                isMobileDevice++;
            }
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null && context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().IndexOf("windows") >= 0)
            {
                isMobileDevice = 0;
            }
            bool isTablet = false;
            if (bool.TryParse(context.Request.Browser["IsTablet"], out isTablet) && isTablet)
            {
                isMobileDevice = 0;
            }
            if (context.Request.Browser.IsMobileDevice)
            {
                isMobileDevice++;
            }
            if (isMobileDevice > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 当前用户
        /// </summary>
        public virtual CustomerInfo CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null)
                    return _cachedCustomer;

                CustomerInfo customer = null;

                if (customer == null || customer.Deleted || !customer.Active)
                {
                    //从缓存或者数据库中得到用户
                    //customer = _authenticationService.GetAuthenticatedCustomer();
                }

                //validation
                if (null != customer && !customer.Deleted && customer.Active)
                {
                    //写入cookie
                    SetCustomerCookie(customer.CustomerGuid);
                    _cachedCustomer = customer;
                }

                return _cachedCustomer;
            }
            set
            {
                SetCustomerCookie(value.CustomerGuid);
                _cachedCustomer = value;
            }
        }
        

        Language IWorkContext.RunTimeLanguage
        {
            get
            {

            }
            set => throw new NotImplementedException();
        }
        DeviceMode IWorkContext.RunTimeDeviceMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
