using MODEL.QueryModels.Geo;

namespace DAL.IRepositories;

public interface IReferenteDataRepository
{
    public Task<CountriesQueryModel> GetCountries();
}
