using Queries;
using MainApp.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
// using MainApp.Helpers;

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

            Dispatcher.Invoke(async ()=> await _viewModel.RefreshAsync().ConfigureAwait(false));
        }

        private async Task RefreshDataAsync()
        {
            var globalThis = this;

            var cardsQuery = new CardsQuery();
            var cards = await cardsQuery.ExecuteAsync();
            //viewModel.Cards = new ListCollectionView(new ObservableCollection<Card>(cards));
            Dispatcher.Invoke(new Action(() => globalThis.DataContext = _viewModel));
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedItem is not Card)
                return;

            Task.Run(() => _viewModel.AddCard(_viewModel.SelectedItem.Id, new Card { Title = "Test" }));
            e.Handled = true;
        }

        private void MenuItem_Edit(object sender, RoutedEventArgs e)
        {
            var viewModel = (sender as MenuItem).DataContext as CardsViewModel;
            if (viewModel == null || viewModel.SelectedItem == null || viewModel.SelectedItem.InEditMode)
                return;

            var currentItem = viewModel.SelectedItem;
            currentItem.InEditMode = true;
        }

        private void MenuItem_Delete(object sender, RoutedEventArgs e)
        {
            //var viewModel = (sender as MenuItem).DataContext as CardsViewModel;
            //if (viewModel == null || viewModel.SelectedItem == null)
            //    return;

            if (_viewModel.SelectedItem == null)
                return;

            Task.Run(() => _viewModel.DeleteAsync(_viewModel.SelectedItem)).Wait();
            Task.Run(RefreshDataAsync).Wait();

            if (DataContext is not CardsViewModel vm) return;

            Task.Run(() => vm.RemoveCard(_viewModel.SelectedItem));
            e.Handled = true;
        }

        private void Remove_Resource_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not CardsViewModel vm) return;
            // if (vm.SelectedItem/*Cards.CurrentItem*/ is not Card card) return;
            if (sender is not Button button) return;
            if (button.DataContext is not ResourceBase resource) return;

            Task.Run(() => vm.RemoveResource(resource)).Wait();

            e.Handled = true;
        }
        private void Add_Resource_Click(object sender, RoutedEventArgs e)
        {
            // if (DataContext is not CardsViewModel vm) return;
            // if (vm.SelectedItem/*Cards.CurrentItem*/ is not Card card) return;
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
                        resource = new ImageResource() { Index = 3, Description = "some text", Uri = "340719-200.png" };
                        break;
                    case "table":
                        resource = new TableResource() { Index = 1, Rows = new List<TableResourceItem>() { new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }, new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" } } };
                        break;
                    default:
                        break;
                }

                _viewModel.AddResource(resource);
            }

            e.Handled = true;
        }
    }
}
