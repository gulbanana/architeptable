using System.Collections.Generic;
using System.Diagnostics;

namespace Architeptable.Data;

static class Bootstrap
{
    public static void InitDB(EntityContext context)
    {
        context.Parts.AddRange(new Part[]
        {
            new() { ID = 1, Name = "Uranium" },
            new() { ID = 2, Name = "Silica" },
            new() { ID = 3, Name = "Sulfur" },
            new() { ID = 4, Name = "Quickwire" },
            new() { ID = 5, Name = "Encased Uranium Cell" },
            new() { ID = 6, Name = "Raw Quartz" },
        });

        context.Recipes.AddRange(new Recipe[]
        {
            new() { ID = 1, Name = "Infused Uranium Cell" },
            new() { ID = 2, Name = "Cheap Silica" },
        });

        context.Facilities.AddRange(new Facility[]
        {
            new() { ID = 1, Name = "EMS Corner" },
            new() { ID = 2, Name = "Nuclear Power Plant" }
        });

        context.Ingredients.AddRange(new Ingredient[]
        {
            new() { RecipeID = 1, PartID = 1, Quantity = 25 },
            new() { RecipeID = 1, PartID = 2, Quantity = 15 },
            new() { RecipeID = 1, PartID = 3, Quantity = 75 },
            new() { RecipeID = 1, PartID = 4, Quantity = 25 },
            new() { RecipeID = 1, PartID = 5, Quantity = 20, IsOutput = true },
            new() { RecipeID = 2, PartID = 6, Quantity = 22.5 },
            new() { RecipeID = 2, PartID = 2, Quantity = 37.5, IsOutput = true },
            new() { RecipeID = 2, PartID = 2, Quantity = 37.5, IsOutput = true },
            new() { FacilityID = 1, PartID = 6, Quantity = 1800 },
            new() { FacilityID = 1, PartID = 2, Quantity = 2000, IsOutput = true },
        });

        context.Processes.AddRange(new Process[]
        {
            new() { FacilityID = 1, RecipeID = 2, Machines = 1, Overclock = 1.0 },
            new() { FacilityID = 2, RecipeID = 1, Machines = 1, Overclock = 1.0 }
        });

        context.SaveChanges();
    }
}