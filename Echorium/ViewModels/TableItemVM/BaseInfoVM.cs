using ReactiveUI;
using System.Collections.ObjectModel;

namespace Echorium.ViewModels.TableItemVM
{
    /// <summary>
    /// Base class of table item info
    /// </summary>
    public class BaseInfoVM : ViewModelBase
    {
        /// <summary>
        /// Is visible statement
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set => this.RaiseAndSetIfChanged(ref _isVisible, value);
        }
        private bool _isVisible = false;


        /// <summary>
        /// Is selected statement
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }
        private bool _isSelected = false;


        /// <summary>
        /// Is expanded statement
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
        }
        private bool _isExpanded = false;


        /// <summary>
        /// Parent item
        /// </summary>
        public BaseInfoVM Parent { get; }


        /// <summary>
        /// Children collection
        /// </summary>
        public ObservableCollection<BaseInfoVM> Children { get; }



        public BaseInfoVM(BaseInfoVM aParentVM)
        {
            Children = new ObservableCollection<BaseInfoVM>();
            Parent = aParentVM;
        }



        /// <summary>
        /// Try add child element
        /// </summary>
        /// <param name="aChild">Child element</param>
        /// <returns></returns>
        public bool TryAddChild(BaseInfoVM aChild)
        {
            if (aChild is null)
                return false;

            Children.Add(aChild);

            return true;
        }
    }
}
