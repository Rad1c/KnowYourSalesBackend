using API.Dtos;
using API.Models;
using BLL.IServices;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MODEL.QueryModels.Commerce;

namespace API.Controllers;

public class CommerceController : BaseController
{
    private readonly ICommerceService _commerceService;
    private readonly ISessionService _sessionService;
    private readonly IImageServices _imageServices;

    public CommerceController(ICommerceService commerceService, ISessionService sessionService, IImageServices imageServices)
    {
        _commerceService = commerceService;
        _sessionService = sessionService;
        _imageServices = imageServices;
    }

    [HttpPut("commerce")]
    public async Task<IActionResult> UpdateCommerce(UpdateCommerceModel req)
    {
        string logoPath = String.Empty;

        if (!string.IsNullOrEmpty(req.Logo))
        {
            var logoPathResult = await _imageServices.AddCommerceImage(_sessionService.Id, req.Logo);

            if (!logoPathResult.IsError) logoPath = logoPathResult.Value;
        }

        ErrorOr<bool> updateResult = await _commerceService.UpdateCommerce(
            _sessionService.Id,
            req.Name,
            logoPath,
            req.CityId
            );

        return OkResponse<bool>(updateResult, "commerce updated.");
    }

    [AllowAnonymous]
    [HttpGet("commerce/{id}")]
    public async Task<IActionResult> GetCommerce(Guid id)
    {
        CommerceQueryModel? commerce = await _commerceService.GetCommerceQuery(id);

        if (commerce is null) return Problem(BLL.Errors.Errors.Commerce.CommerceNotFound);

        return Ok(commerce);
    }

    [HttpDelete("commerce")]
    public async Task<IActionResult> DeleteCommerce()
    {
        ErrorOr<bool> result = await _commerceService.DeleteCommerce(_sessionService.Id);

        if (result.IsError) return Problem(result.Errors);

        return Ok(new MessageDto("commerce deleted."));
    }
}
