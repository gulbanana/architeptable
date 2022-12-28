namespace Architeptable.Models;

public static partial class Mock
{
    public static object App => new object[]
    {
        new Factories(),
        Recipes,
        Ingredients
    };
}