using System.Collections.Generic;
using ReactiveUI;

namespace Architeptable.Models;

class Recipes : ReactiveObject
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
        set => this.RaiseAndSetIfChanged(ref current, value);
    }
}