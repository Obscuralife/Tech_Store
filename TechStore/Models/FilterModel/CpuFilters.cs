using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Data;
using TS.DataAccessLayer.Models;

namespace TechStore.Models.FilterModel
{
    public class CpuFilters
    {
        public List<string> Vendors { get; private set; }
        public List<string> Cores { get; private set; }
        public List<string> Properties { get; private set; }
        public List<string> ModelsSerias { get; private set; }
        public List<string> Sockets { get; private set; }

        public CpuFilters(StoreDbContext context)
        {
            Vendors = new List<string>();
            Cores = new List<string>();
            Properties = new List<string>();
            ModelsSerias = new List<string>();
            Sockets = new List<string>();

            foreach (var cpu in context.Cpus)
            {
                if (!Vendors.Contains(cpu.Vendor))
                {
                    Vendors.Add(cpu.Vendor);
                }
                if (!Cores.Contains(cpu.Cores))
                {
                    Cores.Add(cpu.Cores);
                }
                if (!ModelsSerias.Contains(cpu.ModelSeries))
                {
                    ModelsSerias.Add(cpu.ModelSeries);
                }
                if (!Sockets.Contains(cpu.Socket))
                {
                    Sockets.Add(cpu.Socket);
                }

                var properties = (typeof(Cpu)).GetProperties();
                foreach (var propertyInfo in properties)
                {
                    string property = propertyInfo.Name;

                    if (!Properties.Contains(property))
                    {
                        switch (property)
                        {
                            case "Price":
                                Properties.Add(property);
                                break;
                            case "CoreFrequence":
                                Properties.Add(property);
                                break;
                            case "Year":
                                Properties.Add(property);
                                break;
                            case "Cache":
                                Properties.Add(property);
                                break;
                        }
                    }
                }
            }
        }
    }
}
