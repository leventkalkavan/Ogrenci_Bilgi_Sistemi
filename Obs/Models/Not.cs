using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Obs.Models
{
    public class Not
    {
        [Key]
        public int Id { get; set; }
        public string  Name { get; set; }
        public int OgrenciId { get; set; }
        public User User { get; set; }
        public int DersId { get; set; }
        public Ders Ders { get; set; }
        public string VizeN { get; set; }
        public string FinalN { get; set; }
    }
}
