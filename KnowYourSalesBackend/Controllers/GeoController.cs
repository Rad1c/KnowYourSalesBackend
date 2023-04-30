using DAL.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MODEL.QueryModels.Geo;

namespace API.Controllers;

public class GeoController : BaseController
{
    private readonly IGeoRepository _geoRepository;

    public GeoController(IGeoRepository geoRepository)
    {
        _geoRepository = geoRepository;
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetContries()
    {
        CountriesQueryModel result = await _geoRepository.GetCountries();

        return Ok(result);
    }
}
