using Microsoft.EntityFrameworkCore;
using JuntosEntities;
using JuntosData.Context.Interface;
 
namespace JuntosData.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbContext Instance => this;

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryDatabase");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasKey(p => p.ID);
        }
    }
}