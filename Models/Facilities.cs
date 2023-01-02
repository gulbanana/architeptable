using Architeptable.Data;
using Avalonia.Media;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            .Include(f => f.InputOutputs)
            .ThenInclude(i => i.Part)
            .ToListAsync();

        var allFacilities = from f in facilitiesWithPlans
                            let inputs = f.InputOutputs
                                .Where(s => !s.IsOutput)
                                .Select(s => new InputOutputModel { Owner = self, ID = s.ID, Part = new OptionModel(s.Part.ID, s.Part.Name), Quantity = s.Quantity })
                                .OrderBy(i => i.Part.Name)
                                .ToList()
                            let outputs = f.InputOutputs
                                .Where(s => s.IsOutput)
                                .Select(s => new InputOutputModel { Owner = self, ID = s.ID, Part = new OptionModel(s.Part.ID, s.Part.Name), Quantity = s.Quantity })
                                .OrderBy(i => i.Part.Name)
                                .ToList()
                            let processes = f.Processes
                                .Select(p => new ProcessModel { Owner = self, ID = p.ID, Recipe = new OptionModel(p.Recipe.ID, p.Recipe.Name), Machines = p.Machines, Overclock = p.Overclock })
                                .OrderBy(p => p.Recipe.Name)
                                .ToList()
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

    internal override async Task CalculateAsync(EntityContext context)
    {
        var production = await (from p in context.Processes
                                where current.Processes.Select(p => p.ID).Contains(p.ID)
                                let r = p.Recipe
                                from i in r.Ingredients
                                select new { part = i.Part.ID, volume = p.Machines * p.Overclock * i.Quantity * (i.IsOutput ? 1 : -1) }).ToListAsync();

        var usage = production.GroupBy(p => p.part, p => p.volume).ToDictionary(p => p.Key, p => p.Sum());

        foreach (var input in current.Inputs)
        {
            input.ActualQuantity = -usage.GetValueOrDefault(input.Part.ID, 0);
            input.Highlight = input.ActualQuantity <= input.Quantity ? Brushes.White : Brushes.Red;
        }

        foreach (var output in current.Outputs)
        {
            output.ActualQuantity = usage.GetValueOrDefault(output.Part.ID, 0);
            output.Highlight = output.ActualQuantity >= output.Quantity ? Brushes.White : Brushes.Red;
        }
    }

    public class FacilityModel : EntityModelBase<Facility>
    {
        public required IEnumerable<InputOutputModel> Inputs { get; init; }
        public required IEnumerable<InputOutputModel> Outputs { get; init; }
        public required IEnumerable<ProcessModel> Processes { get; init; }

        private string? name;
        public required string Name
        {
            get => name!;
            set => SaveIfChanged(ref name, value);
        }
    }

    public class InputOutputModel : EntityModelBase<FacilityPart>
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

        private double actualQuantity;
        public double ActualQuantity
        {
            get => actualQuantity;
            set => RaiseAndSetIfChanged(ref actualQuantity, value);
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
