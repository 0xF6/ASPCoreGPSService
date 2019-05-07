namespace gps_service.Core
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DBContext : DbContext
    {
        #region DbSets

        public DbSet<GeoPosition> Positions { get; set; }

        #endregion


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseInMemoryDatabase("gps-db.raw");
    }
}