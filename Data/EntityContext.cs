using Microsoft.EntityFrameworkCore;
using System;

namespace Architeptable.Data;

public class EntityContext : DbContext
{
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipePart> RecipeParts => Set<RecipePart>();
    public DbSet<Facility> Facilities => Set<Facility>();
    public DbSet<FacilityPart> FacilityParts => Set<FacilityPart>();
    public DbSet<FacilityRecipe> Processes => Set<FacilityRecipe>();

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
            new Part() { ID = -3, Name = "Iron Plate" },
            new Part() { ID = -4, Name = "Copper Ore" },
            new Part() { ID = -5, Name = "Iron Rod" },
            new Part() { ID = -6, Name = "Screw" }
        );

        builder.Entity<Recipe>().HasData(
            new Recipe() { ID = -1, Name = "Iron Ingot" },
            new Recipe() { ID = -2, Name = "Iron Plate" },
            new Recipe() { ID = -3, Name = "Iron Rod" },
            new Recipe() { ID = -4, Name = "Screw" }
        );

        builder.Entity<Facility>().HasData(
            new Facility() { ID = -1, Name = "Main Base" },
            new Facility() { ID = -2, Name = "Screw Factory" }
        );

        builder.Entity<RecipePart>().HasData(
            new RecipePart() { ID = -1, RecipeID = -1, PartID = -1, Quantity = 30 },
            new RecipePart() { ID = -2, RecipeID = -1, PartID = -2, Quantity = 30, IsOutput = true },
            new RecipePart() { ID = -3, RecipeID = -2, PartID = -2, Quantity = 30 },
            new RecipePart() { ID = -4, RecipeID = -2, PartID = -3, Quantity = 20, IsOutput = true },
            new RecipePart() { ID = -5, RecipeID = -3, PartID = -2, Quantity = 15 },
            new RecipePart() { ID = -6, RecipeID = -3, PartID = -5, Quantity = 15, IsOutput = true },
            new RecipePart() { ID = -7, RecipeID = -4, PartID = -5, Quantity = 10 },
            new RecipePart() { ID = -8, RecipeID = -4, PartID = -6, Quantity = 40, IsOutput = true }
        );

        builder.Entity<FacilityPart>().HasData(
            new FacilityPart() { ID = -1, PartID = -1, Quantity = 120, DestinationID = -1 },
            new FacilityPart() { ID = -2, PartID = -4, Quantity = 60, DestinationID = -1 },
            new FacilityPart() { ID = -3, PartID = -3, Quantity = 60, SourceID = -1 },
            new FacilityPart() { ID = -4, PartID = -5, Quantity = 60, SourceID = -1, DestinationID = -2 },
            new FacilityPart() { ID = -5, PartID = -6, Quantity = 60, SourceID = -2 }
        );

        builder.Entity<FacilityRecipe>().HasData(
            new FacilityRecipe() { ID = -1, FacilityID = -1, RecipeID = -1, Machines = 2 },
            new FacilityRecipe() { ID = -2, FacilityID = -1, RecipeID = -2, Machines = 1 },
            new FacilityRecipe() { ID = -3, FacilityID = -1, RecipeID = -3, Machines = 1 },
            new FacilityRecipe() { ID = -4, FacilityID = -2, RecipeID = -4, Machines = 1 }
        );
    }
}