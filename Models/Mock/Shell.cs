namespace Architeptable.Models;

public static partial class Mock
{
    public static Shell Shell => new Shell
    {
        Tabs = new LoadableModelBase[]
        {
            new Recipes(),
            new Ingredients()
        }
    };
}