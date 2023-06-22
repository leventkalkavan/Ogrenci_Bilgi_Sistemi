using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Obs.Models
{
    public class Ders
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int BolumId { get; set; }
        public int AkademisyenId { get; set; }
       

    }
}
