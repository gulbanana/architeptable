using Architeptable.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Parts : TabModelBase
{
    public override string Header => "Parts";
    public IEnumerable<Row> All { get; set; } = Enumerable.Empty<Row>();

    public Parts(Shell? owner) : base(owner) { }

    internal override async Task LoadAsync(EntityContext context)
    {
        var self = this;
        var allIngredients = from i in context.Parts
                             select new Row { Owner = self, ID = i.ID, Name = i.Name };

        All = await allIngredients.ToListAsync();
    }

    public class Row : EntityModelBase<Part>
    {
        private string? name;
        public required string Name
        {
            get => name!;
            set => SaveIfChanged(ref name, value);
        }
    }
}