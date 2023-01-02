using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;

namespace Architeptable.Controls;

public class DataGridDerivedColumn : DataGridBoundColumn
{
    public DataGridDerivedColumn() 
    { 
        IsReadOnly = true;
    }

    protected override IControl GenerateElement(DataGridCell cell, object dataItem)
    {
        return new TextBlock
        {
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(12, 0, 0, 0),
            [!TextBlock.TextProperty] = new Binding(((Binding)Binding).Path + ".Content"),
            [!TextBlock.ForegroundProperty] = new Binding(((Binding)Binding).Path + ".Highlight"),
        };
    }

    protected override object PrepareCellForEdit(IControl editingElement, RoutedEventArgs editingEventArgs)
    {
        throw new NotSupportedException();
    }

    protected override IControl GenerateEditingElementDirect(DataGridCell cell, object dataItem)
    {
        throw new NotImplementedException();
    }
}
