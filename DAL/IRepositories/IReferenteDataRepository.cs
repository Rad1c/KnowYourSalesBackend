﻿using MODEL.QueryModels.ReferenteData;

namespace DAL.IRepositories;

public interface IReferenteDataRepository
{
    public Task<List<CategoryQueryModel>> GetCategories();
    public Task<List<CountryQueryModel>> GetCountries();
    public Task<List<CurrencyQueryModel>> GetCurrencies();
}