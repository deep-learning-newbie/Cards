using System.ComponentModel;
using System.Windows.Data;
using Models;
using System.Collections.Generic;
using Commands;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using System.Threading;

namespace MainApp.ViewModels
{
    public class CardsViewModel : ViewModelBase
    {
        #region attributes
        private Card _seletcedItem;
        private ObservableCollection<Card> _entities;
        private ICollectionView _view;
        #endregion

        public CardsViewModel(List<Card> cards) : this()
        {
            //TODO: uncomment this line
            //Cards = new ListCollectionView(cards);
        }

        #region for test purpose only 
        public CardsViewModel()
        {
            _entities = new ObservableCollection<Card>();
            var card2 = new Card()
            {
                Id = 2,
                InEditMode = true,
                Title = "Card 0",
                Childs = new List<Card>()
            };
            card2.Resources.Add(new ImageResource() { Index = 1, Description = "some text", Uri = "Image1" });
            var card21 = new Card()
            {
                Id = 21,
                InEditMode = false,
                Title = "Card 0.1",                
            };
            card21.Resources.Add(new TableResource() { Index = 1, Rows = new List<TableResourceItem>() { new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }, new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" } } });
            card21.Resources.Add(new TableResource() { Index = 2, Rows = new List<TableResourceItem>() { new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }, new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" } } });
            card21.Resources.Add(new ImageResource() { Index = 3, Description = "some text", Uri = "340719-200.png" });

            card2.Childs.Add(card21);
            _entities.Add(card2);

            //foreach (var item in cards)
            //{
            //    Cards.Add(item);
            //}

            //_cardsView = new ListCollectionView(_cards);
            //_view = CollectionViewSource.GetDefaultView(_entities);
            Cards =  CollectionViewSource.GetDefaultView(_entities);
        }
        #endregion

        public Card SelectedItem
        {
            get => _seletcedItem; set
            {
                if (_seletcedItem != null)
                    _seletcedItem.IsSelected = false;
                _seletcedItem = value;
                _seletcedItem.IsSelected = true;
                OnPropertyChanged();
            }
        }
        public ICollectionView Cards { get => _view; set { _view = value; OnPropertyChanged(); } }
        //public ObservableCollection<Card> Cards { get; } = new ObservableCollection<Card>();

        public async Task DeleteAsync()
        {
            if (SelectedItem == null)
                return;

            var deleteCommand = new DeleteCardCommand();
            await deleteCommand.ExecuteAsync(SelectedItem.Id);
        }

        public void AddCard(Card card) 
        {
            var title = $"Card #{_entities.Count}"; 
            var card1 = new Card()
            {
                Id = _entities.Count,
                InEditMode = true,
                Title = title,
                Childs = new List<Card>()
            };

            _entities.Add(card1);
            //App.Current.Dispatcher.Invoke(()=>
            //{
            //    _entities.Add(card1);
            //});
            //var uiContext = SynchronizationContext.Current;
            //uiContext.Send(x => _entities.Add(card1), null);

            if (_view is not ListCollectionView view) return;
            view.Refresh();
        }
        public void RemoveCard(Card card) 
        {
            if (Cards.CurrentItem is not Card card1) return;

            _entities.Remove(card1);

            if (_view is not ListCollectionView view) return;
            view.Refresh();
        }
        public void AddResource(ResourceBase resource) 
        {
            //if (Cards.CurrentItem is not Card card) return;
            if (SelectedItem is not Card card) return;
            resource = resource ?? throw new ArgumentNullException(nameof(resource));

            if (_view is not ListCollectionView view) return;
            //view.EditItem(card);
            card.Resources.Add(resource);
            //view.CommitEdit();
            view.Refresh();
        }
        public void RemoveResource(ResourceBase resource)
        {
            //if (Cards.CurrentItem is not Card card) return;
            if (SelectedItem is not Card card) return;
            resource = resource ?? throw new ArgumentNullException(nameof(resource));

            if (_view is not ListCollectionView view) return;
            //view.EditItem(card);
            card.Resources.Remove(resource);
            //view.CommitEdit();
            view.Refresh();
        }
    }
}