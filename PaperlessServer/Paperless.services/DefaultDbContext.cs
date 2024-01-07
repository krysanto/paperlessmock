using Microsoft.EntityFrameworkCore;
using Paperless.services.Models;

namespace Paperless.rest
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }
        public DbSet<Correspondent> Correspondents { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

    }
}
