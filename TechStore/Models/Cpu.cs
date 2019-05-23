using System;
using System.Collections.Generic;
using System.Text;
using TS.DataAccessLayer.Models.Abstract;

namespace TS.DataAccessLayer.Models
{
    public class Cpu : Product
    {
        public string ModelSeries { get; set; }
        public string Socket { get; set; }
        public string Cores { get; set; }
        public string CoreFrequence { get; set; }
        public string Cache { get; set; }
        
    }
}
