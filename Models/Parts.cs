using Architeptable.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using HarfBuzzSharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Architeptable.Models;

public class Parts : TabModelBase
{
    public override string Header => "Parts";
    public IReadOnlyList<Row> All { get; set; } = Array.Empty<Row>();

    public Parts(Shell? owner) : base(owner) { }

    internal override async Task LoadAsync(EntityContext context)
    {
        var self = this;
        var allIngredients = from p in context.Parts
                             orderby p.Name
                             select new Row { Owner = self, ID = p.ID, Name = p.Name, Icon = CreateIcon(p.Icon) };

        All = await allIngredients.ToListAsync();
    }

    private static IBitmap? CreateIcon(byte[]? data)
    {
        if (data is null)
        {
            return null;
        }

        using var buffer = new MemoryStream(data);
        return Bitmap.DecodeToWidth(buffer, 32);
    }

    internal override Task CalculateAsync(EntityContext context)
    {
        return Task.CompletedTask;
    }

    public class Row : EntityModelBase<Part>
    {
        private static Lazy<Bitmap> spinner = new(() =>
        {
            var loader = AvaloniaLocator.Current.GetService<IAssetLoader>();
            return new Bitmap(loader!.Open(new Uri(@"avares://Architeptable/Assets/Rolling-1s-32px.gif")));
        });

        private string? name;
        public required string Name
        {
            get => name!;
            set => SaveIfChanged(ref name, value);
        }

        private IBitmap? icon;
        public IBitmap? Icon
        {
            get => icon;
            set => RaiseAndSetIfChanged(ref icon, value);
        }

        private bool iconSpinning;
        public bool IconSpinning
        {
            get => iconSpinning;
            set => RaiseAndSetIfChanged(ref iconSpinning, value);
        }

        public async Task SetIcon()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                return;
            }

            var saveIcon = icon;
            Icon = spinner.Value;
            IconSpinning = true;

            try
            {
                var dialog = new OpenFileDialog 
                { 
                    AllowMultiple = false, 
                    Title = "Select Image" 
                };

                var files = await dialog.ShowAsync(desktop.MainWindow);
                if (files == null)
                {
                    Icon = saveIcon;
                    return;
                }

                using var file = File.OpenRead(files.Single());
                using var buffer = new MemoryStream();
                await file.CopyToAsync(buffer);
                buffer.Position = 0;
                Icon = Bitmap.DecodeToWidth(buffer, 32);                    
                Save(e => e.Icon = buffer.ToArray());                    
            }
            catch (Exception)
            {
                Icon = null; // XXX report problem?
            }
            finally
            {
                IconSpinning = false;
            }
        }
    }
}
