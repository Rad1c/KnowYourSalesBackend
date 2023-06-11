namespace BLL.Enums;

public class RoleEnum : Enumeration
{
    public static readonly RoleEnum Admin = new(1, "Admin");
    public static readonly RoleEnum User = new(2, "User");
    public static readonly RoleEnum Shop = new(3, "Shop");
    public static readonly RoleEnum Commerce = new(1, "Commerce");

    public RoleEnum(int id, string code) : base(id, code) { }
}

