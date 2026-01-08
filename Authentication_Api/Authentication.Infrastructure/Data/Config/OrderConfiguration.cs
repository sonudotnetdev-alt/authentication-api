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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x=>x.shipAddress, n => { n.WithOwner(); });
            builder.Property(x=>x.orderStatus).HasConversion(o=>o.ToString(),o=>(OrderStatus)Enum.Parse(typeof(OrderStatus),o));
            builder.HasMany(x=>x.orderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");
        }
    }
}
