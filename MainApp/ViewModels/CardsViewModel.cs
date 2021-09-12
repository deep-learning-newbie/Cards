using System.ComponentModel;
using System.Windows.Data;
using Models;
using Commands;
using System.Threading.Tasks;
using System;
using Queries;

namespace MainApp.ViewModels
{
    public class CardsViewModel : ViewModelBase
    {
        #region attributes
        private Card _seletcedItem;
        private ICollectionView _view;
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