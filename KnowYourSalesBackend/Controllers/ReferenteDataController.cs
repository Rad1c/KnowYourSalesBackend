using DAL.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MODEL.QueryModels.Geo;

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
}
