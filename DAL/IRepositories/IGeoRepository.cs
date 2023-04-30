using MODEL.QueryModels.Geo;

namespace DAL.IRepositories;

public interface IGeoRepository
{
    public Task<CountriesQueryModel> GetCountries();
}
