using ErrorOr;

namespace BLL.Errors
{
    public partial class Errors
    {
        public static class Auth
        {
            public static Error InvalidCredentials => Error.Conflict(code: "Auth.InvalidCred", description: "invalid credentials.");
        }
    }
}
