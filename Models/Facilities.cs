using Architeptable.Data;
using Avalonia.Media;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public partial class Facilities : TabModelBase
{
    public override string Header => "Facilities";
    public IReadOnlyList<OptionModel<int>> PartOptions { get; set; } = Array.Empty<OptionModel<int>>();
    public IReadOnlyList<OptionModel<int>> RecipeOptions { get; set; } = Array.Empty<OptionModel<int>>();

    public Facilities(Shell? owner) : base(owner) { }

    private IReadOnlyList<FacilityModel> all = Array.Empty<FacilityModel>();
    public IReadOnlyList<FacilityModel> All
    {
        get => all;
        set => RaiseAndSetIfChanged(ref all, value);
    }

    private FacilityModel? current = default;
    public FacilityModel? Current
    {
        get => current;
        set => RaiseAndSetIfChanged(ref current, value);
    }

    internal override async Task LoadAsync(EntityContext context)
    {
        var self = this;

        var allInputs = await context.FacilityParts
            .OrderBy(i => i.Part.Name)
            .Select(s => new InputOutputModel 
            { 
                Owner = self, 
                ID = s.ID,
                Quantity = s.Quantity,
                Part = new(s.Part.ID, s.Part.Name), 
                Source = new(s.SourceID, s.Source != null ? s.Source.Name : "Local"),
                Destination = new(s.DestinationID, s.Destination != null ? s.Destination.Name : "Local")
            })
            .ToListAsync();

        var allFacilities = await context.Facilities
            .Include(f => f.Processes.OrderBy(p => p.Recipe.Name))
            .ThenInclude(p => p.Recipe)
            .OrderBy(f => f.Name)
            .ToListAsync();

        var last = Current;

        All = allFacilities
            .Select(f =>
            {
                var inputs = allInputs
                    .Where(i => i.Destination.ID == f.ID)
                    .ToList();

                var outputs = allInputs
                    .Where(i => i.Source.ID == f.ID)
                    .ToList();

                var processes = f.Processes
                    .Select(p => new ProcessModel { Owner = self, ID = p.ID, Recipe = new(p.Recipe.ID, p.Recipe.Name), Machines = p.Machines, Overclock = p.Overclock })
                    .ToList();

                var otherFacilities = allFacilities
                    .Where(of => of.ID != f.ID)
                    .Select(of => new OptionModel<int?>(of.ID, of.Name))
                    .Prepend(new OptionModel<int?>(null, "Local"))
                    .ToList();

                return new FacilityModel
                {
                    Owner = self,
                    ID = f.ID,
                    Name = f.Name,
                    Inputs = inputs,
                    Outputs = outputs,
                    Processes = processes,
                    FacilityOptions = otherFacilities
                };
            })
            .ToList();

        if (last != null)
        {
            Current = All.SingleOrDefault(f => f.ID == last.ID) ?? All.FirstOrDefault();
        }
        else
        {
            Current = All.FirstOrDefault();
        }

        PartOptions = await context.Parts
            .OrderBy(r => r.Name)
            .Select(p => new OptionModel<int>(p.ID, p.Name))
            .ToListAsync();

        RecipeOptions = await context.Recipes
            .OrderBy(r => r.Name)
            .Select(p => new OptionModel<int>(p.ID, p.Name))
            .ToListAsync();

        foreach (var facility in All)
        {
            await CalculateAsync(context, facility);
        }
    }

    internal override async Task CalculateAsync(EntityContext context)
    {
        if (Current != null)
        {
            await CalculateAsync(context, Current);
        }
    }

    private async Task CalculateAsync(EntityContext context, FacilityModel facility)
    {
        var production = await (from p in context.Processes
                                where facility.Processes.Select(p => p.ID).Contains(p.ID)
                                let r = p.Recipe
                                from i in r.Ingredients
                                select new { part = i.Part.ID, volume = p.Machines * p.Overclock * i.Quantity * (i.IsOutput ? 1 : -1) }).ToListAsync();

        var usage = production.GroupBy(p => p.part, p => p.volume).ToDictionary(p => p.Key, p => p.Sum());

        foreach (var input in facility.Inputs)
        {
            var partConsumption = -usage.GetValueOrDefault(input.Part.ID, 0);
            input.Consumption = new DerivedModel<double>(partConsumption,
                partConsumption > input.Quantity ? Brushes.Red :
                partConsumption < input.Quantity ? Brushes.LightGreen : 
                Brushes.White);
        }

        foreach (var output in facility.Outputs)
        {
            var partProduction = usage.GetValueOrDefault(output.Part.ID, 0);
            output.Production = new DerivedModel<double>(partProduction,
                partProduction < output.Quantity ? Brushes.Red :
                partProduction > output.Quantity ? Brushes.LightGreen :
                Brushes.White);
        }
    }

    public class FacilityModel : EntityModelBase<Facility>
    {
        public required IReadOnlyList<InputOutputModel> Inputs { get; init; }
        public required IReadOnlyList<InputOutputModel> Outputs { get; init; }
        public required IReadOnlyList<ProcessModel> Processes { get; init; }
        public required IReadOnlyList<OptionModel<int?>> FacilityOptions { get; init; }

        private string? name;
        public required string Name
        {
            get => name!;
            set => SaveIfChanged(ref name, value);
        }
    }

    public class InputOutputModel : EntityModelBase<FacilityPart>
    {
        private OptionModel<int>? part;
        public required OptionModel<int> Part
        {
            get => part!;
            set => SaveIfChanged(ref part, value, e => e.PartID = value.ID);
        }

        private OptionModel<int?>? source;
        public required OptionModel<int?> Source
        {
            get => source!;
            set => SaveIfChanged(ref source, value, e => e.SourceID = value.ID, reload: true);
        }

        private OptionModel<int?>? destination;
        public required OptionModel<int?> Destination
        {
            get => destination!;
            set => SaveIfChanged(ref destination, value, e => e.DestinationID = value.ID, reload: true);
        }

        private double quantity;
        public required double Quantity
        {
            get => quantity;
            set => SaveIfChanged(ref quantity, value);
        }

        private DerivedModel<double> production = new(0.0, Brushes.White);
        public DerivedModel<double> Production
        {
            get => production!;
            set => RaiseAndSetIfChanged(ref production, value);
        }

        private DerivedModel<double> consumption =  new(0.0, Brushes.White);
        public DerivedModel<double> Consumption
        {
            get => consumption;
            set => RaiseAndSetIfChanged(ref consumption, value);
        }
    }

    public class ProcessModel : EntityModelBase<FacilityRecipe>
    {
        private OptionModel<int>? currentRecipe;
        public required OptionModel<int> Recipe
        {
            get => currentRecipe!;
            set => SaveIfChanged(ref currentRecipe, value, e => e.RecipeID = value.ID);
        }

        private int machines;
        public required int Machines
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
