namespace Architeptable.Data;

public class FacilityPart
{
    public int ID { get; set; }
    public double Quantity { get; set; }
    public bool IsOutput { get; set; }

    public required int FacilityID { get; set; }
    public Facility Facility { get; set; } = default!;

    public required int PartID { get; set; }
    public Part Part { get; set; } = default!;
}
