using Architeptable.Data;
using System;
using System.Threading.Tasks;

namespace Architeptable.Models;

public abstract class TabModelBase : LoadableModelBase
{
    private readonly Shell? owner;
    private bool isLoaded;
    public override bool IsLoaded => isLoaded;
    public abstract string Header { get; }

    public TabModelBase(Shell? owner)
    {
        this.owner = owner;
    }

    public sealed override async Task LoadAsync()
    {
        using (var context = new EntityContext())
        {
            await LoadAsync(context);
            isLoaded = true;
        }
    }

    public void Invalidate()
    {
        isLoaded = false;
    }

    internal void Save(Action<EntityContext> write)
    {
        if (owner is null)
        {
            return;
        }

        using (var context = new EntityContext())
        {
            write(context);
            context.SaveChanges();
        }

        owner.InvalidateOthers(this);
    }

    internal abstract Task LoadAsync(EntityContext context);
}
