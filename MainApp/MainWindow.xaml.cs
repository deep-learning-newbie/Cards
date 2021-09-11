using Queries;
using MainApp.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoMapper;
// using MainApp.Helpers;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CardsViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            // MapperWrapper.Initialize();

            Task.Run(InitViewModel).Wait();
            this.DataContext = viewModel;
        }

        private async Task InitViewModel()
        {
            var cardsQuery = new CardsQuery();
            var cards = await cardsQuery.ExecuteAsync();
            viewModel = new CardsViewModel(cards);
        }

        private async Task RefreshDataAsync()
        {
            var globlaThis = this;

            var cardsQuery = new CardsQuery();
            var cards = await cardsQuery.ExecuteAsync();
            //viewModel.Cards = new ListCollectionView(new ObservableCollection<Card>(cards));
            Dispatcher.Invoke(new Action (() => globlaThis.DataContext = viewModel));
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            if (DataContext is not CardsViewModel vm) return;

            var title = @"Card #{vm.Cards.Count}"; //CurrentItem
            var card = new Card()
            {
                Id = vm.Cards.Count,
                InEditMode = true,
                Title = title,
                Resources = new ObservableCollection<ResourceBase>(),
                Childs = new List<Card>()
            };
            vm.Cards.AddNewItem(card);
        }

        private void MenuItem_Edit(object sender, RoutedEventArgs e)
        {
            var viewModel = (sender as MenuItem).DataContext as CardsViewModel;
            if (viewModel == null || viewModel.SelectedItem == null || viewModel.SelectedItem.InEditMode)
                return;

            var currentItem = viewModel.SelectedItem;

        }

        private void MenuItem_Delete(object sender, RoutedEventArgs e)
        {
            var viewModel = (sender as MenuItem).DataContext as CardsViewModel;
            if (viewModel == null || viewModel.SelectedItem == null)
                return;

            Task.Run(viewModel.DeleteAsync).Wait();
            Task.Run(RefreshDataAsync).Wait();
        }

        private void Remove_Resource_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not CardsViewModel vm) return;
            if (vm.Cards.CurrentItem is not Card card) return;
            if (sender is not Button button) return;
            if (button.DataContext is not ResourceBase resource) return;

            card.Resources.Remove(resource);

            e.Handled = true;
        }
        private void Add_Resource_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not CardsViewModel vm) return;

            vm.SelectedItem?.Resources.Add(new TableResource() { Index = 1, Rows = new List<TableResourceItem>() { new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }, new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" } } });

            e.Handled = true;
        }
    }
}
