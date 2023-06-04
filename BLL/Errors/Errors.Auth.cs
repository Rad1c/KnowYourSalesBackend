using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class AuthEr
    {
        public static Error InvalidCredentials => Error.Validation(code: "Auth.InvalidCred", description: "invalid credentials.");
        public static Error BadToken => Error.Validation(code: "Auth.BadToken", description: "bad token.");
    }
}
