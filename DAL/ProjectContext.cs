using BE;
using System.Data.Entity;


namespace DAL
{
    public class ProjectContext : DbContext
    {
        public ProjectContext() : base("projectDB")
        {

        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Explosion> Explosions { get; set; }
    }
}
