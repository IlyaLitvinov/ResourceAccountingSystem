using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RAS.Models.Models
{
    public class CounterOfWater
    {
        [Key]
        public int Id { get; set; }
        public int? SerialNumber { get; set; }
        public int? Readings { get; set; }

        public IEnumerable<House> Houses { get; set; }
    }
}
