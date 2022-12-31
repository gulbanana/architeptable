using System.Collections.Generic;
using System.Linq;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly List<Recipes.PartModel> recipeParts = new()
    {
        new(0, "Water"),
        new(0, "Iron Ore"),
        new(0, "Copper Ore"),
        new(0, "Iron Ingot")
    };

    private static readonly List<Recipes.RecipeModel> recipes = new()
    {
        new("Pure Iron Ingot", new List<Recipes.IngredientModel>
        {
            new() { AllParts = recipeParts, CurrentPart = recipeParts[1], Quantity = 35 },
            new() { AllParts = recipeParts, CurrentPart = recipeParts[0], Quantity = 20 },
            new() { AllParts = recipeParts, CurrentPart = recipeParts[3], Quantity = 65, IsOutput = true },
        }),
        new("Iron Alloy Ingot", new List<Recipes.IngredientModel>
        {
            new() { AllParts = recipeParts, CurrentPart = recipeParts[1], Quantity = 20 },
            new() { AllParts = recipeParts, CurrentPart = recipeParts[2], Quantity = 20 },
            new() { AllParts = recipeParts, CurrentPart = recipeParts[3], Quantity = 50, IsOutput = true },
        })
    };

    public static readonly Recipes Recipes = new(null)
    {
        All = recipes,
        Current = recipes.First()
    };
}