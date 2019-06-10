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
        public static List<RequestHelper> GetRequestHistory() => RequestHistory;
        public static RequestHelper LastRequest() => RequestHistory?.LastOrDefault();

        public static void Clear()
        {
            ProductCache?.Clear();
            RequestHistory?.Clear();
        }
        public static void ClearCache()
        {
            ProductCache?.Clear();
        }

        public static bool ContainsProducts() => (RequestHistory is null || RequestHistory.Count == 0) ? false : true;
        public static void RemoveRange(RequestHelper request) => RequestHistory.RemoveAll(p => p.Key == request.Key && p.Value == request.Value);

        public static void AddRange(List<T> products)
        {
            if (ProductCache is null)
            {
                ProductCache = new List<T>(products);
            }
            else
            {
                foreach (var product in products)
                {
                    if (!ProductCache.Contains(product))
                    {
                        ProductCache.Add(product);
                    }
                }
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

        public static List<T> GetProductsByRequests(List<RequestHelper> requests)
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

        public static List<T> GetProductsByRequests(RequestHelper request)
        {
            return ProductCache.Where(p => p.GetType()
                                             .GetProperty(request.Key)
                                             .GetValue(p).Equals(request.Value))
                                             .ToList();
        }
    }
}
