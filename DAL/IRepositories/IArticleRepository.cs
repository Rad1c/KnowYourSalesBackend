﻿using MODEL.Entities;

namespace DAL.IRepositories;

public interface IArticleRepository : IRepository<Article>
{
    public Task<Article?> GetArticleById(Guid id);
    public Task<Article?> GetArticleByNameQuery(Guid id, string name);
    public Task<Article?> GetArticleByName(Guid id, string name);
}