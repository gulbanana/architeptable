using Microsoft.EntityFrameworkCore;
using System;

namespace Architeptable.Data;

public class EntityContext : DbContext
{
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Facility> Facilities => Set<Facility>();
    public DbSet<Process> Processes => Set<Process>();

    public string DbPath { get; }

    public EntityContext()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = System.IO.Path.Join(localAppData, "architeptable-prototype.etilqs");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options
#if !DEBUG
            .UseModel(Precompiled.EntityContextModel.Instance)
#endif
            .UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Part>().HasData(
            new Part() { ID = -1, Name = "Iron Ore" },
            new Part() { ID = -2, Name = "Iron Ingot" },
            new Part() { ID = -3, Name = "Iron Plate" }
        );

        builder.Entity<Recipe>().HasData(
            new Recipe() { ID = -1, Name = "Iron Ingot" },
            new Recipe() { ID = -2, Name = "Iron Plate" }
        );

        builder.Entity<Facility>().HasData(
            new Facility() { ID = -1, Name = "Main Base" }
        );

        builder.Entity<Ingredient>().HasData(
            new Ingredient() { ID = -1, RecipeID = -1, PartID = -1, Quantity = 30 },
            new Ingredient() { ID = -2, RecipeID = -1, PartID = -2, Quantity = 30, IsOutput = true },
            new Ingredient() { ID = -3, RecipeID = -2, PartID = -2, Quantity = 30 },
            new Ingredient() { ID = -4, RecipeID = -2, PartID = -3, Quantity = 20, IsOutput = true },
            new Ingredient() { ID = -6, FacilityID = -1, PartID = -1, Quantity = 120 },
            new Ingredient() { ID = -5, FacilityID = -1, PartID = -3, Quantity = 60, IsOutput = true }
        );

        builder.Entity<Process>().Property(p => p.Machines).HasDefaultValue(1);
        builder.Entity<Process>().Property(p => p.Overclock).HasDefaultValue(1.0);

        builder.Entity<Process>().HasData(
            new Process() { ID = -1, FacilityID = -1, RecipeID = -1, Machines = 2 },
            new Process() { ID = -2, FacilityID = -1, RecipeID = -2, Machines = 1 }
        );
    }
}