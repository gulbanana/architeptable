using Microsoft.EntityFrameworkCore;
using System;

namespace Architeptable.Data;

class EntityContext : DbContext
{
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Facility> Facilities => Set<Facility>();

    public string DbPath { get; }

    public EntityContext()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = System.IO.Path.Join(localAppData, "architeptable.etilqs");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options
#if !DEBUG
            .UseModel(Precompiled.EntityContextModel.Instance)
#endif
            .UseSqlite($"Data Source={DbPath}");
}