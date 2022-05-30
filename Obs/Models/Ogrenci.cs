using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Obs.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int BolumId { get; set; }
        public Bolum Bolum { get; set; }
        public int YariyilId { get; set; }
        public Yariyil Yariyil { get; set; }
        public string Password { get; set; }
        // public ICollection<DersOgrenci> DersListesi { get; set; }
        public string VizeN { get; set; }
        public string FinalN { get; set; }
    }


    }

