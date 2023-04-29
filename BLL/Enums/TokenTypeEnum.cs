namespace BLL.Enums
{
    public class TokenTypeEnum : Enumeration
    {
        public static readonly TokenTypeEnum AccessToken = new(1, "Access");
        public static readonly TokenTypeEnum RefreshToken = new(1, "Refresh");

        public TokenTypeEnum(int id, string code) : base(id, code) { }
    }
}
