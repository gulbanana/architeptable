using System.Collections.Generic;

namespace Architeptable.Models;

public class Recipes : ModelBase
{
    public record Ingredient(string Name, double Quantity, bool IsOutput);

    public class Recipe
    {
        public required string Name { get; init; }
        public required IEnumerable<Ingredient> Ingredients { get; init; }
    }

    public string Header => "Recipes";

    public required IEnumerable<Recipe> All { get; init; }

    private Recipe current = default!;
    public required Recipe Current
    {
        get => current;
        set => RaiseAndSetIfChanged(ref current, value);
    }
}