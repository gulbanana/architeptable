namespace Architeptable.Data;

public class FacilityRecipe
{
    public int ID { get; set; }
    public int Machines { get; set; } = 1;
    public double Overclock { get; set; } = 1.0;

    public Facility Facility { get; set; } = default!;
    public required int FacilityID { get; set; }

    public Recipe Recipe { get; set; } = default!;
    public required int RecipeID { get; set; }    
}
