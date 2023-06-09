﻿using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class User
    {
        public static Error UserNotFound => Error.NotFound(code: "User.NotFound", description: "user not found.");
        public static Error CommerceNotFound => Error.NotFound(code: "User.CommerceNotFound", description: "commerce not found.");
        public static Error ArticleNotFound => Error.NotFound(code: "User.ArticleNotFound", description: "article not found.");
        public static Error FavCommerceAddedAlready => Error.Conflict(code: "User.FavoriteCommerceAlreadyAdded", description: "commerce is already added in favorites.");
        public static Error FavArticleAddedAlready => Error.Conflict(code: "User.FavoriteArticleAlreadyAdded", description: "article is already added in favorites.");
    }
}
