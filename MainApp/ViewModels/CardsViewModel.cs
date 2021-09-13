using System.ComponentModel;
using System.Windows.Data;
using Models;
using Commands;
using System.Threading.Tasks;
using System;
using Queries;
using MainApp.Behaviours;
using System.Windows.Input;

namespace MainApp.ViewModels
{
    public class CardsViewModel : ViewModelBase
    {
        #region attributes
        private Card _seletcedCard;
        private ICollectionView _view;
        private ResourceBase _selectedResource;
        #endregion

        public CardsViewModel()
        {
            RefreshCommand = new AsyncRelayCommand(RefreshAsync, (x) => true);
            SaveCardCommand = new AsyncRelayCommand(SaveCardAsync, (x) => true);
            AddCardCommand = new AsyncRelayCommand(AddCardAsync, (x) => true);
            RemoveCardCommand = new AsyncRelayCommand(RemoveCardAsync, (x) => true);
            AddResourceCommand = new AsyncRelayCommand(AddResourceAsync, (x) => true);
            RemoveResourceCommand = new AsyncRelayCommand(RemoveResourceAsync, (x) => true);
            PreviewMouseUpCommand = new AsyncRelayCommand(PreviewMouseUpAsync, (x) => true);
        }

        #region properties
        public Card SelectedCard
        {
            get => _seletcedCard; set
            {
                if (_seletcedCard != null)
                    _seletcedCard.IsSelected = false;
                _seletcedCard = value;
                if (_seletcedCard != null)
                    _seletcedCard.IsSelected = true;

                OnPropertyChanged();
            }
        }
        public ResourceBase SelectedResource { get => _selectedResource; set { _selectedResource = value; OnPropertyChanged(); } }
        public ICollectionView Cards { get => _view; set { _view = value; OnPropertyChanged(); } }
        #endregion

        #region commands
        public ICommand RefreshCommand { get; }
        public async Task RefreshAsync(object param)
        {
            var cardsQuery = new CardsQuery();

            var cards = await cardsQuery.ExecuteAsync();
            Cards = CollectionViewSource.GetDefaultView(cards);
        }

        public ICommand SaveCardCommand { get; }
        public async Task SaveCardAsync(object param)
        {
            var changesFinished = (bool)param;
            if (!changesFinished && SelectedCard != null)
            {
                var updateResourceCommand = new UpdateCardResourceCommand();
                await updateResourceCommand.ExecuteAsync(SelectedCard);
            }
        }

        public ICommand AddCardCommand { get; }
        public async Task AddCardAsync(object param)
        {
            if (SelectedCard == null) return;

            //TODO:
            var count = 0;
            var addCommand = new AddChildCardCommand();
            await addCommand.ExecuteAsync(SelectedCard.Id, $"Card {count}");

            if (RefreshCommand.CanExecute(null))
                RefreshCommand.Execute(null);
        }

        public ICommand RemoveCardCommand { get; }
        public async Task RemoveCardAsync(object param)
        {
            if (SelectedCard == null) return;

            var deleteCommand = new DeleteCardCommand();
            await deleteCommand.ExecuteAsync(SelectedCard.Id);

            if (RefreshCommand.CanExecute(null))
                RefreshCommand.Execute(null);
        }

        public ICommand AddResourceCommand { get; }
        public async Task AddResourceAsync(object param)
        {
            if (param is not ResourceBase resource) throw new ArgumentNullException(nameof(resource));
            if (SelectedCard is not Card card) return;

            var addResourceCommand = new AddCardResourceCommand();
            await addResourceCommand.ExecuteAsync(card.Id, resource.ResourceType, "Item 1", "Item 2", "TEST BODY");

            if (RefreshCommand.CanExecute(null))
                RefreshCommand.Execute(null);
        }

        public ICommand RemoveResourceCommand { get; }
        public async Task RemoveResourceAsync(object param)
        {
            if (param is not ResourceBase resource) throw new ArgumentNullException(nameof(param));

            var deleteCardResourceCommand = new DeleteCardResourceCommand();
            await deleteCardResourceCommand.ExecuteAsync(resource.Id);

            if (RefreshCommand.CanExecute(null))
                RefreshCommand.Execute(null);
        }

        public ICommand PreviewMouseUpCommand { get; }
        public async Task PreviewMouseUpAsync(object param)
        {
            if (param is not Card card) return;

            SelectedCard = card;
        }
        #endregion
    }
}