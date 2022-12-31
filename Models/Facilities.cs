using Architeptable.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Architeptable.Models.Recipes;

namespace Architeptable.Models;

public class Facilities : TabModelBase
{
    public override string Header => "Facilities";
    public IEnumerable<FacilityModel> All { get; set; } = Enumerable.Empty<FacilityModel>();

    public Facilities(Shell? owner) : base(owner) { }

    private FacilityModel current = default!;
    public FacilityModel Current
    {
        get => current;
        set => RaiseAndSetIfChanged(ref current, value);
    }

    internal override async Task LoadAsync(EntityContext context)
    {
        var self = this;

        var allParts = await context.Parts
            .Select(p => new PartModel(p.ID, p.Name))
            .ToListAsync();

        var allRecipes = await context.Recipes
            .Select(p => new RecipeModel(p.ID, p.Name))
            .ToListAsync();

        var facilitiesWithSpecifications = await context.Facilities
            .Include(f => f.Processes)
            .Include(f => f.Specifications)
            .ThenInclude(s => s.Part)
            .ToListAsync();

        var allFacilities = from f in facilitiesWithSpecifications
                            let inputs = f.Specifications
                                .Where(s => !s.IsOutput)
                                .Select(s => new IngredientModel { Owner = self, ID = s.ID, AllParts = allParts, CurrentPart = new PartModel(s.Part.ID, s.Part.Name), Quantity = s.Quantity })
                                .ToList()
                            let outputs = f.Specifications
                                .Where(s => s.IsOutput)
                                .Select(s => new IngredientModel { Owner = self, ID = s.ID, AllParts = allParts, CurrentPart = new PartModel(s.Part.ID, s.Part.Name), Quantity = s.Quantity })
                                .ToList()
                            select new FacilityModel 
                            { 
                                Owner = self, 
                                ID = f.ID, 
                                Name = f.Name, 
                                Inputs = inputs, 
                                Outputs = outputs,
                                Processes = f.Processes.Select(r => new ProcessModel { AllRecipes = allRecipes, CurrentRecipe = new RecipeModel(r.ID, r.Name) })
                            };

        All = allFacilities.ToList();
        Current = All.First();
    }

    public class FacilityModel : EntityModelBase<Facility>
    {
        public required IEnumerable<IngredientModel> Inputs { get; init; }
        public required IEnumerable<IngredientModel> Outputs { get; init; }
        public required IEnumerable<ProcessModel> Processes { get; init; }

        private string? name;
        public required string Name
        {
            get => name!;
            set => SaveIfChanged(ref name, value);
        }
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
    }

    public class ProcessModel : ModelBase
    {
        public required IEnumerable<RecipeModel> AllRecipes { get; init; }

        private RecipeModel? currentRecipe;
        public required RecipeModel CurrentRecipe
        {
            get => currentRecipe!;
            set => RaiseAndSetIfChanged(ref currentRecipe, value);
        }
    }

    public record PartModel(int ID, string Name);

    public record RecipeModel(int ID, string Name);
}
