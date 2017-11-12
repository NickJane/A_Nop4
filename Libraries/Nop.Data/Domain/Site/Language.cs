using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Domain.Site
{
    public class Language:BaseEntity<int>
    {
        public string LanguageCulture { get; set; }
    }
}
