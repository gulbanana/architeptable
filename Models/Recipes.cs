using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Recipes : TabModelBase
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

    public override string Header => "Recipes";

    public IEnumerable<Recipe> All { get; set; } = Enumerable.Empty<Recipe>();

    private Recipe current = default!;
    public Recipe Current
    {
        get => current;
        set => RaiseAndSetIfChanged(ref current, value);
    }

    internal override async Task LoadAsync(Data.EntityContext context)
    {
        var recipesWithIngredients = await context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .ToListAsync();

        All = recipesWithIngredients.Select(r => new Recipe
        {
            Name = r.Name,
            Ingredients = r.Ingredients.Select(ri => new Ingredient
            {
                Name = ri.Ingredient.Name,
                Quantity = ri.Quantity,
                IsOutput = ri.IsOutput
            }).ToList()
        }).ToList();

        current = All.First();
    }
}