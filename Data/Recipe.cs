using System.Collections.Generic;

namespace Architeptable.Data;

class Recipe
{
    public int ID { get; set; }
    public string Name { get; set; } = "New Recipe";

    public ICollection<Ingredient> Ingredients { get; set; } = default!;
}
