namespace Architeptable.Models;

public static partial class Mock
{
    public static object App => new object[]
    {
        Recipes,
        Ingredients
    };
}