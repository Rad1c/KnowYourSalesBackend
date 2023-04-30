using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class Commerce
    {
        public static Error CityNotFound => Error.NotFound(code: "Commerce.CityNotFound", description: "city not found.");
    }
}

