using Avalonia.Controls;
using Echorium.Models;
using Echorium.Utils;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Echorium.ViewModels
{
    public class SearchViewVM : ViewModelBase
    {
        private SearchViewM _searchViewM;



        /// <summary>
        /// Full path to directory search
        /// </summary>
        public string SearchDirectory
        {
            get => _searchDirectory;
            set => this.RaiseAndSetIfChanged(ref _searchDirectory, value);
        }
        private string _searchDirectory;


        /// <summary>
        /// Collection of matching directories
        /// </summary>
        public ObservableCollection<FolderInfoM> FolderInfos { get; }



        public SearchViewVM()
        {
            _searchViewM = new SearchViewM();
            FolderInfos = new ObservableCollection<FolderInfoM>();
            MakeDummyInfo();
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


        private void MakeDummyInfo()
        {
            // TODO: Generate dummy models 4 view
        }
    }
}
