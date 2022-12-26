using System.Collections.Generic;

namespace Architeptable.Models;

class Ingredients : ModelBase
{
    public class Ingredient
    {
        public required string Name { get; init; }
    }

    public string Header => "Ingredients";

    public required IEnumerable<Ingredient> All { get; init; }
}