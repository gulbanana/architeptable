using Architeptable.Models;
using Avalonia.Controls;
using Avalonia.Layout;
using System;

namespace Architeptable.Views
{
    public partial class AsyncView : UserControl
    {
        public AsyncView()
        {
            InitializeComponent();
        }

        private async void OnDataContext(object sender, EventArgs e)
        {
            if (DataContext is LoadableModelBase lmb && !lmb.IsLoaded)
            {
                Content = new ProgressBar
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsIndeterminate = true
                };
                await lmb.LoadAsync();
                Content = lmb;
            }
            else
            {
                Content = DataContext;
            }
        }
    }
}
