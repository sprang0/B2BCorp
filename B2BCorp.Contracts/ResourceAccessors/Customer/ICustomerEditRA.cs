﻿using B2BCorp.Contracts.DTOs.Common;

namespace B2BCorp.Contracts.ResourceAccessors.Customer
{
    public interface ICustomerEditRA
    {
        Task<Result<Guid>> AddCustomer(string name);
        Task<Result> VerifyCustomer(Guid customerId);
        Task<Result> SetCustomerCreditLimit(Guid customerId, decimal creditLimit);
    }
}
