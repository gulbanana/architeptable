using Architeptable.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public partial class Facilities : TabModelBase
{
    public override string Header => "Facilities";
    public IEnumerable<FacilityModel> All { get; set; } = Enumerable.Empty<FacilityModel>();
    public IEnumerable<OptionModel> PartOptions { get; set; } = Enumerable.Empty<OptionModel>();
    public IEnumerable<OptionModel> RecipeOptions { get; set; } = Enumerable.Empty<OptionModel>();

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

        var facilitiesWithPlans = await context.Facilities
            .Include(f => f.Processes)
            .ThenInclude(p => p.Recipe)
            .Include(f => f.Ingredients)
            .ThenInclude(i => i.Part)
            .ToListAsync();

        var allFacilities = from f in facilitiesWithPlans
                            let inputs = f.Ingredients
                                .Where(s => !s.IsOutput)
                                .Select(s => new IngredientModel { Owner = self, ID = s.ID, Part = new OptionModel(s.Part.ID, s.Part.Name), Quantity = s.Quantity })
                                .OrderBy(i => i.Part.Name)
                                .ToList()
                            let outputs = f.Ingredients
                                .Where(s => s.IsOutput)
                                .Select(s => new IngredientModel { Owner = self, ID = s.ID, Part = new OptionModel(s.Part.ID, s.Part.Name), Quantity = s.Quantity })
                                .OrderBy(i => i.Part.Name)
                                .ToList()
                            let processes = f.Processes.Select(p => new ProcessModel { Owner = self, ID = p.ID, Recipe = new OptionModel(p.Recipe.ID, p.Recipe.Name), Machines = p.Machines, Overclock = p.Overclock })
                            orderby f.Name
                            select new FacilityModel 
                            { 
                                Owner = self, 
                                ID = f.ID, 
                                Name = f.Name, 
                                Inputs = inputs, 
                                Outputs = outputs,
                                Processes = processes
                            };

        All = allFacilities.ToList();

        Current = All.FirstOrDefault()!;

        PartOptions = await context.Parts
            .OrderBy(r => r.Name)
            .Select(p => new OptionModel(p.ID, p.Name))
            .ToListAsync();

        RecipeOptions = await context.Recipes
            .OrderBy(r => r.Name)
            .Select(p => new OptionModel(p.ID, p.Name))
            .ToListAsync();
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
        private OptionModel? currentPart;
        public required OptionModel Part
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

    public class ProcessModel : EntityModelBase<Process>
    {
        private OptionModel? currentRecipe;
        public required OptionModel Recipe
        {
            get => currentRecipe!;
            set => RaiseAndSetIfChanged(ref currentRecipe, value);
        }

        private double machines;
        public required double Machines
        {
            get => machines;
            set => SaveIfChanged(ref machines, value);
        }

        private double overclock;
        public required double Overclock
        {
            get => overclock;
            set => SaveIfChanged(ref overclock, value);
        }

        public string OverclockText
        {
            get => Overclock.ToString("0.0");
            set => Overclock = double.Parse(value);
        }
    }
}
