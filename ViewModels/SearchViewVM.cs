using Avalonia.Controls;
using Echorium.Models;
using Echorium.Models.TableItemM;
using Echorium.Utils;
using Echorium.ViewModels.TableItemVM;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Echorium.ViewModels
{
    public class SearchViewVM : ViewModelBase
    {
        private SearchViewM _searchViewM { get; }



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
        public ObservableCollection<FolderInfoVM> FolderInfos { get; }



        public SearchViewVM()
        {
            _searchViewM = new SearchViewM();
            FolderInfos = new ObservableCollection<FolderInfoVM>();
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


        /// <summary>
        /// Create dummy info for testing
        /// </summary>
        private void MakeDummyInfo()
        {
            for (int i = 0; i < 4; ++i)
            {
                FolderInfoM folderInfoM = new(new System.IO.DirectoryInfo(@"D:\Sources\WS-9195"));
                FolderInfoVM folderInfoVM = new(folderInfoM);

                for (int j = 0; j < 5; ++j)
                {
                    FileInfoM fileInfoM = new(folderInfoM.DirectoryDescription.GetFiles()[0]);
                    FileInfoVM fileInfoVM = new(folderInfoVM, fileInfoM);

                    for (int k = 0; k < 6; ++k)
                    {
                        WordInfoM wordInfoM = new(Guid.NewGuid().ToString(), Convert.ToUInt32(k));
                        WordInfoVM wordInfoVM = new(fileInfoVM, wordInfoM);

                        fileInfoVM.TryAddChild(wordInfoVM);
                    }

                    folderInfoVM.TryAddChild(fileInfoVM);
                }

                FolderInfos.Add(folderInfoVM);
            }
        }
    }
}
