using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Controllers.BaseApiModel;
using TS.DataAccessLayer.Models.Abstract;

namespace TechStore.Models.FilteredProducts
{
    public static class FilteredProductsRepository<T> where T : Product
    {
        public static List<T> ProductCache { get; private set; }
        public static List<RequestHelper> RequestHistory { get; private set; }


        public static int Count() => ProductCache.Count;
        public static void Clear()
        {
            ProductCache.Clear();
            RequestHistory.Clear();
        }
        public static List<RequestHelper> GetRequestHistory() => RequestHistory;

        public static void RemoveRange(RequestHelper request)
        {
            var products = ProductCache.Where(p => p.GetType()
                                             .GetProperty(request.Key)
                                             .GetValue(p).Equals(request.Value))
                                             .ToList();

            foreach (var product in products.AsParallel())
            {
                ProductCache.Remove(product);
            }

            var requests = RequestHistory.Where(p => p == request);
            foreach (var item in requests)
            {
                RequestHistory.Remove(item);
            }
        }

        public static void AddRange(List<T> products)
        {
            if (ProductCache is null)
            {
                ProductCache = new List<T>(products);
            }
            else
            {
                ProductCache.AddRange(products);
            }
        }

        public static void AddRequestHistory(RequestHelper request)
        {
            if (RequestHistory is null)
            {
                RequestHistory = new List<RequestHelper> { request };
            }
            else
            {
                RequestHistory.Add(request);
            }
        }

        public static List<T> GetProductsByPropertyAsync(List<RequestHelper> requests)
        {
            var products = new List<T>();

            foreach (var product in ProductCache.AsParallel())
            {
                bool allpropertyEquals = true;
                foreach (var request in requests)
                {
                    var type = product.GetType();
                    var prop = type.GetProperty(request.Key);
                    var value = (string)prop.GetValue(product);

                    if (value != request.Value)
                    {
                        allpropertyEquals = false;
                        break;
                    }
                }

                if (allpropertyEquals)
                {
                    products.Add(product);
                }
            }

            return products;
        }
    }
}
