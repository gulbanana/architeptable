using Architeptable.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Shell : LoadableModelBase
{
    public override bool IsLoaded => false;
    public IEnumerable<TabModelBase> Tabs { get; set; } = Enumerable.Empty<TabModelBase>();

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

            Tabs = new TabModelBase[]
            {
                new Factories(this),
                new Recipes(this),
                new Parts(this)
            };
        });
    }

    public void InvalidateOthers(TabModelBase sender)
    {
        foreach (var tab in Tabs)
        {
            if (tab != sender)
            {
                tab.Invalidate();
            }
        }
    }
}
