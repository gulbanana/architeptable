using System.Collections.Generic;

namespace Architeptable.Data;

public class Facility
{
    public int ID { get; set; }
    public string Name { get; set; } = "New Facility";

    public ICollection<FacilityPart> Inputs { get; set; } = default!;
    public ICollection<FacilityPart> Outputs { get; set; } = default!;
    public ICollection<FacilityRecipe> Processes { get; set; } = default!;
}
