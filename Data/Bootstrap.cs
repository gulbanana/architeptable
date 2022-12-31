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
            new() { ID = 1, Name = "EMS Corner", Processes = new HashSet<Recipe>()
            {
                context.Recipes.Find(1)!
            } },
            new() { ID = 2, Name = "Nuclear Power Plant", Processes = new HashSet<Recipe>()
            {
                context.Recipes.Find(2)!
            } }
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

        context.SaveChanges();
    }
}