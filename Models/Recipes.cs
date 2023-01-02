using Architeptable.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Recipes : TabModelBase
{
    public override string Header => "Recipes";
    public IReadOnlyList<RecipeModel> All { get; set; } = Array.Empty<RecipeModel>();
    public IReadOnlyList<OptionModel> PartOptions { get; set; } = Array.Empty<OptionModel>();

    public Recipes(Shell? owner) : base(owner) { }

    private RecipeModel? current;
    public RecipeModel? Current
    {
        get => current;
        set => RaiseAndSetIfChanged(ref current, value);
    }

    internal override async Task LoadAsync(EntityContext context)
    {
        var self = this;

        var recipesWithIngredients = from r in context.Recipes
                                     from i in r.Ingredients
                                     let p = i.Part
                                     let im = new IngredientModel { Owner = self, ID = i.ID, Part = new(p.ID, p.Name), Quantity = i.Quantity, IsOutput = i.IsOutput }
                                     select new { r.ID, r.Name, im };

        var last = Current;

        All = (from r in await recipesWithIngredients.ToListAsync()
               orderby r.im.IsOutput, r.Name
               group r.im by (r.ID, r.Name) into g
               orderby g.Key.Name
               select new RecipeModel { Owner = self, ID = g.Key.ID, Name = g.Key.Name, Ingredients = g.ToList() }).ToList();

        if (last != null)
        {
            Current = All.SingleOrDefault(f => f.ID == last.ID) ?? All.FirstOrDefault();
        }
        else
        {
            Current = All.FirstOrDefault();
        }

        PartOptions = await context.Parts
            .OrderBy(p => p.Name)
            .Select(p => new OptionModel<int>(p.ID, p.Name))
            .ToListAsync();
    }

    internal override Task CalculateAsync(EntityContext context)
    {
        return Task.CompletedTask;
    }

    public class RecipeModel : EntityModelBase<Recipe>
    {
        private string? name;
        public required string Name
        {
            get => name!;
            set => SaveIfChanged(ref name, value);
        }

        public required IReadOnlyList<IngredientModel> Ingredients { get; init; }
    }

    public class IngredientModel : EntityModelBase<RecipePart>
    {
        private OptionModel<int>? part;
        public required OptionModel<int> Part
        {
            get => part!;
            set => SaveIfChanged(ref part, value, e => e.PartID = value.ID);
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