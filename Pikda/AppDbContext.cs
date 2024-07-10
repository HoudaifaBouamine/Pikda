using Microsoft.EntityFrameworkCore;
using Pikda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pikda
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<OcrModel> OcrModels { get; set; }
        public DbSet<Area> Areas { get; set; }
    }
}
