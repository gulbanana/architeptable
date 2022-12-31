﻿using System.Collections.Generic;

namespace Architeptable.Data;

public class Facility
{
    public int ID { get; set; }
    public string Name { get; set; } = "New Facility";

    public ICollection<Ingredient> Specifications { get; set; } = default!;
    public ICollection<Recipe> Processes { get; set; } = default!;
}
