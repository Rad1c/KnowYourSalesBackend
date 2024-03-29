﻿using ErrorOr;
using MODEL.Entities;
using MODEL.QueryModels.Commerce;

namespace BLL.IServices
{
    public interface ICommerceService
    {
        public Task<ErrorOr<Commerce?>> RegisterCommerce(string name, byte[] passwordHash, byte[] salt, Guid cityId, string email, string emailVericationCode);
        public Task<ErrorOr<bool>> UpdateCommerce(Guid commerceId, string? name, string? logo, Guid? cityId);
        public Task<CommerceQueryModel?> GetCommerceQuery(Guid id);
        public Task<ErrorOr<bool>> DeleteCommerce(Guid id);
        public Task<Commerce?> GetCommerceByAccountId(Guid accId);
    }
}
