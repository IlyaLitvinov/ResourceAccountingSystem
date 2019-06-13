using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RAS.Models.Models
{
    public class House
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }

        public int? CounterOfWaterId { get; set; }
        public CounterOfWater CounterOfWater { get; set; }
    }
}
