using System.ComponentModel.DataAnnotations.Schema;

namespace Architeptable.Data;

public class FacilityPart
{
    public int ID { get; set; }
    public double Quantity { get; set; }
    
    [InverseProperty(nameof(Facility.Outputs))]
    public Facility? Source { get; set; }
    public int? SourceID { get; set; }

    [InverseProperty(nameof(Facility.Inputs))]
    public Facility? Destination { get; set; }
    public int? DestinationID { get; set; }

    public Part Part { get; set; } = default!;
    public required int PartID { get; set; }
}
