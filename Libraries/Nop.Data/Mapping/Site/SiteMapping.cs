using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema; 
namespace Nop.Data.Mapping.Site
{
    public class SiteMapping : EntityTypeConfiguration<Nop.Data.Domain.Site.Site>
    {
        public SiteMapping()
        {
            ToTable("Site");
            this.HasKey(l => new { l.Id });
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Ignore(m => m.CommentSourceName); 
        }
    }

    public class SiteDomainMapping : EntityTypeConfiguration<Nop.Data.Domain.Site.SiteDomain>
    {
        public SiteDomainMapping()
        {
            ToTable("SiteDomain");
            this.HasKey(l => new { l.Id, l.SiteId });
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Ignore(m => m.CommentSourceName); 
        }
    }
}
