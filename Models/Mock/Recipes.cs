using System.Collections.Generic;
using System.Linq;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly List<OptionModel<int>> recipeParts = new()
    {
        new(0, "Water"),
        new(0, "Iron Ore"),
        new(0, "Copper Ore"),
        new(0, "Iron Ingot")
    };

    private static readonly List<Recipes.RecipeModel> recipes = new()
    {
        new()
        {
            Name = "Pure Iron Ingot",
            Ingredients = new List<Recipes.IngredientModel>
            {
                new() { Part = recipeParts[1], Quantity = 35 },
                new() { Part = recipeParts[0], Quantity = 20 },
                new() { Part = recipeParts[3], Quantity = 65, IsOutput = true },
            }
        },
        new()
        {
            Name = "Iron Alloy Ingot",
            Ingredients = new List<Recipes.IngredientModel>
            {
                new() { Part = recipeParts[1], Quantity = 20 },
                new() { Part = recipeParts[2], Quantity = 20 },
                new() { Part = recipeParts[3], Quantity = 50, IsOutput = true },
            }
        }
    };

    public static readonly Recipes Recipes = new(null)
    {
        PartOptions = recipeParts,
        All = recipes,
        Current = recipes.First()
    };
}