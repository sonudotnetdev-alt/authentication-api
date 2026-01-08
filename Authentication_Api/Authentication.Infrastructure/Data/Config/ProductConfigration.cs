using Authentication.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Data.Config
{
    public class ProductConfigration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(128);
            builder.Property(x => x.Description).HasMaxLength(128);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

            //Seeding 
            builder.HasData(
                new Products { Id = 1, Name = "Product 1", Description = "Description 1", Price = 100, CategoryId = 1},
                new Products { Id = 2, Name = "Product 2", Description = "Description 2", Price = 300, CategoryId = 1},
                new Products { Id = 3, Name = "Product 3", Description = "Description 3", Price = 500, CategoryId = 3},
                new Products { Id = 4, Name = "Product 4", Description = "Description 4", Price = 900, CategoryId = 2},
                new Products { Id = 5, Name = "Product 5", Description = "Description 5", Price = 1900, CategoryId = 2 },
                new Products { Id = 6, Name = "Product 6", Description = "Description 6", Price = 2900, CategoryId = 3 },
                new Products { Id = 7, Name = "Product 7", Description = "Description 7", Price = 3900, CategoryId = 3 },
                new Products { Id = 8, Name = "Product 8", Description = "Description 8", Price = 4900, CategoryId = 1 },
                new Products { Id = 9, Name = "Product 9", Description = "Description 9", Price = 5900, CategoryId = 2 }
                );
        }
    }
}
