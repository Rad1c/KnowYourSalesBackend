using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace BLL.IServices;

public interface IImageServices
{
    public Task<ErrorOr<string>> AddArticleImage(Guid articleId, IFormFile img);
    public Task<ErrorOr<string>> AddArticleImage(Guid articleId, string base64Img);
    public Task<ErrorOr<string>> AddCommerceImage(Guid commerceId, string base64Img);
}