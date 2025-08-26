namespace SuGarToolkit.SourceGenerators;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class RelayPropertyAttribute : Attribute
{
    public RelayPropertyAttribute(string path)
    {
        _path = path;
    }

    private readonly string _path;
}