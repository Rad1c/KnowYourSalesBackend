using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace BLL.IServices;

public interface IImageServices
{
    public Task<ErrorOr<string>> AddArticleImage(Guid articleId, IFormFile img);
}
