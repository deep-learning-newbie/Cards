//using MainApp.Helpers;
//using Models;
//using System.Collections.Generic;

//namespace MainApp.ViewModels
//{
//    public class CardViewModel : ViewModelBase
//    {
//        private int id;
//        private string name;
//        private List<CardViewModel> childs;

//        public CardViewModel(Card model)
//        {
//            this.id = model.Id;
//            this.name = model.Name;
//            this.childs = MapperWrapper.Mapper.Map<List<CardViewModel>>(model.Childs);
//        }

//        public int Id
//        {
//            set
//            {
//                id = value;
//                OnPropertyChanged();
//            }
//            get { return id; }
//        }

//        public string Name
//        {
//            set
//            {
//                name = value;
//                OnPropertyChanged();
//            }
//            get { return name; }
//        }

//        public List<CardViewModel> Childs
//        {
//            set
//            {
//                childs = value;
//                OnPropertyChanged();
//            }
//            get { return childs; }
//        }
//    }
//}
