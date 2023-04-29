namespace BLL.Enums
{
    public class SexEnum : Enumeration
    {
        public readonly SexEnum Male = new(1, "M");
        public readonly SexEnum Female = new(1, "F");

        public SexEnum(int id, string code) : base(id, code) { }
    }
}
