using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.SharedEntity
{
    public abstract class Entity
    {
        [Key]
        public long Id { get; set; }
    }
}

