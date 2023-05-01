using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class CommerceEr
    {
        public static Error CityNotFound => Error.NotFound(code: "Commerce.CityNotFound", description: "city not found.");
        public static Error CommerceNotFound => Error.NotFound(code: "Commerce.NotFound", description: "commerce not found.");
    }
}

