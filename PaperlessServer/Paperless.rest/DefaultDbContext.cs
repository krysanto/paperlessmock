using Microsoft.EntityFrameworkCore;
using Paperless.rest.Models;

namespace Paperless.rest
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }
        public DbSet<Correspondent> Correspondents { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Correspondent>().HasKey(e => e.Id);
            modelBuilder.Entity<Document>().HasKey(e => e.Id);
            modelBuilder.Entity<DocumentType>().HasKey(e => e.Id);
        }
    }
}
