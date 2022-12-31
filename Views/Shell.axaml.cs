using Avalonia.Controls;
using Avalonia.LogicalTree;
using System;
using System.Linq;

namespace Architeptable.Views
{
    public partial class Shell : UserControl
    {
        public Shell()
        {
            InitializeComponent();
        }

        public void TabsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is TabControl tabs)
            {
                for (var i = 0; i < tabs.ItemCount; i++)
                {
                    var container = tabs.ItemContainerGenerator.ContainerFromIndex(i);
                    container.Classes.Clear();
                    if (i < tabs.SelectedIndex)
                    {
                        container.Classes.Add("left");
                    }
                    else if (i > tabs.SelectedIndex)
                    {
                        container.Classes.Add("right");
                    }
                }
            }
        }
    }
}
