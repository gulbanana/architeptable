using System.Collections.Generic;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly List<Ingredients.Ingredient> ingredients = new()
    {
        new() { Name = "Water" },
        new() { Name = "Iron Ore" },
        new() { Name = "Iron Ingot" },
        new() { Name = "Copper Ore" },        
        new() { Name = "Copper Ingot" },
    };

    public static readonly Ingredients Ingredients = new()
    {
        All = ingredients
    };
}