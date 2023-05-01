using DAL.IRepositories;
using Dapper;
using MODEL;
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

        public async Task<List<CountryQueryModel>> GetCountries()
        {
            string query = "SELECT * FROM mv_countries";

            var lookup = new Dictionary<Guid, CountryQueryModel>();

            using var connection = _queryContext.CreateConnection();
            var result = await connection.QueryAsync<CountryQueryModel, CityQueryModel, CountryQueryModel>(
                sql: query,
                map: (country, city) =>
                {
                    if (!lookup.TryGetValue(country.CountryId, out var c))
                    {
                        c = country;
                        c.Cities = new List<CityQueryModel>();
                        lookup.Add(c.CountryId, c);
                    }

                    c.Cities.Add(city);
                    return c;
                },
                splitOn: "CityId"
            );

            return result.Distinct().ToList();
        }

        public async Task<List<CategoryQueryModel>> GetCategories()
        {
            string query = "SELECT * FROM mv_categories";
            using var connection = _queryContext.CreateConnection();

            var result = await connection.QueryAsync<CategoryQueryModel>(query);

            return (List<CategoryQueryModel>)result;
        }

        public async Task<List<CurrencyQueryModel>> GetCurrencies()
        {
            string query = "SELECT id, name, code FROM currency";
            using var connection = _queryContext.CreateConnection();

            var result = await connection.QueryAsync<CurrencyQueryModel>(query);

            return (List<CurrencyQueryModel>)result;
        }
    }
}
