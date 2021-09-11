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
    }
}
