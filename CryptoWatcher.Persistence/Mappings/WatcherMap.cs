﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoWatcher.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoWatcher.Persistence.Mappings
{
    public class WatcherMap
    {
        public WatcherMap(EntityTypeBuilder<Watcher> entityBuilder)
        {
            // Properties
            entityBuilder.Property(t => t.UserId)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.CurrencyId)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.IndicatorId)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.Value)
                .HasColumnType("decimal")
                .IsRequired();

            entityBuilder.Property(t => t.Enabled)
                .HasColumnType("bit")
                .IsRequired();

            entityBuilder.Property(t => t.Buy)
                .HasColumnType("decimal")
                .IsRequired();

            entityBuilder.Property(t => t.Sell)
                .HasColumnType("decimal")
                .IsRequired();           
        }
    }
}
