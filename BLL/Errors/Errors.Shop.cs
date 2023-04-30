﻿using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class Shop
    {
        public static Error CommerceNotFound => Error.NotFound(code: "Shop.CommerceNotFound", "commerce not found.");
        public static Error CityNotFound => Error.NotFound(code: "Shop.CityNotFound", "city not found.");
        public static Error ShopAlreadyExist => Error.NotFound(code: "Shop.ShopAlreadyExist", "shop already exist.");
    }
}