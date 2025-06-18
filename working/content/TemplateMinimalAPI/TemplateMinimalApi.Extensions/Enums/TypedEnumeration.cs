namespace TemplateMinimalApi.Extensions.Enums;

public class TypedEnumeration : IComparable
{
    public int Id { get; private set; }
    public string? ServerName { get; private set; }
    public string? ServerIp { get; private set; }
    public string? DataBaseName { get; private set; }
    public string? ConnectionString { get; private set; }

    protected TypedEnumeration(int id, string? serverName, string? serverIp, string? databaseName, string? connectionString)
        => (Id, ServerName, ServerIp, DataBaseName, ConnectionString) = (id, serverName, serverIp, databaseName, connectionString);

    public override string ToString() => $"{ServerName}-{DataBaseName}";

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                 .Select(f => f.GetValue(null))
                 .Cast<T>();

    public override bool Equals(object obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    public int CompareTo(object obj) => Id.CompareTo(((Enumeration)obj).Id);
}
