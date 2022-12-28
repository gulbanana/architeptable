using Architeptable.Data;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Factories : TabModelBase
{
    public override string Header => "Factories";

    internal override Task LoadAsync(EntityContext context)
    {
        return Task.CompletedTask;
    }
}