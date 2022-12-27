namespace Architeptable.Data;

static class Bootstrap
{
    public static void InitDB(EntityContext context)
    {
        context.Ingredients.AddRange(new Ingredient[]
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
        
        context.RecipeIngredients.AddRange(new RecipeIngredient[]
        {
            new() { ID = 1, RecipeID = 1, IngredientID = 1, Quantity = 25 },
            new() { ID = 2, RecipeID = 1, IngredientID = 2, Quantity = 15 },
            new() { ID = 3, RecipeID = 1, IngredientID = 3, Quantity = 75 },
            new() { ID = 4, RecipeID = 1, IngredientID = 4, Quantity = 25 },
            new() { ID = 5, RecipeID = 1, IngredientID = 5, Quantity = 20, IsOutput = true },
            new() { ID = 6, RecipeID = 2, IngredientID = 6, Quantity = 22.5 },
            new() { ID = 7, RecipeID = 2, IngredientID = 2, Quantity = 37.5, IsOutput = true },
        });

        context.SaveChanges();
    }
}