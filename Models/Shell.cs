using Architeptable.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Shell : LoadableModelBase
{
    public override bool IsLoaded => false;
    public IEnumerable<LoadableModelBase> Tabs { get; set; } = Enumerable.Empty<LoadableModelBase>();

    public override Task LoadAsync()
    {
        // Microsoft.EntityFrameworkCore.Sqlite is not true async, so we have to thread it
        return Task.Run(() =>
        {
            using var context = new EntityContext();

            if (context.Database.EnsureCreated())
            {
                Bootstrap.InitDB(context);
            }

            Tabs = new LoadableModelBase[]
            {
                new Factories(),
                new Recipes(),
                new Ingredients()
            };
        });
    }
}
