using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Domain.Localization
{
    public class Language :BaseEntity<int>
    {
        public string LanguageName { get; set; }

        public string LanguageCultrue { get; set; }

        public bool Published { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
    }
}
