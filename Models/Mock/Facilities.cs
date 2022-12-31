using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly List<OptionModel> facilityParts = new()
    {
        new(0, "Uranium Ore"),
        new(0, "Silica"),
        new(0, "Sulfur"),
        new(0, "Quickwire"),
        new(0, "Encased Uranium Cell"),
    };


    private static readonly List<OptionModel> facilityRecipes = new()
    {
        new(0, "Infused Uranium Cell")
    };

    private static readonly List<Facilities.FacilityModel> facilityFacilities = new()
    {
        new Facilities.FacilityModel
        {
            Name = "Nuclear Power Plant",
            Inputs = new Facilities.IngredientModel[]
            {
                new() { Part = facilityParts[0], Quantity = 1000 }
            },
            Outputs = Enumerable.Empty<Facilities.IngredientModel>(),
            Processes = new Facilities.ProcessModel[]
            {
               new() { Recipe = facilityRecipes[0], Machines = 10, Overclock = 1.0 }
            }
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