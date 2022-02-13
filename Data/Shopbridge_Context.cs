using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shopbridge_base.Domain.Models;
using Shopbridge_base.Models;

namespace Shopbridge_base.Data
{
    public class Shopbridge_Context : DbContext
    {
        public Shopbridge_Context (DbContextOptions<Shopbridge_Context> options)
            : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
