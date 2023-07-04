using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class Commerce
    {
        public static Error CityNotFound => Error.NotFound(code: "Commerce.CityNotFound", description: "city not found.");
        public static Error CommerceNotFound => Error.NotFound(code: "Commerce.NotFound", description: "commerce not found.");
        public static Error CommerceAlreadyExist => Error.Conflict(code: "Commerce.AlreadyExist", description: "commerce already exist.");
    }
}

