using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Simple
{
    public partial class TabControl : Grid
    {
        #region Fields
        private Grid HeaderGrid { get; } = new Grid { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
        private Grid SelectedItemContent { get; } = new Grid { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
        #endregion

        #region Public Properties
        public List<TabItem> Items { get; set; } = new List<TabItem>();
        #endregion

        #region Constructor
        public TabControl()
        {
            Children.Add(HeaderGrid);
            Children.Add(SelectedItemContent);

            RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            SetRow(SelectedItemContent, 1);

            Loaded += TabControl_Loaded;
        }
        #endregion

        #region Event Handlers
        private void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= TabControl_Loaded;

            var i = 0;
            foreach (var item in Items)
            {
                HeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                item.ContainerGrid = new Grid();
                item.ContainerGrid.Children.Add(item.Header);

                HeaderGrid.Children.Add(item.ContainerGrid);
                SetColumn(item.ContainerGrid, i);

                item.ContainerGrid.Tag = item;
                item.ContainerGrid.Tapped += Header_Tapped;

                item.Content.Visibility = Visibility.Collapsed;
                SelectedItemContent.Children.Add(item.Content);

                i++;
            }

            if (Items.Count > 0) Select(Items[0]);
        }

        private void Header_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var headerGridContainer = (Grid)sender;
            var tabItem = (TabItem)headerGridContainer.Tag;
            Select(tabItem);
        }
        #endregion

        #region Private Methods
        private void Select(TabItem tabItem)
        {
            foreach (var item in Items)
            {
                item.ContainerGrid.Background = ReferenceEquals(tabItem, item) ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Transparent);
                item.Content.Visibility = ReferenceEquals(tabItem, item) ? Visibility.Visible : Visibility.Collapsed;
                //Canvas.SetZIndex(item.Content, ReferenceEquals(tabItem, item) ? 1 : 0);
            }
        }
        #endregion
    }
}
