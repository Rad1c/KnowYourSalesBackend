using MODEL.QueryModels.Geo;
using MODEL.QueryModels.ReferenteData;

namespace DAL.IRepositories;

public interface IReferenteDataRepository
{
    public Task<CountriesQueryModel> GetCountries();
    public Task<List<CategoryQueryModel>> GetCategories();
}
