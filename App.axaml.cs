using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Architeptable.Views;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace Architeptable;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            throw new NotSupportedException();
        }

        using var context = new Data.EntityContext();
        if (context.Database.EnsureCreated())
        {
            Data.Bootstrap.InitDB(context);
        }

        var recipesWithIngredients = context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .Select(r => new Models.Recipes.Recipe
            {
                Name = r.Name,
                Ingredients = r.Ingredients.Select(ri => new Models.Recipes.Ingredient
                {
                    Name = ri.Ingredient.Name,
                    Quantity = ri.Quantity,
                    IsOutput = ri.IsOutput
                }).ToList()
            })
            .ToList();

        var allIngredients = from i in context.Ingredients
                             select new Models.Ingredients.Ingredient { Name = i.Name };

        var tabs = new object[]
        {
            new Models.Factories()
            {

            },
            new Models.Recipes
            {
                All = recipesWithIngredients,
                Current = recipesWithIngredients.First()
            },
            new Models.Ingredients
            {
                All = allIngredients.ToList()
            }
        };

        desktop.MainWindow = new MainWindow { DataContext = tabs  };

        base.OnFrameworkInitializationCompleted();
    }
}