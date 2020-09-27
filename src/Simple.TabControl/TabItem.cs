using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Simple
{
    public class TabItem
    {
        public FrameworkElement Header { get; set; }
        public FrameworkElement Content { get; set; }
        internal Grid ContainerGrid { get; set; }
    }
}
