using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Controllers.BaseApiModel;
using TechStore.Data;
using TS.DataAccessLayer.Models.Abstract;

namespace TechStore.Repository
{
    public abstract class ProductRepository<T> where T : Product
    {
        public readonly StoreDbContext _context;
        public readonly DbSet<T> _entity;

        protected ProductRepository(StoreDbContext context)
        {
            this._context = context;
            this._entity = context.Set<T>();
        }

        public async virtual Task<List<T>> GetAllProductsAsnyc()
        {
            return await _entity.ToListAsync();
        }

        public async virtual Task<T> GetProductByIdAsync(long id)
        {
            var product = await _entity.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return null;
            }

            return product;
        }

        public async virtual Task<T> SaveAsync(T product)
        {
            _entity.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async virtual Task<T> RemoveAsync(T product)
        {
            _entity.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async virtual Task<T> RemoveProductByIdAsync(long id)
        {
            var product = await GetProductByIdAsync(id);
            await RemoveAsync(product);
            return product;
        }

        public async virtual Task<List<T>> GetProductsByRequests(RequestHelper request)
        {
            return await _entity.Where(p => p.GetType()
                                             .GetProperty(request.Key)
                                             .GetValue(p).Equals(request.Value))
                                             .ToListAsync();
        }

        public virtual List<T> GetProductsByRequests(List<RequestHelper> requests)
        {
            var products = new List<T>();

            foreach (var product in _entity.AsParallel())
            {
                bool allpropertyEquals = true;
                foreach (var request in requests.AsParallel())
                {
                    var type = product.GetType();
                    var prop = type.GetProperty(request.Key);
                    var value = (string)prop.GetValue(product);

                    if (value != request.Value)
                    {
                        allpropertyEquals = false;
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
