using System.Collections.Generic;

namespace Architeptable;

record Ingredient(int ID, string Name, byte[]? Icon = null);
record Recipe(int ID, string Name);
record RecipeIngredient(int RecipeID, int IngredientID, double Quantity, bool IsOutput = false);

static class Data
{
    public static readonly List<Ingredient> Ingredients = new()
    {
        new(1, "Uranium"),
        new(2, "Silica"),
        new(3, "Sulfur"),
        new(4, "Quickwire"),
        new(5, "Encased Uranium Cell"),
        new(6, "Raw Quartz"),
    };

    public static readonly List<Recipe> Recipes = new()
    {
        new(1, "Infused Uranium Cell"),
        new(2, "Silica")
    };

    public static readonly List<RecipeIngredient> RecipeIngredients = new()
    {
        new(1, 1, 25),
        new(1, 2, 15),
        new(1, 3, 75),
        new(1, 4, 25),
        new(1, 5, 20, true),
        new(2, 6, 22.5),
        new(2, 2, 37.5)
    };
}