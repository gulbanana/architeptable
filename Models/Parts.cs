using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Parts : TabModelBase
{
    public class Ingredient
    {
        public required string Name { get; init; }
    }

    public override string Header => "Parts";

    public IEnumerable<Ingredient> All { get; set; } = Enumerable.Empty<Ingredient>();

    internal override async Task LoadAsync(Data.EntityContext context)
    {
        var allIngredients = from i in context.Ingredients
                             select new Ingredient { Name = i.Name };

        All = await allIngredients.ToListAsync();
    }
}