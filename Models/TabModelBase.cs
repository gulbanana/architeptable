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
            isLoaded = true;
        }
    }

    public void Invalidate()
    {
        isLoaded = false;
    }

    internal void Save(Action<EntityContext> write, bool reload)
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
                    if (reload)
                    {
                        await LoadAsync(context);
                    }
                    else
                    {
                        await CalculateAsync(context);
                    }
                }
            }
        });
    }

    internal abstract Task LoadAsync(EntityContext context);

    internal abstract Task CalculateAsync(EntityContext context);
}
