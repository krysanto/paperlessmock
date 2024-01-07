using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Paperless.Migrations;
using Paperless.rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.Migrations
{
    public class DefaultDBContextFactory : IDesignTimeDbContextFactory<DefaultDbContext>
    {
        public DefaultDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DefaultDbContext>();
            optionsBuilder.UseNpgsql("Host=db;Database=paperless;Username=paperless;Password=paperless;");

            return new DefaultDbContext(optionsBuilder.Options);
        }
    }
}
