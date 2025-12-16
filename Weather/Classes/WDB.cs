using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Weather.Classes
{
    public class WDB:DbContext
    {
        public DbSet<Cash> Cache { get; set; }
        public DbSet<ApiWeather> ApiUsage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql("server=127.0.0.1;port=3306;user=root;password=;database=WP;", new MySqlServerVersion(new Version(8, 0))
    );
        }
    }
}