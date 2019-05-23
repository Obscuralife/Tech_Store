using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Data;
using TS.DataAccessLayer.Models;

namespace TechStore.Repository
{
    public class CpuRepository : ProductRepository<Cpu>
    {
        public CpuRepository(StoreDbContext context) : base(context) { }

    }
}
