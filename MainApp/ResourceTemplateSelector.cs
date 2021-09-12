using Models;
using System.Windows;
using System.Windows.Controls;

namespace MainApp
{
    public class ResourceTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null && item is ImageResource)
            {
                return element.FindResource("ImageResourceItemTemplate") as DataTemplate; 
            }

            if (element != null && item != null && item is TableResource)
            {
                return element.FindResource("TableResourceItemTemplate") as DataTemplate;
            }

            return null;
        }
    }
}
