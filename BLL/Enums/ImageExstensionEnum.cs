namespace BLL.Enums;

public class ImageExstensionEnum : Enumeration
{
    public static readonly ImageExstensionEnum Png = new(1, "png");
    public static readonly ImageExstensionEnum Jpg = new(2, "jpg");
    public static readonly ImageExstensionEnum Jpeg = new(3, "jpeg");
    public static readonly ImageExstensionEnum Gif = new(4, "gif");
    public static readonly ImageExstensionEnum Svg = new(5, "svg");

    public ImageExstensionEnum(int id, string code) : base(id, code) { }
}
