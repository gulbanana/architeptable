namespace Architeptable.Data;

class RecipeIngredient
{
    public int ID { get; set; }
    public double Quantity { get; set; }
    public bool IsOutput { get; set; }

    public required int RecipeID { get; set; }

    public required int IngredientID { get; set; }
    public Ingredient Ingredient { get; set; } = default!;
}
