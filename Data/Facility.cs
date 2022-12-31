using System.Collections.Generic;

namespace Architeptable.Data;

public class Facility
{
    public int ID { get; set; }
    public string Name { get; set; } = "New Facility";

    public ICollection<Part> Inputs { get; set; } = default!;
    public ICollection<Part> Outputs { get; set; } = default!;
}
