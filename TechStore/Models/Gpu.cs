using System;
using System.Collections.Generic;
using System.Text;
using TS.DataAccessLayer.Models.Abstract;

namespace TS.DataAccessLayer.Models
{
    public class Gpu : Product
    {
        public string ModelSeries { get; set; }
        public string CoreFrequence { get; set; }
        public string Memory { get; set; }
        public string MemoryType { get; set; }
        public string MemoryFrequence { get; set; }
    }
}
