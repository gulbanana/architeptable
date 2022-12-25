using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Architeptable.Views;
using System.Linq;

namespace Architeptable;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var recipes = (from r in Data.Recipes
                           join ri in Data.RecipeIngredients on r.ID equals ri.RecipeID
                           join i in Data.Ingredients on ri.IngredientID equals i.ID
                           let im = new Models.Ingredient(i.Name, ri.Quantity, ri.IsOutput)
                           group im by r.Name into g
                           select new Models.Recipe { Name = g.Key, Ingredients = g }).ToArray();

            desktop.MainWindow = new MainWindow
            {
                DataContext = new Models.Recipes
                {
                    All = recipes,
                    Current = recipes.First()
                }
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}