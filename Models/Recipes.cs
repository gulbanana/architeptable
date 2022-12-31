using Architeptable.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Recipes : TabModelBase
{
    public override string Header => "Recipes";
    public IEnumerable<RecipeModel> All { get; set; } = Enumerable.Empty<RecipeModel>();

    public Recipes(Shell? owner) : base(owner) { }

    private RecipeModel? current;
    public RecipeModel Current
    {
        get => current!;
        set => RaiseAndSetIfChanged(ref current, value);
    }

    internal override async Task LoadAsync(EntityContext context)
    {
        var self = this;

        var allParts = await context.Parts
            .Select(p => new PartModel(p.ID, p.Name))
            .ToListAsync();

        var recipesWithIngredients = from r in context.Recipes
                                     from i in r.Ingredients
                                     let p = i.Part
                                     let im = new IngredientModel { Owner = self, ID = i.ID, CurrentPart = new(p.ID, p.Name), AllParts = allParts, Quantity = i.Quantity, IsOutput = i.IsOutput }
                                     select new { r.ID, r.Name, im };

        All = (from r in await recipesWithIngredients.ToListAsync()
               group r.im by (r.ID, r.Name) into g
               select new RecipeModel { Owner = self, ID = g.Key.ID, Name = g.Key.Name, Ingredients = g }).ToList();

        Current = All.First();
    }

    public record PartModel(int ID, string Name);

    public class RecipeModel : EntityModelBase<Recipe>
    {
        private string? name;
        public required string Name
        {
            get => name!;
            set => SaveIfChanged(ref name, value);
        }

        public required IEnumerable<IngredientModel> Ingredients { get; init; }
    }

    public class IngredientModel : EntityModelBase<Ingredient>
    {
        public required IEnumerable<PartModel> AllParts { get; init; }

        private PartModel? currentPart;
        public required PartModel CurrentPart
        {
            get => currentPart!;
            set => SaveIfChanged(ref currentPart, value, e => e.PartID = value.ID);
        }

        private double quantity;
        public required double Quantity
        {
            get => quantity;
            set => SaveIfChanged(ref quantity, value);
        }

        private bool isOutput;
        public bool IsOutput
        {
            get => isOutput;
            set => SaveIfChanged(ref isOutput, value);
        }
    }
}