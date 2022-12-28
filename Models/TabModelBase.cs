using Architeptable.Data;
using System.Threading.Tasks;

namespace Architeptable.Models;

public abstract class TabModelBase : LoadableModelBase
{
    private bool isLoaded;
    public override bool IsLoaded => isLoaded;
    public abstract string Header { get; }

    public sealed override async Task LoadAsync()
    {
        using (var context = new EntityContext())
        {
            await LoadAsync(context);
            isLoaded = true;
        }
    }

    protected void Invalidate()
    {
        isLoaded = false;
    }

    internal abstract Task LoadAsync(EntityContext context);
}
