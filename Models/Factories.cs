using Architeptable.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Factories : TabModelBase
{
    public override string Header => "Factories";
    public IEnumerable<Row> All { get; set; } = Enumerable.Empty<Row>();

    public Factories(Shell? owner) : base(owner) { }

    internal override async Task LoadAsync(EntityContext context)
    {
        var self = this;
        var allFacilities = from f in context.Facilities
                            select new Row { Owner = self, ID = f.ID, Name = f.Name };

        All = await allFacilities.ToListAsync();
    }

    public class Row
    {
        public TabModelBase? Owner { get; init; }
        public int ID { get; init; }

        private string? name;
        public required string Name
        {
            get => name!;
            set
            {
                name = value;
                Owner?.Save(c => c.Facilities.Find(ID)!.Name = value);
            }
        }
    }
}