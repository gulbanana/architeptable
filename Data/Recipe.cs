using System.Collections.Generic;

namespace Architeptable.Data;

public class Recipe
{
    public int ID { get; set; }
    public string Name { get; set; } = "New Recipe";

    public ICollection<RecipePart> Ingredients { get; set; } = default!;
}
