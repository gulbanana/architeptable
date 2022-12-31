using Architeptable.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Factories : TabModelBase
{
    public class Facility
    {
        public required string Name { get; init; }
    }

    public override string Header => "Factories";

    public IEnumerable<Facility> All { get; set; } = Enumerable.Empty<Facility>();

    internal override async Task LoadAsync(EntityContext context)
    {
        var allFacilities = from f in context.Facilities
                            select new Facility { Name = f.Name };

        All = await allFacilities.ToListAsync();
    }
}