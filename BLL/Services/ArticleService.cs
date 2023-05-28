﻿using BLL.Helper;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using MODEL.Entities;

namespace BLL.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly IShopRepository _shopRepository;

    public ArticleService(IArticleRepository articleRepository, IShopRepository shopRepository)
    {
        _articleRepository = articleRepository;
        _shopRepository = shopRepository;   
    }

    public async Task<ErrorOr<Article?>> CreateArticle(Guid commerceId, List<Guid> shopIds, List<Guid> categoryIds, string name, string description, decimal oldPrice, decimal newPrice, string validDate)
    {
        Commerce? commerce = await _shopRepository.GetCommerceWithShops(commerceId);

        foreach (var item in shopIds)
        {
            if(!commerce.Shops.Any(x => x.Id == item))
            {
                return Errors.Errors.Shop.ShopNotFound;
            }
        }

        List<Category> categories = await _shopRepository.GetCategories();

        if (categories is null) return Errors.Errors.Shop.CategoryNotFound;

        foreach (var item in categoryIds)
        {
            if (!categories.Any(x => x.Id == item))
            {
                return Errors.Errors.Shop.ShopNotFound;
            }
        }

        Article? article = await _articleRepository.GetArticleByName(commerceId, name);

        if (article is not null) return Errors.Errors.ArticleEr.ArticleAddedAlready;

        Article newArticle = new()
        {
            Name = name,
            Description = description,
            OldPrice = oldPrice,
            NewPrice = newPrice,
            ValidDate = (DateTime)BaseHelper.ConvertStringToDateTime(validDate),
            Sale = BaseHelper.CalculateSale(oldPrice, newPrice)
        };

        _articleRepository.Save<Article>(newArticle);
        return newArticle;
        //TODO: sending the mail to user on article creation
    }

    public async Task<ErrorOr<bool>> DeleteArticle(Guid id)
    {
        Article? article = await _articleRepository.GetArticleById(id);

        if (article is null) 
            return Errors.Errors.ArticleEr.ArticleNotFound;

        article.IsDeleted = true;

        _articleRepository.UpdateEntity<Article>(article!);
        return true;
    }

    public async Task<ErrorOr<bool>> UpdateArticle(Guid articleId, string? name, string? description, decimal? newPrice, string? validDate)
    {
        Article? article = await _articleRepository.GetArticleById(articleId);

        if (article is null)
            return Errors.Errors.ArticleEr.ArticleNotFound;

        if (name is not null) article.Name = name;

        if (description is not null) article.Description = description;

        if (newPrice is not null) 
        { 
            article.NewPrice = (decimal)newPrice;
            article.Sale = BaseHelper.CalculateSale(article.OldPrice, newPrice.Value);
        }

        if (validDate is not null)
        {
            DateTime date = (DateTime)BaseHelper.ConvertStringToDateTime(validDate);
            article.ValidDate = date;
        }

        _articleRepository.UpdateEntity<Article>(article!);

        return true;
    }
}