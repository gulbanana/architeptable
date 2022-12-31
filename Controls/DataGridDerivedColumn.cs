using Avalonia.Controls;
using Avalonia.Layout;

namespace Architeptable.Controls;

public class DataGridDerivedColumn : DataGridTextColumn
{
    public DataGridDerivedColumn() 
    { 
        IsReadOnly = true;
    }

    protected override IControl GenerateElement(DataGridCell cell, object dataItem)
    {
        var text = (TextBlock)base.GenerateElement(cell, dataItem);
        text.VerticalAlignment = VerticalAlignment.Center;
        return new Border
        {
            Classes = new("derived"),
            Child = text,
            Padding = new(12, 0)
        };
    }
}
