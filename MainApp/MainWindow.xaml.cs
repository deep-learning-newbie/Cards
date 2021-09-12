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

            InitViewModel();
            this.DataContext = _viewModel;
        }

        private void InitViewModel()
        {
            var cards = new List<Card>();
            _viewModel = new CardsViewModel();

            Dispatcher.Invoke(async () => await _viewModel.RefreshAsync().ConfigureAwait(false));
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedItem is not Card)
                return;

            Task.Run(() => _viewModel.AddCardAsync(_viewModel.SelectedItem.Id, new Card { Title = "Test" }));
            e.Handled = true;
        }

        private void MenuItem_Delete(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedItem == null)
                return;

            Task.Run(() => _viewModel.DeleteAsync(_viewModel.SelectedItem)).Wait();
            e.Handled = true;
        }

        private void Remove_Resource_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not CardsViewModel vm) return;
            if (sender is not Button button) return;
            if (button.DataContext is not ResourceBase resource) return;

            Task.Run(() => vm.RemoveResourceAsync(resource)).Wait();

            e.Handled = true;
        }
        private void Add_Resource_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedItem == null) return;

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

                Task.Run(() => _viewModel.AddResourceAsync(resource)).ConfigureAwait(false);
            }

            e.Handled = true;
        }
    }
}
