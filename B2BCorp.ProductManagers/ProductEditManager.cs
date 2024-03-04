using B2BCorp.Contracts.DTOs.Common;
using B2BCorp.Contracts.Managers.Product;
using B2BCorp.Contracts.ResourceAccessors.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BCorp.ProductManagers
{
    public class ProductEditManager(IProductEditRA productEditRA) : IProductEditManager
    {
        readonly IProductEditRA productEditRA = productEditRA;

        public async Task<Result<Guid>> AddProduct(string name, decimal price, int minAllowed, int maxAllowed)
        {
            return await productEditRA.AddProduct(name, price, minAllowed, maxAllowed);
        }

        public async Task<Result> ActivateProduct(Guid productId)
        {
            return await productEditRA.ActivateProduct(productId);
        }

        public async Task<Result> DiscontinueProduct(Guid productId)
        {
            return await productEditRA.DiscontinueProduct(productId);
        }
    }
}
