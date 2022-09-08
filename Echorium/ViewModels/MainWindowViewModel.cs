namespace Echorium.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// View-Model of "Search" user control
        /// </summary>
        public SearchViewVM SearchViewVM { get; set; }


        public MainWindowViewModel()
        {
            SearchViewVM = new SearchViewVM();
        }
    }
}
