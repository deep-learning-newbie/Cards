using System.Collections.Generic;
using System.Windows;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for ResourceSelectorDialog.xaml
    /// </summary>
    public partial class ResourceSelectorDialog : Window
    {
        public ResourceSelectorDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        public IEnumerable<string> Items { get; } = new List<string>() { "Image", "Table" };
        public string ResourceTypeTitle { get; set; }

        private void OnOkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            e.Handled = true;
        }
        private void OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            e.Handled = true;
        }
    }
}
