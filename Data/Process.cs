namespace Architeptable.Data;

public class Process
{
    public int ID { get; set; }
    public double Machines { get; set; }
    public double Overclock { get; set; }

    public required int FacilityID { get; set; }
    public Facility Facility { get; set; } = default!;

    public required int RecipeID { get; set; }
    public Recipe Recipe { get; set; } = default!;
}
