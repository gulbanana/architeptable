using Architeptable.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Facilities : TabModelBase
{
    public override string Header => "Facilities";
    public IEnumerable<FacilityModel> All { get; set; } = Enumerable.Empty<FacilityModel>();

    public Facilities(Shell? owner) : base(owner) { }

    private FacilityModel current = default!;
    public FacilityModel Current
    {
        get => current;
        set => RaiseAndSetIfChanged(ref current, value);
    }

    internal override async Task LoadAsync(EntityContext context)
    {
        var self = this;
        var allFacilities = from f in context.Facilities
                            select new FacilityModel { Owner = self, ID = f.ID, Name = f.Name };

        All = await allFacilities.ToListAsync();
    }

    public class FacilityModel
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