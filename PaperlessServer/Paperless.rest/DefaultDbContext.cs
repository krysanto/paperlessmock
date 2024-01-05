using Microsoft.EntityFrameworkCore;
using Paperless.rest.Models;

namespace Paperless.rest
{
    public class DefaultDbContext : DbContext
    {
        public DbSet<Correspondent> Correspondents { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }
    }
}
