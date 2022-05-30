using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Obs.Models
{
    public class Log
    {
        [Key]
        public int LogId { get; set; }
        public string LogType { get; set; }
        public string LogUser { get; set; }
        public DateTime LogTime { get; set; }
    }
}
