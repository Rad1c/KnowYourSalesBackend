using ErrorOr;

namespace BLL.Errors
{
    public partial class Errors
    {
        public static class Auth
        {
            public static Error InvalidCredentials => Error.Conflict(code: "Auth.InvalidCred", description: "invalid credentials.");
            public static Error BadToken => Error.Validation(code: "Auth.BadToken", description: "bad token.");
        }
    }
}
