using Avalonia.Controls;
using Echorium.Models;
using Echorium.Models.TableItemM;
using Echorium.Utils;
using Echorium.ViewModels.TableItemVM;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Echorium.ViewModels
{
    public class SearchViewVM : ViewModelBase
    {
        private SearchViewM _searchViewM { get; }
        private int _debounceSearchTime = 800;
        private CancellationStorage _searchCancellationStorage;


        /// <summary>
        /// Full path to directory search
        /// </summary>
        public string SearchDirectory
        {
            get => _searchDirectory;
            set => this.RaiseAndSetIfChanged(ref _searchDirectory, value);
        }
        private string _searchDirectory = "";


        /// <summary>
        /// Text to search at directory
        /// </summary>
        public string TextToSearch
        {
            get => _textToSearch;
            set => this.RaiseAndSetIfChanged(ref _textToSearch, value);
        }
        private string _textToSearch = "";


        /// <summary>
        /// Loading progress
        /// </summary>
        public double Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
        }
        private double _progress;



        /// <summary>
        /// Collection of matching directories
        /// </summary>
        public ObservableCollection<FolderInfoVM> FolderInfos { get; }



        public SearchViewVM()
        {
            _searchViewM = new ();
            FolderInfos = new ();
            _searchCancellationStorage = new ();

            this.PropertyChanged += SearchViewVM_PropertyChanged;

            //MakeDummyInfo();
            //
        }


        private async void SearchViewVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TextToSearch):
                case nameof(SearchDirectory):
                    await TrySearchForMatchesAsync();
                    Progress = 0;
                    break;
            }
        }


        private async Task<bool> TrySearchForMatchesAsync()
        {
            using var cancellation = _searchCancellationStorage.Refresh();
            var token = cancellation.Token;

            try
            {
                if (_debounceSearchTime > 0)
                    await Task.Delay(_debounceSearchTime);
                token.ThrowIfCancellationRequested();

                return await SearchForMatchesAsync(token);
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }


        private async Task<bool> SearchForMatchesAsync(CancellationToken cancellationToken)
        {
            bool result = false;
            FolderInfos.Clear();
            if (string.IsNullOrEmpty(TextToSearch) || string.IsNullOrEmpty(SearchDirectory) || !TextToSearch.IsValidRegex())
                return result;

            var regex = new Regex(TextToSearch, RegexOptions.IgnoreCase);

            var directoryInfo = new DirectoryInfo(SearchDirectory);
            if (!directoryInfo.Exists)
                return result;

            var allFiles = await Task.Run(() =>
            {
                return FileHelper.GetAllFilesFromDirectory(directoryInfo);
            });

            if (allFiles.IsNullOrEmpty())
                return result;

            for (int i = 0; i < allFiles.Count; i++)
            {
                FileInfo? file = allFiles[i];
                Progress = (i / (double)allFiles.Count) * 100;

                if (await FileHelper.FileIsBinary(file.FullName))
                    continue;

                // TODO: Not sure about manual encoding
                var encoding = await FileHelper.GetEncodingAsync(file.FullName);

                using var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
                var counter = 0;
                List<WordInfoM> wordInfos = new();

                await Task.Run(() =>
                {
                    using var reader = new StreamReader(fileStream, encoding);
                    while (!reader.EndOfStream)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        // TODO: FileStream props usage to calc progress

                        ++counter;
                        var line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line) && regex.Match(line).Success)
                            wordInfos.Add(new WordInfoM(line, counter));
                    }
                }, cancellationToken);

                var firstWordInfo = wordInfos.FirstOrDefault();
                if (firstWordInfo is null)
                    continue;

                var foundFolderVM = FolderInfos.FirstOrDefault(f => f.FolderName == file?.Directory?.FullName);
                if (foundFolderVM is null)
                {
                    var folderInfoM = new FolderInfoM(file.Directory!);
                    foundFolderVM = new(folderInfoM);
                    FolderInfos.Add(foundFolderVM);
                }

                var fileInfoM = new FileInfoM(file);
                var fileInfoVM = new FileInfoVM(foundFolderVM, fileInfoM);

                foreach (var wordInfo in wordInfos)
                {
                    WordInfoVM wordInfoVM = new(fileInfoVM, wordInfo);
                    fileInfoVM.TryAddChild(wordInfoVM);
                }

                foundFolderVM.TryAddChild(fileInfoVM);
            }

            return result;
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
                FolderInfoM folderInfoM = new(new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory()));
                FolderInfoVM folderInfoVM = new(folderInfoM);

                for (int j = 0; j < 5; ++j)
                {
                    FileInfoM fileInfoM = new(folderInfoM.DirectoryDescription.GetFiles()[0]);
                    FileInfoVM fileInfoVM = new(folderInfoVM, fileInfoM);

                    for (int k = 0; k < 6; ++k)
                    {
                        WordInfoM wordInfoM = new(Guid.NewGuid().ToString(), k);
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
