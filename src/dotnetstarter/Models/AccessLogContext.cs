using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class AccessLog
    {
        [Key]
        public int AccessNo { get; set; }

        public string AccessUrl { get; set; }
        public string AccessIPAddr { get; set; }
        public string BrowserType { get; set; }
        public DateTime AccessDate { get; set; }
    }

    public class AccessLogContext : DbContext
    {
        public AccessLogContext(DbContextOptions<AccessLogContext> options) : base(options){ }

        public DbSet<AccessLog> taccesslog { get; set; }
    }

    public static class AccessLogContextFactory
    {
        public static AccessLogContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccessLogContext>();
            optionsBuilder.UseMySQL(connectionString);

            //Ensure database creation
            var context = new AccessLogContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
