using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class Images
    {
        public static Error ImgExstensionNotSuported => Error.Failure(code: "Image.ExstensionNotSuporte", "exstension not suported.");
    }
}
