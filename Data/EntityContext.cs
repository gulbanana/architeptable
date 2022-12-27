using Microsoft.EntityFrameworkCore;
using System;

namespace Architeptable.Data;

class EntityContext : DbContext
{
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

    public string DbPath { get; }

    public EntityContext()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = System.IO.Path.Join(localAppData, "architeptable.etilqs");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
}