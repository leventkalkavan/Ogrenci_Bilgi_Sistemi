using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Obs.Models
{
    public class Akademisyen
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        //public ICollection<Bolum> Bolumler { get; set; }
        public int BolumId { get; set; }
        public Bolum Bolum { get; set; }
        
        
    }
}
