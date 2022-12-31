namespace Architeptable.Data;

class Ingredient
{
    public int ID { get; set; }
    public double Quantity { get; set; }
    public bool IsOutput { get; set; }

    public required int RecipeID { get; set; }

    public required int PartID { get; set; }
    public Part Part { get; set; } = default!;
}
