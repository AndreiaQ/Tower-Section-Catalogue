using Microsoft.EntityFrameworkCore;

namespace Tower_Section_Catalogue.Data
{
    public class Tower_Section_CatalogueContext : DbContext
    {
        public Tower_Section_CatalogueContext (DbContextOptions<Tower_Section_CatalogueContext> options)
            : base(options)
        {
        }

        public DbSet<Tower_Section_Catalogue.Models.Shell>? Shell { get; set; } = default!;
        public DbSet<Tower_Section_Catalogue.Models.TowerSection>? TowerSection { get; set; }
    }
}
