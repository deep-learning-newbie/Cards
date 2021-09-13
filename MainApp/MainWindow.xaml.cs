using MainApp.ViewModels;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CardsViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new CardsViewModel();
            DataContext = _viewModel;
        }

        private void Add_Resource_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedCard == null) return;

            var dialog = new ResourceSelectorDialog();
            dialog.Owner = this;
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                ResourceBase resource = null;
                switch (dialog.ResourceTypeTitle.ToLower())
                {
                    case "image":
                        resource = new ImageResource() { Index = 3, Description = "some text" };
                        break;
                    case "table":
                        resource = new TableResource() { ResourceType = ResourceType.Table, Index = 1, Rows = new List<TableResourceItem>() { new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }, new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" } } };
                        break;
                    default:
                        break;
                }

                Task.Run(() =>
                {
                    if (_viewModel.AddResourceCommand.CanExecute(resource))
                        _viewModel.AddResourceCommand.Execute(resource);

                }).ConfigureAwait(false);
            }

            e.Handled = true;
        }

        private void Grid_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource is not FrameworkElement element) return;
            if (element.DataContext is not Card card) return;

            _viewModel.SelectedCard = card;
            
        }
    }
}
