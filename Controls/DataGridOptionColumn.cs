using Architeptable.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;
using System.Collections;
using System.Linq;
using System.Reactive.Subjects;

namespace Architeptable.Controls;

public class DataGridOptionColumn : DataGridBoundColumn
{
    public DataGridOptionColumn() 
    {
        IsReadOnly = false;
        BindingTarget = ComboBox.SelectedItemProperty;
    }

    protected override void LogBindingError(AvaloniaProperty property, Exception e) { }

    public static readonly StyledProperty<IEnumerable> ItemsProperty = AvaloniaProperty.Register<DataGridOptionColumn, IEnumerable>(nameof(Items), defaultValue: Enumerable.Empty<OptionModel>());
    public IEnumerable Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    protected override IControl GenerateElement(DataGridCell cell, object dataItem) => new TextBlock
    {
        VerticalAlignment = VerticalAlignment.Center,
        Margin = new(12, 0, 32, 0),
        [!TextBlock.TextProperty] = Binding is Binding b ? new Binding(b.Path + ".Name") : new Subject<string>().ToBinding()
    };

    protected override IControl GenerateEditingElementDirect(DataGridCell cell, object dataItem) => new ComboBox
    {
        VerticalAlignment = VerticalAlignment.Center,
        HorizontalAlignment = HorizontalAlignment.Stretch,
        IsDropDownOpen = true,
        ItemTemplate = new FuncDataTemplate<OptionModel>((model, scope) => new TextBlock { Text = model.Name }),
        [!ComboBox.ItemsProperty] = this[!ItemsProperty],
    };

    protected override object? PrepareCellForEdit(IControl editingElement, RoutedEventArgs editingEventArgs) => editingElement switch
    {
        ComboBox comboBox => comboBox.SelectedItem,
        _ => null
    };
}
