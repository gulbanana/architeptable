using System.Collections.Generic;

namespace Architeptable.Data;

public class Facility
{
    public int ID { get; set; }
    public string Name { get; set; } = "New Facility";

    public ICollection<FacilityPart> InputOutputs { get; set; } = default!;
    public ICollection<Process> Processes { get; set; } = default!;
}
