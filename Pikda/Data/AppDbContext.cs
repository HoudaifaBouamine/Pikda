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
        public AppDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Database=Pikda;Username=postgres;Password=postgres");
        }

        public DbSet<OcrModel> OcrModels { get; set; }
        public DbSet<Area> Areas { get; set; }
    }
}
