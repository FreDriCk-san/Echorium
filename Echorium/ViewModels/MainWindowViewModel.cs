namespace Echorium.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Main window title content
        /// </summary>
        public static string MainWindowTitle => $"{System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Name}" +
            $" v.{System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version}";

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
