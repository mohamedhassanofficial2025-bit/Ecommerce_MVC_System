using EcommerceSystem.DAL.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
    {
        /*---------------------------------------------------*/

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public AppDbContext():base()
        {
        }
        /*---------------------------------------------------*/
        public override int SaveChanges()
        {
            AuditLog();
            return base.SaveChanges();
        }

        private void AuditLog()
        {
            var dateTime = DateTime.UtcNow;
            foreach(var entry in ChangeTracker.Entries<IAuditEntity>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = dateTime;
                else if(entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt= dateTime;
            }

        }
        /*---------------------------------------------------*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var createdAt = new DateTime(2026, 5, 11, 13, 45, 30, 120);
            List<Category> categories = new List<Category>()
            {
                new Category(){Id=1, Name="Electronics",CreatedAt=createdAt},
                new Category(){Id=2, Name="Accessories",CreatedAt=createdAt},
                new Category(){Id=3, Name="Cameras",CreatedAt=createdAt},
                new Category(){Id=4, Name="Home Appliances", CreatedAt = createdAt}
            };

            List<Product> products = new List<Product>()
            {
                new Product(){Id=1,Title="Laptop",Description="High performance laptop",Price=1000,Count=5,CategoryId=1,CreatedAt=createdAt},
                new Product(){Id=2,Title="Smartphone",Description="Latest model smartphone",Price=500,Count=10,CategoryId=1, CreatedAt = createdAt},
                new Product(){Id=3,Title="Headphones",Description="Noise cancelling headphones",Price=200,Count=15,CategoryId=2, CreatedAt = createdAt},
                new Product(){Id=4,Title="Camera",Description="Digital SLR camera",Price=800,Count=7,CategoryId=3, CreatedAt = createdAt},
                new Product(){Id=5,Title="Smartwatch",Description="Fitness tracking smartwatch",Price=300,Count=12,CategoryId=1, CreatedAt = createdAt},
                new Product(){Id=6,Title="Tablet",Description="Portable tablet device",Price=400,Count=9,CategoryId=1, CreatedAt = createdAt},
                new Product(){Id=7,Title="Microwave",Description="Kitchen microwave",Price=2500,Count=4,CategoryId=4, CreatedAt = createdAt},
                new Product(){Id=8,Title="Refrigerator",Description="Double door fridge",Price=9000,Count=2,CategoryId=4, CreatedAt = createdAt},
                new Product(){Id=9,Title="Blender",Description="Electric blender",Price=700,Count=6,CategoryId=4, CreatedAt = createdAt},
                new Product(){Id=10,Title="Keyboard",Description="Mechanical keyboard",Price=350,Count=20,CategoryId=2, CreatedAt = createdAt}
            };


            modelBuilder.Entity<Product>().HasData(products);
            modelBuilder.Entity<Category>().HasData(categories);
            base.OnModelCreating(modelBuilder);
        }
        /*---------------------------------------------------*/
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
    }
}
