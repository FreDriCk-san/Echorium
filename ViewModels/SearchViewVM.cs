using Avalonia.Controls;
using Echorium.Models;
using Echorium.Utils;
using ReactiveUI;
using System.Threading.Tasks;

namespace Echorium.ViewModels
{
    public class SearchViewVM : ViewModelBase
    {
        #region Private props
        private SearchViewM _searchViewM;
        #endregion Private props


        #region Public props
        /// <summary>
        /// Full path to directory search
        /// </summary>
        public string SearchDirectory
        {
            get => _searchDirectory;
            set => this.RaiseAndSetIfChanged(ref _searchDirectory, value);
        }
        private string _searchDirectory;
        #endregion Public props


        public SearchViewVM()
        {
            _searchViewM = new SearchViewM();
        }


        /// <summary>
        /// Call folder dialog and set search directory
        /// </summary>
        /// <returns></returns>
        public async Task CallFolderDialogAsync()
        {
            if (WindowsManager.MainWindowRef is null)
                return;

            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync(WindowsManager.MainWindowRef);

            if (!string.IsNullOrEmpty(result))
                SearchDirectory = result;
        }
    }
}
