using System.ComponentModel;
using System.Windows.Data;
using Models;
using System.Collections.Generic;
using Commands;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MainApp.ViewModels
{
    public class CardsViewModel : ViewModelBase
    {
        #region attributes
        private Card _seletcedItem;
        #endregion

        public CardsViewModel(List<Card> cards) : this()
        {
            //TODO: uncomment this line
            //Cards = new ListCollectionView(cards);
        }

        #region for test purpose only 
        public CardsViewModel()
        {
            var cards = new List<Card>();
            //cards.Add(new Card()
            //{
            //    Id = 1,
            //    InEditMode = false,
            //    Title = "Card1",
            //    Resources = new ObservableCollection<ResourceBase>()
            //    {
            //        new ImageResource() { Index=1, Description = "some text", Uri = "Image1" },
            //    },
            //    Childs = new List<Card>()
            //    {
            //        new Card()
            //        {
            //            Id = 11, InEditMode = false, Title = "Card1.1" ,
            //            Resources = new ObservableCollection<ResourceBase>()
            //            {
            //                new TableResource() { Index=1,Rows = new List<string>(){"Item 1", "Item 2" , "Item 3"}},
            //                new TableResource() { Index=2,Rows = new List<string>(){ "Item 1", "Item 2" , "Item 3" }},
            //                new ImageResource() { Index=3, Description = "some text", Uri = "340719-200.png" }
            //            }
            //        }
            //    }
            //});


            var card2 = new Card()
            {
                Id = 2,
                InEditMode = true,
                Title = "Card 1",
                Resources = new ObservableCollection<ResourceBase>()
                {
                    new ImageResource() { Index=1, Description = "some text", Uri = "Image1" },
                },
                Childs = new List<Card>()
            };
            card2.Childs.Add(

                new Card()
                {
                    Id = 21,
                    InEditMode = false,
                    Title = "Card 1.1",
                    Resources = new ObservableCollection<ResourceBase>()
                    {
                        new TableResource() { Index=1, Rows = new List<TableResourceItem>(){ new TableResourceItem(){Column1 ="Item 1", Column2 = "Item 2"},  new TableResourceItem(){Column1 ="Item 1", Column2 = "Item 2"}} },
                        new TableResource() { Index=2, Rows = new List<TableResourceItem>(){ new TableResourceItem(){Column1 ="Item 1", Column2 = "Item 2"}, new TableResourceItem(){Column1 ="Item 1", Column2 = "Item 2"}} },
                        new ImageResource() { Index=3, Description = "some text", Uri = "340719-200.png" }
                    }
                });
            cards.Add(card2);
            Cards = new ListCollectionView(cards);
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
        public ICollectionView Cards { get; set; }

        public async Task DeleteAsync()
        {
            if (SelectedItem == null)
                return;

            var deleteCommand = new DeleteCommand();
            await deleteCommand.ExecuteAsync(SelectedItem.Id);
        }
    }
}