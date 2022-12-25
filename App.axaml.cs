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
                           let im = new Models.Recipes.Ingredient(i.Name, ri.Quantity, ri.IsOutput)
                           group im by r.Name into g
                           select new Models.Recipes.Recipe { Name = g.Key, Ingredients = g }).ToArray();

            desktop.MainWindow = new MainWindow
            {
                DataContext = new object[]
                {
                    new Models.Factories()
                    {

                    },
                    new Models.Recipes
                    {
                        All = recipes,
                        Current = recipes.First()
                    },
                    new Models.Ingredients
                    {
                        All = from i in Data.Ingredients
                              select new Models.Ingredients.Ingredient { Name = i.Name }
                    }
                }
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}