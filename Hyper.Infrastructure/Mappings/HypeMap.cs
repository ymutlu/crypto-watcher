﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Hyper.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Hyper.Persistence.Mappings
{
    public class HypeMap
    {
        public HypeMap(EntityTypeBuilder<Hype> entityBuilder)
        {
        }
    }
}