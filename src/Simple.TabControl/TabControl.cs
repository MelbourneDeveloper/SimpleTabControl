using System.Collections.Generic;
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
        public Brush SelectionBrush { get; set; } = new SolidColorBrush(Colors.Black);
        public Style HeaderSelectionStyle { get; set; }
        public Style HeaderNonSelectionStyle { get; set; }
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

                item.HeaderContainerGrid = new Grid();
                item.HeaderContainerGrid.Children.Add(item.Header);

                HeaderGrid.Children.Add(item.HeaderContainerGrid);
                SetColumn(item.HeaderContainerGrid, i);

                item.HeaderContainerGrid.Tag = item;
                item.HeaderContainerGrid.Tapped += Header_Tapped;

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
                if (HeaderSelectionStyle != null)
                {
                    item.Header.Style = ReferenceEquals(tabItem, item) ? HeaderSelectionStyle : HeaderNonSelectionStyle;
                }

                if (SelectionBrush != null)
                {
                    item.HeaderContainerGrid.Background = ReferenceEquals(tabItem, item) ? SelectionBrush : new SolidColorBrush(Colors.Transparent);
                }

                item.Content.Visibility = ReferenceEquals(tabItem, item) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion
    }
}
