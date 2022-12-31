using Architeptable.Data;
using Avalonia.Threading;
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
            await CalculateAsync(context);
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

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            using (var context = new EntityContext())
            {
                write(context);
                if (await context.SaveChangesAsync() > 0)
                {
                    owner.InvalidateOthers(this);
                    await CalculateAsync(context);
                }
            }
        });
    }

    internal abstract Task LoadAsync(EntityContext context);

    internal abstract Task CalculateAsync(EntityContext context);
}
