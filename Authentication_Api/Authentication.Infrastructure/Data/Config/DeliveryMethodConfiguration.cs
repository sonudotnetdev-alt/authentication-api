using Authentication.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Data.Config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.HasData(
                new DeliveryMethod { Id = 1, ShortName = "DHL", Description = "Fastest Delivery", DeliveryTime = "", Price = 50 },
                new DeliveryMethod { Id = 2, ShortName = "Fedex", Description = "Get it with 3 days", DeliveryTime = "", Price = 30 },
                new DeliveryMethod { Id = 3, ShortName = "Delhivery", Description = "Slow but cheap", DeliveryTime = "", Price = 20 },
                new DeliveryMethod { Id = 4, ShortName = "Aramex", Description = "Free", DeliveryTime = "", Price = 0 },
                new DeliveryMethod { Id = 5, ShortName = "Jumia", Description = "Free", DeliveryTime="", Price = 0 }
                );
        }
    }
}
