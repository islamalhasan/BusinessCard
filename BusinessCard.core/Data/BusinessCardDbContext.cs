using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.core.Data
{
    public class BusinessCardDbContext:DbContext
    {
        public class BusinessCardDbContextFactory : IDesignTimeDbContextFactory<BusinessCardDbContext>
        {
            public BusinessCardDbContext CreateDbContext(string[] args)
            {
                // Define the options to use for the DbContext
                var optionsBuilder = new DbContextOptionsBuilder<BusinessCardDbContext>();

                // Use the same connection string used in your application
                var connectionString = "Server=(localdb)\\mssqllocaldb;Database=BusinessCard;Integrated Security=True;MultipleActiveResultSets=true;";

                optionsBuilder.UseSqlServer(connectionString);

                return new BusinessCardDbContext(optionsBuilder.Options);
            }
        }
        public BusinessCardDbContext(DbContextOptions<BusinessCardDbContext> options) :base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Businesscards> businesscards { get; set; }
        public DbSet<ImportLogs> importLogs { get; set; }
        public DbSet<ExportLogs> exportLogs { get; set; }


    }
}
