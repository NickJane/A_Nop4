
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

 
namespace Nop.Data.Mapping.Localization
{
    public class LanguageMapping : EntityTypeConfiguration<Nop.Data.Domain.Localization.Language>
    {
        public LanguageMapping()
        {
            ToTable("Language");
            this.HasKey(l => new { l.Id });
            //this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Ignore(m => m.CommentSourceName); 
        }
    }

    public class LanguageOfSiteMapping : EntityTypeConfiguration<Nop.Data.Domain.Localization.LanguageOfSite>
    {
        public LanguageOfSiteMapping()
        {
            ToTable("LanguageOfSite");
            this.HasKey(l => new { l.Id, l.SiteId });
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Ignore(m => m.CommentSourceName); 
        }
    }
}
