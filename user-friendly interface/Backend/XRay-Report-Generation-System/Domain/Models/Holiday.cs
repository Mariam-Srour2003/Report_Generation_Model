using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using Domain.Models.SharedEntity;

namespace Domain.Models
{
    public class Holiday : Entity
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}

