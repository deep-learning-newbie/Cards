using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    public class Card : INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private ObservableCollection<ResourceBase> _resources;
        private bool _inEditMode;
        private List<Card> _childs;
        #endregion

        public int Id { get; set; }
        public string Title { get => _title; set { if (string.IsNullOrWhiteSpace(value)) throw new System.ArgumentNullException(nameof(Title)); _title = value; OnPropertyChanged(); } }
        public bool InEditMode { get => _inEditMode; set { _inEditMode = value; OnPropertyChanged();  } }
        public List<Card> Childs { get => _childs; set { _childs = new List<Card>(); OnPropertyChanged();} }
        public ObservableCollection<ResourceBase> Resources { get => _resources; set { _resources = value; OnPropertyChanged(); } }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}