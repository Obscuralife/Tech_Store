using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Models.FilteredProducts;
using TechStore.Repository;
using TS.DataAccessLayer.Models.Abstract;

namespace TechStore.Controllers.BaseApiModel
{
    public abstract class BaseProductApiController<T> : ControllerBase where T : Product
    {
        private readonly ProductRepository<T> _repository;

        public BaseProductApiController(ProductRepository<T> productRepository)
        {
            _repository = productRepository;
        }

        public virtual int GetProductsCount() =>(FilteredProductsRepository<T>.RequestHistory.Count == 0) ? GetProductsCount() : _repository._entity.Count();

        public virtual void ClearFilteredProducts() => FilteredProductsRepository<T>.Clear();

        [HttpPost]
        public virtual IActionResult AddPrepareProducts(RequestHelper request)
        {
            FilteredProductsRepository<T>.AddRequestHistory(request);
            var requestVendorsCount = FilteredProductsRepository<T>.RequestHistory.Count(p => p.Key == "Vendor");
            var requests = FilteredProductsRepository<T>.GetRequestHistory();

            if (requestVendorsCount == 0)
            {
                var prod = _repository.GetProductsByRequests(request);
                FilteredProductsRepository<T>.AddRange(prod.Result);
            }
            else
            {
                foreach (var _request in requests.AsParallel())
                {
                    var newRequests = new List<RequestHelper>(requests.Where(p => p.Key != "Vendor").ToList()) { _request };

                    if (_request.Key == "Vendor" && requestVendorsCount > 1)
                    {
                        var prod = _repository.GetProductsByRequests(newRequests);
                        FilteredProductsRepository<T>.AddRange(prod);
                    }
                    else if (_request.Key == "Vendor")
                    {
                        var products = _repository.GetProductsByRequests(requests);
                        FilteredProductsRepository<T>.AddRange(products);
                    }
                    else if (requestVendorsCount != 0)
                    {
                        var prod = FilteredProductsRepository<T>.GetProductsByPropertyAsync(newRequests);
                        FilteredProductsRepository<T>.AddRange(prod);
                    }
                }
            }
            return Ok(request);
        }

        [HttpPost]
        public virtual IActionResult RemovePrepareProducts(RequestHelper request)
        {
            FilteredProductsRepository<T>.RemoveRange(request);

            return Ok(request);
        }


    }
}
