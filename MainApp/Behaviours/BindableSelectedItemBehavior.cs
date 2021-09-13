using MainApp.ViewModels;
using Microsoft.Xaml.Behaviors;
using Models;
using System.Windows;
using System.Windows.Controls;


namespace MainApp.Behaviours
{
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        #region SelectedItem Property
        public Card SelectedItem
        {
            get { return (Card)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(Card), typeof(BindableSelectedItemBehavior), new UIPropertyMetadata(null));
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is not Card card) return;
            SelectedItem = card;

            if (sender is not TreeView treeView) return;
            if (treeView.DataContext is not CardsViewModel viewModel) return;

            viewModel.SelectedCard = SelectedItem;
        }
    }
}
