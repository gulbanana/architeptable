namespace Architeptable.Models;

public static partial class Mock
{
    public static readonly Factories Factories = new(null)
    {
        All = new[]
        {
            new Factories.Row { Name = "Nuclear Power Plant" }
        }
    };
}