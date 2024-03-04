using B2BCorp.Contracts.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BCorp.Contracts.ResourceAccessors.Product
{
    public interface IProductEditRA
    {
        Task<Result<Guid>> AddProduct(string name, decimal price, int minAllowed, int maxAllowed);
        public Task<Result> ActivateProduct(Guid productId);
        public Task<Result> DiscontinueProduct(Guid productId);
    }
}
