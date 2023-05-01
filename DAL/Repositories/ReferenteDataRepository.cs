using DAL.IRepositories;
using Dapper;
using MODEL;
using MODEL.QueryModels.Geo;
using MODEL.QueryModels.ReferenteData;

namespace DAL.Repositories
{
    public class ReferenteDataRepository : IReferenteDataRepository
    {
        private readonly QueryContext _queryContext;

        public ReferenteDataRepository(QueryContext queryContext)
        {
            _queryContext = queryContext;
        }

        public async Task<CountriesQueryModel> GetCountries()
        {
            string query = "SELECT * from mv_countries";

            var countries = new Dictionary<string, List<string>>();
            using var connection = _queryContext.CreateConnection();

            var queryResult = await connection.QueryAsync<(string CountryName, string CityName)>(query);

            foreach (var (countryName, cityName) in queryResult)
            {
                if (!countries.ContainsKey(countryName))
                {
                    countries[countryName] = new List<string>();
                }

                countries[countryName].Add(cityName);
            }

            return new CountriesQueryModel { Countries = countries };
        }

        public async Task<List<CategoryQueryModel>> GetCategories()
        {
            string query = "SELECT * from mv_categories";
            using var connection = _queryContext.CreateConnection();

            var result = await connection.QueryAsync<CategoryQueryModel>(query);

            return (List<CategoryQueryModel>)result;
        }
    }
}
