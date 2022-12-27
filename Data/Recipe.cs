using System.Collections.Generic;

namespace Architeptable.Data;

class Recipe
{
    public int ID { get; set; }
    public string Name { get; set; } = "New Recipe";

    public ICollection<RecipeIngredient> Ingredients { get; set; } = default!;
}