using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.core.Data
{
    public class BusinessCardDbContext:DbContext
    {
        public BusinessCardDbContext(DbContextOptions options) :base(options)
        {

        }
        public DbSet<User> User { get; set; }

    }
}
