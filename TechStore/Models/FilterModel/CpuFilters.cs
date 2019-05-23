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
        public Dictionary<string, bool> Vendors { get;  set; }
        public Dictionary<string, bool> Cores { get;  set; }
        public Dictionary<string, bool> ModelsSerias { get;  set; }
        public Dictionary<string, bool> Sockets { get;  set; }
        public List<string> Properties { get;  set; }

        public CpuFilters(StoreDbContext context)
        {
            Vendors = new Dictionary<string, bool>();
            Cores = new Dictionary<string, bool>();
            Properties = new List<string>();
            ModelsSerias = new Dictionary<string, bool>();
            Sockets = new Dictionary<string, bool>();
            foreach (var cpu in context.Cpus)
            {
                if (!Vendors.ContainsKey(cpu.Vendor))
                {
                    Vendors.Add(cpu.Vendor, false);
                }
                if (!Cores.ContainsKey(cpu.Cores))
                {
                    Cores.Add(cpu.Cores, false);
                }
                if (!ModelsSerias.ContainsKey(cpu.ModelSeries))
                {
                    ModelsSerias.Add(cpu.ModelSeries, false);
                }
                if (!Sockets.ContainsKey(cpu.Socket))
                {
                    Sockets.Add(cpu.Socket, false);
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
