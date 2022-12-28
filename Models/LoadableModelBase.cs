using Architeptable.Data;
using System.Threading.Tasks;

namespace Architeptable.Models;

public abstract class LoadableModelBase : ModelBase
{
    public abstract bool IsLoaded { get; }

    public abstract Task LoadAsync();
}
