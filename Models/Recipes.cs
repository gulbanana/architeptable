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

    private RecipeModel current = default!;
    public RecipeModel Current
    {
        get => current;
        set => RaiseAndSetIfChanged(ref current, value);
    }

    internal override async Task LoadAsync(Data.EntityContext context)
    {
        var self = this;

        var allParts = await context.Parts
            .Select(p => new PartModel(p.ID, p.Name))
            .ToListAsync();

        var recipesWithIngredients = from r in context.Recipes
                                     from i in r.Ingredients
                                     let p = i.Part
                                     let im = new IngredientRow { Owner = self, ID = i.ID, CurrentPart = new(p.ID, p.Name), AllParts = allParts, Quantity = i.Quantity, IsOutput = i.IsOutput }
                                     select new { r.Name, im };

        All = from r in await recipesWithIngredients.ToListAsync()
              group r.im by r.Name into g
              select new RecipeModel(g.Key, g);

        current = All.First();
    }

    public class IngredientRow
    {
        public TabModelBase? Owner { get; init; }
        public int ID { get; init; }

        public required IEnumerable<PartModel> AllParts { get; init; }

        private PartModel? currentPart;
        public required PartModel CurrentPart
        {
            get => currentPart!;
            set
            {
                if (currentPart != value)
                {
                    currentPart = value;
                    Owner?.Save(c => c.Ingredients.Find(ID)!.PartID = value.ID);
                }                
            }
        }

        private double quantity;
        public required double Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                Owner?.Save(c => c.Ingredients.Find(ID)!.Quantity = value);
            }
        }

        private bool isOutput;
        public bool IsOutput
        {
            get => isOutput;
            set
            {
                isOutput = value;
                Owner?.Save(c => c.Ingredients.Find(ID)!.IsOutput = value);
            }
        }
    }

    public record RecipeModel(string Name, IEnumerable<IngredientRow> Ingredients);
    public record PartModel(int ID, string Name);
}