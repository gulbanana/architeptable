using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Recipes : TabModelBase
{
    public override string Header => "Recipes";
    public IEnumerable<Recipe> All { get; set; } = Enumerable.Empty<Recipe>();

    public Recipes(Shell? owner) : base(owner) { }

    private Recipe current = default!;
    public Recipe Current
    {
        get => current;
        set => RaiseAndSetIfChanged(ref current, value);
    }

    internal override async Task LoadAsync(Data.EntityContext context)
    {
        var self = this;
        var recipesWithIngredients = from r in context.Recipes
                                     from ri in r.Ingredients
                                     let i = ri.Ingredient
                                     let im = new IngredientRow { Owner = self, ID = ri.ID, Name = i.Name, Quantity = ri.Quantity, IsOutput = ri.IsOutput }
                                     select new { r.Name, im };

        All = from r in await recipesWithIngredients.ToListAsync()
              group r.im by r.Name into g
              select new Recipe { Name = g.Key, Ingredients = g };

        current = All.First();
    }

    public class IngredientRow
    {
        public TabModelBase? Owner { get; init; }
        public int ID { get; init; }

        public required string Name { get; init; }

        private double quantity;
        public required double Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                Owner?.Save(c => c.RecipeIngredients.Find(ID)!.Quantity = value);
            }
        }

        private bool isOutput;
        public bool IsOutput
        {
            get => isOutput;
            set
            {
                isOutput = value;
                Owner?.Save(c => c.RecipeIngredients.Find(ID)!.IsOutput = value);
            }
        }
    }

    public class Recipe
    {
        public required string Name { get; init; }
        public required IEnumerable<IngredientRow> Ingredients { get; init; }
    }
}