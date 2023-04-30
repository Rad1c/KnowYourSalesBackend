using ErrorOr;
using MODEL.Entities;

namespace BLL.IServices
{
    public interface ICommerceService
    {
        public Task<ErrorOr<Commerce?>> RegisterCommerce(string name, byte[] passwordHash, byte[] salt, Guid cityId, string email);
    }
}
