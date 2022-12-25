using System.Collections.Generic;
using ReactiveUI;

namespace Architeptable.Models;

class Ingredients : ReactiveObject
{
    public class Ingredient
    {
        public required string Name { get; init; }
    }

    public string Header => "Ingredients";

    public required IEnumerable<Ingredient> All { get; init; }
}