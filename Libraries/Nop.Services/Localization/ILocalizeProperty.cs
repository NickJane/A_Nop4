using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Localization
{
    /// <summary>
    /// 定义动态数据国际化的每张表的公共属性
    /// </summary>
    public interface ILocalizeProperty
    {
        string LanguageName { get; set; }

        int BusinessId { get; set; }

        int LanguageId { get; set; }

        int DisplayOrder { get; set; }
    }
}
