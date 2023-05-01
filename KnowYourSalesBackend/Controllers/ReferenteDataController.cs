using DAL.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MODEL.QueryModels.Geo;
using MODEL.QueryModels.ReferenteData;

namespace API.Controllers;

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
        CountriesQueryModel result = await _referenteDataRepository.GetCountries();

        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        List<CategoryQueryModel> result = await _referenteDataRepository.GetCategories();

        return Ok(result);
    }
}
