using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class User
    {
        public static Error UserNotFound => Error.NotFound(code: "User.NotFound", description: "user not found.");
    }
}
