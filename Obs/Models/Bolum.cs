using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Obs.Models
{
    public class Bolum
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
    }
}
