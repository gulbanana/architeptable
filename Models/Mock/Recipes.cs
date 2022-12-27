using System.Collections.Generic;
using System.Linq;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly List<Recipes.Recipe> _Recipes = new()
    {
        new()
        {
            Name = "Pure Iron Ingot",
            Ingredients = new List<Recipes.Ingredient>
            {
                new("Iron Ore", 35, false),
                new("Water", 20, false),
                new("Iron Ingot", 65, true)
            }
        },
        new()
        {
            Name = "Iron Alloy Ingot",
            Ingredients = new List<Recipes.Ingredient>
            {
                new("Iron Ore", 20, false),
                new("Copper Ore", 20, false),
                new("Iron Ingot", 50, true)
            }
        }
    };

    public static readonly Recipes Recipes = new Recipes()
    {
        All = _Recipes,
        Current = _Recipes.First()
    };
}