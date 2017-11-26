using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Domain.Site
{
    public class Site:BaseEntity<int>
    {
        public string SiteName { get; set; }
    }

    public class SiteDomain : BaseEntity<int>
    {
        public string Domain { get; set; }

        public int SiteId { get; set; }

        public int DisplayOrder { get; set; }
    }
}
