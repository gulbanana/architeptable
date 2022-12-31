namespace Architeptable.Models;

public static partial class Mock
{
    public static readonly Factories Factories = new()
    {
        All = new[]
        {
            new Factories.Facility { Name = "Nuclear Power Plant" }
        }
    };
}