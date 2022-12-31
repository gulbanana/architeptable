using System.Collections.Generic;
using System.Linq;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly List<Recipes.RecipeModel> recipes = new()
    {
        new()
        {
            Name = "Pure Iron Ingot",
            Ingredients = new List<Recipes.IngredientRow>
            {
                new() { Name = "Iron Ore", Quantity = 35 },
                new() { Name = "Water", Quantity = 20 },
                new() { Name = "Iron Ingot", Quantity = 65, IsOutput = true },
            }
        },
        new()
        {
            Name = "Iron Alloy Ingot",
            Ingredients = new List<Recipes.IngredientRow>
            {
                new() { Name = "Iron Ore", Quantity = 20 },
                new() { Name = "Copper Ore", Quantity = 20 },
                new() { Name = "Iron Ingot", Quantity = 50, IsOutput = true },
            }
        }
    };

    public static readonly Recipes Recipes = new(null)
    {
        All = recipes,
        Current = recipes.First()
    };
}