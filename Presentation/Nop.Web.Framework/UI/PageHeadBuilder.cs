
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace Nop.Web.Framework.UI
{
    public partial class PageHeadBuilder : IPageHeadBuilder
    {
        #region Fields

        private static readonly object s_lock = new object();

        private readonly List<string> _titleParts;
        private readonly List<string> _metaDescriptionParts;
        private readonly List<string> _metaKeywordParts;
        private readonly Dictionary<ResourceLocation, List<string>> _scriptParts;
        private readonly Dictionary<ResourceLocation, List<string>> _cssParts;
        private readonly List<string> _canonicalUrlParts;
        private readonly List<string> _headCustomParts;


        private readonly string _rootPath;
        #endregion

        #region Ctor

        public PageHeadBuilder()
        {
            this._titleParts = new List<string>();
            this._metaDescriptionParts = new List<string>();
            this._metaKeywordParts = new List<string>();
            this._scriptParts = new Dictionary<ResourceLocation, List<string>>();
            this._cssParts = new Dictionary<ResourceLocation, List<string>>();
            this._canonicalUrlParts = new List<string>();
            this._headCustomParts = new List<string>();

            this._rootPath = "";
            var switchOssPath = EngineContext.Current.Resolve<GlobalSettings>().SwtichResourceOSSPath;
            if (switchOssPath=="on")
            { 
                //这里写资源文件在的cdn域名即可
                this._rootPath = "http://baidu.com";
                this._rootPath = "";
            }
        }

        #endregion

        #region Methods 

        public virtual string GenerateCssFiles(UrlHelper urlHelper, ResourceLocation location)
        {
            if (!_cssParts.ContainsKey(location) || _cssParts[location] == null)
                return "";

            //use only distinct rows
            var distinctParts = _cssParts[location].Distinct().ToList();
            if (distinctParts.Count == 0)
                return "";

            var httpContent = EngineContext.Current.Resolve<HttpContextBase>();

            //bundling is disabled
            var result = new StringBuilder();
            for (int i = 0; i < distinctParts.Count(); i++)
            {
                var version = ResourceVersion(httpContent.Server, distinctParts[i]);
                distinctParts[i] = distinctParts[i].TrimStart('/');


                result.AppendFormat("<link href=\"{0}/{1}?version={2}\" rel=\"stylesheet\" type=\"text/css\" />", _rootPath, urlHelper.Content(distinctParts[i]), version);
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }
        private string ResourceVersion(HttpServerUtilityBase Server ,string filepath) {
            if (filepath.IndexOf('?') == -1)
            {
                var path = Server.MapPath(filepath).ToUpper();
                ResourceVersionManager.AddResource(path);
                string temp = "";

                return ResourceVersionManager.ResourceDic.TryGetValue(path, out temp) ? temp : "";
            }
            return "";
        }

        public virtual string GenerateScripts(UrlHelper urlHelper, ResourceLocation location)
        {
            if (!_scriptParts.ContainsKey(location) || _scriptParts[location] == null)
                return "";

            if (_scriptParts.Count == 0)
                return "";

            //bundling is disabled
            var result = new StringBuilder();
            var paths = _scriptParts[location].Distinct().ToList();
            var httpContent = EngineContext.Current.Resolve<HttpContextBase>();
            for (int i = 0; i < paths.Count(); i++)
            {
                var version = ResourceVersion(httpContent.Server, paths[i]);
                paths[i] = paths[i].TrimStart('/');
                result.AppendFormat("<script src=\"{0}/{1}?version={2}\" type=\"text/javascript\"></script>", _rootPath, urlHelper.Content(paths[i]), version);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }
         

        public string GeneratePath(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) { return ""; }
            url = url.TrimStart('/');

            return string.Format("{0}/{1}", _rootPath, url);
        }


        public virtual void AddScriptParts(ResourceLocation location, string part)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(part))
                return;

            _scriptParts[location].Add(part);
        }
        public virtual void AppendScriptParts(ResourceLocation location, string part)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(part))
                return;

            _scriptParts[location].Insert(0, part);
        }

        public virtual void AddCssFileParts(ResourceLocation location, string part)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(part))
                return;

            _cssParts[location].Add(part);
        }
        public virtual void AppendCssFileParts(ResourceLocation location, string part)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(part))
                return;

            _cssParts[location].Insert(0, part);
        }
        #endregion


    }
}
