using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Data;
using TS.DataAccessLayer.Models;

namespace TechStore.Repository
{
    public class GpuRepository : ProductRepository<Gpu>
    {
        public GpuRepository(StoreDbContext context) : base(context) { }
    }
}
