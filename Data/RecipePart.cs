namespace Architeptable.Data;

public class RecipePart
{
    public int ID { get; set; }
    public double Quantity { get; set; }
    public bool IsOutput { get; set; }

    public Recipe Recipe { get; set; } = default!;
    public required int RecipeID { get; set; }

    public Part Part { get; set; } = default!;
    public required int PartID { get; set; }    
}
