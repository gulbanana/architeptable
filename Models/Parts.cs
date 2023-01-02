using Architeptable.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Parts : TabModelBase
{
    public override string Header => "Parts";
    public IReadOnlyList<Row> All { get; set; } = Array.Empty<Row>();

    public Parts(Shell? owner) : base(owner) { }

    internal override async Task LoadAsync(EntityContext context)
    {
        var self = this;
        var allIngredients = from p in context.Parts
                             orderby p.Name
                             select new Row { Owner = self, ID = p.ID, Name = p.Name };

        All = await allIngredients.ToListAsync();
    }

    internal override Task CalculateAsync(EntityContext context)
    {
        return Task.CompletedTask;
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