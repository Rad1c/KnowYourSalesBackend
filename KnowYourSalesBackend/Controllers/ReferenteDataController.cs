using API.Models;
using API.Models.Validators;
using DAL.IRepositories;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MODEL.QueryModels.ReferenteData;

namespace API.Controllers;

[AllowAnonymous]
public class ReferenteDataController : BaseController
{
    private readonly IReferenteDataRepository _referenteDataRepository;

    public ReferenteDataController(IReferenteDataRepository referenteDataRepository)
    {
        _referenteDataRepository = referenteDataRepository;
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetContries()
    {
        List<CountryQueryModel> result = await _referenteDataRepository.GetCountries();

        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        List<CategoryQueryModel> result = await _referenteDataRepository.GetCategories();

        return Ok(result);
    }

    [HttpGet("currencies")]
    public async Task<IActionResult> GetCurrencies()
    {
        List<CurrencyQueryModel> result = await _referenteDataRepository.GetCurrencies();

        return Ok(result);
    }

    [HttpGet("/country/cities")]
    public async Task<IActionResult> GetCitiesByCountryCode([FromQuery] GetCitiesByCountryCodeQueryModel req)
    {
        ValidationResult results = new GetCitiesByCountryCodeQueryModelValidator().Validate(req);

        if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        List<CityQueryModel> result = await _referenteDataRepository.GetCitiesByCountryCode(req.Code);

        return Ok(result);
    }
}
