using System.Reflection;

namespace BLL.Enums;

public abstract class Enumeration : IComparable
{
    public string Code { get; private set; }
    public int Id { get; private set; }

    protected Enumeration(int id, string code) => (Id, Code) = (id, code);

    public override string ToString() => Code;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null))
                    .Cast<T>();

    public static T FromDisplayName<T>(string displayName) where T : Enumeration
    {
        var matchingItem = Parse<T, string>(displayName, "display name", item => item.Code == displayName);
        return matchingItem;
    }

    private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

        return matchingItem;
    }

    public int CompareTo(object? other)
    {
        if (other is null)
            return -1;
        else
            return Code.CompareTo(((Enumeration)other).Code);
    }

    public static bool ContainsCode<T>(string code) where T : Enumeration
    {
        if (string.IsNullOrEmpty(code)) return false;

        List<T> allEnums = GetAll<T>().ToList();

        return allEnums.Select(x => x.Code.ToLower()).ToList()
        .Contains(code.ToLower());
    }

    public static T? GetByCode<T>(string? code) where T : Enumeration
    {
        if (code is null) return null;

        return GetAll<T>().FirstOrDefault(x => x.Code == code);
    }
}
