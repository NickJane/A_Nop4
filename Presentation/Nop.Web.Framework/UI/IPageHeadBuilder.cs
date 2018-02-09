using System.Web.Mvc;

namespace Nop.Web.Framework.UI
{
    public partial interface IPageHeadBuilder
    {

        string GenerateScripts(UrlHelper urlHelper, ResourceLocation location);

        string GenerateCssFiles(UrlHelper urlHelper, ResourceLocation location);
        void AddScriptParts(ResourceLocation location, string part);

        void AppendScriptParts(ResourceLocation location, string part);
        void AddCssFileParts(ResourceLocation location, string part);
        void AppendCssFileParts(ResourceLocation location, string part);
    }
}
