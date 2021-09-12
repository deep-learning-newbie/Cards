using System.ComponentModel;
using System.Windows.Data;
using Models;
using System.Collections.Generic;
using Commands;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using System.Threading;
using Queries;

namespace MainApp.ViewModels
{
    public class CardsViewModel : ViewModelBase
    {
        #region attributes
        private Card _seletcedItem;
        private ObservableCollection<Card> _entities;
        private ICollectionView _view;
        #endregion

        public CardsViewModel(/*List<Card> cards*/) // : this()
        {
            // TODO: uncomment this line
            // Cards = new ListCollectionView(cards);
        }

        //#region for test purpose only 
        //public CardsViewModel()
        //{
        //    _entities = new ObservableCollection<Card>();
        //    var card2 = new Card()
        //    {
        //        Id = 2,
        //        InEditMode = true,
        //        Title = "Card 0",
        //        Childs = new List<Card>()
        //    };
        //    card2.Resources.Add(new ImageResource() { Index = 1, Description = "some text", Uri = "Image1" });
        //    var card21 = new Card()
        //    {
        //        Id = 21,
        //        InEditMode = false,
        //        Title = "Card 0.1",                
        //    };
        //    card21.Resources.Add(new TableResource() { Index = 1, Rows = new List<TableResourceItem>() { new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }, new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" } } });
        //    card21.Resources.Add(new TableResource() { Index = 2, Rows = new List<TableResourceItem>() { new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }, new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" } } });
        //    card21.Resources.Add(new ImageResource() { Index = 3, Description = "some text", Uri = "340719-200.png" });

        //    card2.Childs.Add(card21);
        //    _entities.Add(card2);

        //    //foreach (var item in cards)
        //    //{
        //    //    Cards.Add(item);
        //    //}

        //    //_cardsView = new ListCollectionView(_cards);
        //    //_view = CollectionViewSource.GetDefaultView(_entities);
        //    Cards =  CollectionViewSource.GetDefaultView(_entities);
        //}
        //#endregion

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

        public async Task DeleteAsync(Card card)
        {
            card = card ?? throw new ArgumentNullException(nameof(card));

            var deleteCommand = new DeleteCardCommand();
            await deleteCommand.ExecuteAsync(card.Id);

            await RefreshAsync().ConfigureAwait(false);
        }

        public async Task AddCardAsync(int parentCardId, Card card) 
        {
            card = card ?? throw new ArgumentNullException(nameof(card));

            var addCommand = new AddChildCardCommand();
            await addCommand.ExecuteAsync(parentCardId, card.Title);

            await RefreshAsync().ConfigureAwait(false);
        }

        public async Task RemoveCardAsync(Card card) 
        {
            if (Cards.CurrentItem is not Card) return;

            var deleteCommand = new DeleteCardCommand();
            await deleteCommand.ExecuteAsync(card.Id);

            await RefreshAsync().ConfigureAwait(false);
        }

        public async Task AddResourceAsync(ResourceBase resource) 
        {
            if (SelectedItem is not Card card) return;
            resource = resource ?? throw new ArgumentNullException(nameof(resource));

            var addResourceCommand = new AddCardResourceCommand();
            await addResourceCommand.ExecuteAsync(card.Id, resource.ResourceType, "Item 1", "Item 2", "TEST BODY");

            await RefreshAsync().ConfigureAwait(false);
        }

        public async Task RemoveResourceAsync(ResourceBase resource)
        {
            // if (SelectedItem is not Card) return;
            resource = resource ?? throw new ArgumentNullException(nameof(resource));

            var deleteCardResourceCommand = new DeleteCardResourceCommand();
            await deleteCardResourceCommand.ExecuteAsync(resource.Id);
            
            await RefreshAsync().ConfigureAwait(false);
        }

        public async Task RefreshAsync()
        {
            var cardsQuery = new CardsQuery();
            var cards = await cardsQuery.ExecuteAsync();
            Cards = CollectionViewSource.GetDefaultView(cards);
        }
    }
}