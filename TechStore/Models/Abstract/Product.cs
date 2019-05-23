using System;
using System.Collections.Generic;
using System.Text;

namespace TS.DataAccessLayer.Models.Abstract
{
    public abstract class Product : ICategory
    {
        public string CategoryName { get; set; }
        public long Id { get; set; }
        public string Vendor { get; set; }
        public string Year { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
    }
}
