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
    public class Product
    {
        [Key]
        public int ProductNo { get; set; }

        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
    }

    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options){ }

        public DbSet<Product> TProduct { get; set; }
    }

    public static class ProductContextFactory
    {
        public static ProductContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
            optionsBuilder.UseMySQL(connectionString);

            //Ensure database creation
            var context = new ProductContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
