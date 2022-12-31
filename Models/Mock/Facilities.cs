using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly List<OptionModel> facilityParts = new()
    {
        new(0, "Uranium")
    };


    private static readonly List<OptionModel> facilityRecipes = new()
    {
        new(0, "Nuclear Power")
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
               new() { Recipe = facilityRecipes[0] }
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