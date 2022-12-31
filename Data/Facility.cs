using System.Collections.Generic;

namespace Architeptable.Data;

class Facility
{
    public int ID { get; set; }
    public string Name { get; set; } = "New Facility";

    public ICollection<Ingredient> Inputs { get; set; } = default!;
    public ICollection<Ingredient> Outputs { get; set; } = default!;
}
