using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;

namespace Architeptable.Models;

public static partial class Mock
{
    private static readonly IBitmap iconLogo = new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>()!.Open(new Uri(@"avares://Architeptable/Assets/avalonia-logo.ico")));
    private static readonly IBitmap iconSpinner = new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>()!.Open(new Uri(@"avares://Architeptable/Assets/Rolling-1s-32px.gif")));

    private static readonly List<Parts.Row> parts = new()
    {
        new() { Name = "Water", Icon = iconLogo },
        new() { Name = "Iron Ore", Icon = iconLogo },
        new() { Name = "Iron Ingot", Icon = iconLogo },
        new() { Name = "Copper Ore", Icon = iconLogo },
        new() { Name = "Copper Ingot", Icon = iconSpinner, IconSpinning = true },
    };

    public static readonly Parts Parts = new(null)
    {
        All = parts
    };
}