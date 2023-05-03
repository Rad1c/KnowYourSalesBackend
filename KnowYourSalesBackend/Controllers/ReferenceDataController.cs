using DAL.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MODEL.QueryModels.ReferenceData;

namespace API.Controllers;

public class ReferenceDataController : BaseController
{
    private readonly IReferenceDataRepository _referenteDataRepository;

    public ReferenceDataController(IReferenceDataRepository referenteDataRepository)
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
}
