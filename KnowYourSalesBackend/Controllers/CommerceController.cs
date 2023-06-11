using API.Dtos;
using API.Models.UpdateCommerce;
using BLL.IServices;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MODEL.QueryModels.Commerce;

namespace API.Controllers;

public class CommerceController : BaseController
{
    private readonly ICommerceService _commerceService;

    public CommerceController(ICommerceService commerceService)
    {
        _commerceService = commerceService;
    }

    [HttpPut("commerce")]
    public async Task<IActionResult> UpdateCommerce(UpdateCommerceModel req)
    {
        ValidationResult results = new UpdateCommerceModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<bool> updateResult = await _commerceService.UpdateCommerce(
            req.CommerceId,
            req.Name,
            req.Logo,
            req.CityId
            );

        return OkResponse<bool>(updateResult, "commerce updated.");
    }

    [HttpGet("commerce/{id}")]
    public async Task<IActionResult> GetCommerce(Guid id)
    {
        CommerceQueryModel? commerce = await _commerceService.GetCommerceQuery(id);

        if (commerce is null) return Problem(BLL.Errors.Errors.CommerceEr.CommerceNotFound);

        return Ok(commerce);
    }

    [HttpDelete("commerce/{id}")]
    public async Task<IActionResult> DeleteCommerce(Guid id)
    {
        ErrorOr<bool> result = await _commerceService.DeleteCommerce(id);

        if (result.IsError) return Problem(result.Errors);

        return Ok(new MessageDto("commerce deleted."));
    }
}
