using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Domain.Localization
{ 
    public class LanguageOfSite : BaseEntity<int>
    {
        public int SiteId { get; set; }
        public int LanguageId { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }

        public string LanguageName { get; set; }

        public string LanguageCulture { get; set; }
    }
}
