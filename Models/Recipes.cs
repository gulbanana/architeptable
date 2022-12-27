using System.Collections.Generic;

namespace Architeptable.Models;

public class Recipes : ModelBase
{
    public class Ingredient
    {
        public required string Name { get; init; }
        public required double Quantity { get; init; }
        public bool IsOutput { get; init; }
    }

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