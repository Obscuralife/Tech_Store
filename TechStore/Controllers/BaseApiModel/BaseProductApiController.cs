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

        public virtual int GetProductsCount() => _repository._entity.Count();

        public virtual int GetFilteredProductsCount() => (FilteredProductsRepository<T>.ContainsProducts()) ? FilteredProductsRepository<T>.Count() : GetProductsCount();

        public virtual void ClearFilteredProducts() => FilteredProductsRepository<T>.Clear();

        [HttpPost]
        public virtual IActionResult AddPrepareProducts(RequestHelper request)
        {
            var penultimate = FilteredProductsRepository<T>.LastRequest();
            FilteredProductsRepository<T>.AddRequestHistory(request);
            var RequestHistory = FilteredProductsRepository<T>.GetRequestHistory();
            var requestVendorsCount = FilteredProductsRepository<T>.RequestHistory.Count(p => p.Key == "Vendor");

            if (penultimate?.Key == request.Key && requestVendorsCount == 0)
            {
                var prod = _repository.GetProductsByRequests(request);
                FilteredProductsRepository<T>.AddRange(prod.Result);
            }
            else
            {
                var products = _repository.GetProductsByRequests(RequestHistory);
                FilteredProductsRepository<T>.ClearCache();
                FilteredProductsRepository<T>.AddRange(products);
            }

            //if (requestVendorsCount == 0)
            //{
            //    var prod = _repository.GetProductsByRequests(request);
            //    FilteredProductsRepository<T>.AddRange(prod.Result);
            //}
            //else
            //{
            //    if (request.Key == "Vendor" && requestVendorsCount > 1)
            //    {
            //        var newRequests = new List<RequestHelper>(RequestHistory.Where(p => p.Key != "Vendor").ToList()) { request };
            //        var prod = _repository.GetProductsByRequests(newRequests);
            //        FilteredProductsRepository<T>.AddRange(prod);
            //    }
            //    else if (request.Key == "Vendor")
            //    {
            //        var products = _repository.GetProductsByRequests(RequestHistory);
            //        FilteredProductsRepository<T>.AddRange(products);
            //    }
            //    else
            //    {
            //        var prod = new List<T>();
            //        if (penultimate?.Key == request.Key)
            //        {
            //            var newRequest = new List<RequestHelper>(RequestHistory.Where(p => p.Key == "Vendor").ToList()) { request };
            //            prod = _repository.GetProductsByRequests(newRequest);
            //        }
            //        else
            //        {
            //            prod = FilteredProductsRepository<T>.GetProductsByRequests(request);
            //            FilteredProductsRepository<T>.ClearCache();
            //        }
            //        FilteredProductsRepository<T>.AddRange(prod);
            //    }
            //}
            return Ok(request);
        }

        [HttpPost]
        public virtual IActionResult RemovePrepareProducts(RequestHelper request)
        {
            FilteredProductsRepository<T>.RemoveRange(request);
            var RequestHistory = FilteredProductsRepository<T>.GetRequestHistory().ToArray();


            if (RequestHistory.Length == 0)
            {
                FilteredProductsRepository<T>.ClearCache();
            }
            else
            {
                FilteredProductsRepository<T>.Clear();
                foreach (var requestItem in RequestHistory)
                {
                    AddPrepareProducts(requestItem);
                }
            }
            return Ok(request);
        }


    }
}
