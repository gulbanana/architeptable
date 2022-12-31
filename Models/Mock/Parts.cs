using System.Collections.Generic;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly List<Parts.Ingredient> parts = new()
    {
        new() { Name = "Water" },
        new() { Name = "Iron Ore" },
        new() { Name = "Iron Ingot" },
        new() { Name = "Copper Ore" },        
        new() { Name = "Copper Ingot" },
    };

    public static readonly Parts Parts = new()
    {
        All = parts
    };
}