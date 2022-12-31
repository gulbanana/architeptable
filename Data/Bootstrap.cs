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
            new() { ID = 2, Name = "Silica" },
        });
        
        context.RecipeIngredients.AddRange(new Ingredient[]
        {
            new() { ID = 1, RecipeID = 1, PartID = 1, Quantity = 25 },
            new() { ID = 2, RecipeID = 1, PartID = 2, Quantity = 15 },
            new() { ID = 3, RecipeID = 1, PartID = 3, Quantity = 75 },
            new() { ID = 4, RecipeID = 1, PartID = 4, Quantity = 25 },
            new() { ID = 5, RecipeID = 1, PartID = 5, Quantity = 20, IsOutput = true },
            new() { ID = 6, RecipeID = 2, PartID = 6, Quantity = 22.5 },
            new() { ID = 7, RecipeID = 2, PartID = 2, Quantity = 37.5, IsOutput = true },
        });

        context.Facilities.AddRange(new Facility[]
        {
            new() { Name = "EMS Corner" },
            new() { Name = "Nuclear Power Plant" }
        });

        context.SaveChanges();
    }
}