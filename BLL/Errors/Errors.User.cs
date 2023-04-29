using ErrorOr;

namespace BLL.Errors
{
    public partial class Errors
    {
        public static class User
        {
            //TODO: remove this line. This is implemented just for example of partial class :)
            public static Error DuplicateEmail => Error.Conflict(code: "User.DuplicateEmail");
        }
    }
}
