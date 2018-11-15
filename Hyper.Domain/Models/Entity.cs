﻿using System;

namespace Hyper.Domain.Models
{
    public abstract class Entity
    {
        public string Id { get; protected set; }
        //public User CreatedBy { get; set; }
        public DateTime CreationTime { get; protected set; }
        //public User LastModifiedBy { get; set; }
        //public DateTime LastModificationTime { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid().ToString();
            CreationTime = DateTime.Now;
        }
    }
}