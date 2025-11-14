using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Enums;

namespace Venice.Orders.Infrastructure.Data.SqlServer.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).IsRequired();
        builder.Property(o => o.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasColumnName("CreatedAt");
        builder.Property(o => o.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");
        builder.Property(o => o.CustomerId)
            .IsRequired()
            .HasColumnName("CustomerId");
        builder.Property(o => o.Date)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasColumnName("Date");
        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName("Status");
        builder.Property(o => o.Total)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("Total");

        builder.HasIndex(o => o.CustomerId)
            .HasDatabaseName("IX_Orders_CustomerId");
        builder.HasIndex(o => o.Date)
            .HasDatabaseName("IX_Orders_Date");
        builder.HasIndex(o => o.Status)
            .HasDatabaseName("IX_Orders_Status");

        builder.Ignore(o => o.Items);
    }
}

