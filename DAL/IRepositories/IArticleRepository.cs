﻿using MODEL.Entities;
using MODEL.QueryModels.ReferenteData;

namespace DAL.IRepositories;

public interface IArticleRepository : IRepository<Article>
{
    public Task<Article?> GetArticleById(Guid id);
    public Task<Article?> GetArticleByNameQuery(Guid id, string name);
    public Task<Article?> GetArticleByName(Guid id, string name);
    public Task<ArticleQueryModel?> GetArticlesPaginatedQuery(int pageSize, int page, string? name = null, string? cityName = null, string? categoryName = null, Guid? commerceId = null);
}
