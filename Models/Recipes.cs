using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        var recipesWithIngredients = from r in context.Recipes
                                     from ri in r.Ingredients
                                     let i = ri.Ingredient
                                     let im = new Ingredient { Name = i.Name, Quantity = ri.Quantity, IsOutput = ri.IsOutput }
                                     select new { r.Name, im };

        All = from r in await recipesWithIngredients.ToListAsync()
              group r.im by r.Name into g
              select new Recipe { Name = g.Key, Ingredients = g };

        current = All.First();
    }
}