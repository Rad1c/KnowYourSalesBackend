namespace BLL.Enums;

public class RoleEnum : Enumeration
{
    public static readonly RoleEnum Admin = new(1, "Admin");
    public static readonly RoleEnum User = new(1, "User");
    public static readonly RoleEnum Shop = new(1, "Shop");

    public RoleEnum(int id, string code) : base(id, code) { }
}

