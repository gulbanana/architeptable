using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly List<OptionModel<int>> facilityParts = new()
    {
        new(0, "Uranium Ore"),
        new(0, "Silica"),
        new(0, "Sulfur"),
        new(0, "Quickwire"),
        new(0, "Encased Uranium Cell"),
    };

    private static readonly List<OptionModel<int>> facilityRecipes = new()
    {
        new(0, "Infused Uranium Cell")
    };

    private static readonly List<OptionModel<int?>> facilityOtherFacilities = new()
    {
        new(null, "Local"),
        new(null, "Nuclear Fuel Plant"),
        new(null, "Nuclear Power Plant")
    };

    private static readonly List<Facilities.FacilityModel> facilityFacilities = new()
    {
        new Facilities.FacilityModel
        {
            Name = "Nuclear Fuel Plant",
            Inputs = new Facilities.InputOutputModel[]
            {
                new() { Source = facilityOtherFacilities[0], Destination = facilityOtherFacilities[1], Part = facilityParts[0], Quantity = 600, Consumption = new(900, Brushes.Red) }
            },
            Outputs = new Facilities.InputOutputModel[]
            {
                new() { Source = facilityOtherFacilities[1], Destination = facilityOtherFacilities[2], Part = facilityParts[4], Quantity = 100, Production = new(100, Brushes.White) }
            },
            Processes = new Facilities.ProcessModel[]
            {
               new() { Recipe = facilityRecipes[0], Machines = 10, Overclock = 1.0 }
            },
            FacilityOptions = facilityOtherFacilities
        }
    };

    public static readonly Facilities Facilities = new(null)
    {
        All = facilityFacilities,
        Current = facilityFacilities[0],
        PartOptions = facilityParts,
        RecipeOptions = facilityRecipes
    };
}