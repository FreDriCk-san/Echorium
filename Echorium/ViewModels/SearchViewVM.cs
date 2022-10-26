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
        /// Loading status
        /// </summary>
        public LoadStatusEnum LoadStatusEnum
        {
            get => _loadStatusEnum;
            set => this.RaiseAndSetIfChanged(ref _loadStatusEnum, value);
        }
        private LoadStatusEnum _loadStatusEnum = LoadStatusEnum.None;



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
            // Очистка коллекции результата
            FolderInfos.Clear();

            // Если регулярное выражение отсутсвует или отсутствует директория для поиска, то ничего не делать
            if (string.IsNullOrEmpty(TextToSearch) || string.IsNullOrEmpty(SearchDirectory))
            {
                LoadStatusEnum = LoadStatusEnum.Canceled;
                return result;
            }

            // Если регулярное выражение не является валидным, то ничего не делать
            if (!TextToSearch.IsValidRegex())
            {
                LoadStatusEnum = LoadStatusEnum.InvalidSearchPattern;
                return result;
            }

            // Формирование экземпляра класса регулярного выражения
            // [на вход поступает строка регулярного выражения и ключ на остутствие регистрочувствительности]
            var regex = new Regex(TextToSearch, RegexOptions.IgnoreCase);

            // Формирование экземпляра класса "директории"
            var directoryInfo = new DirectoryInfo(SearchDirectory);
            // Если директории не существует, то ничего не делать
            if (!directoryInfo.Exists)
                return result;

            // Рекурсивный поиск всех файлов указанной директории
            var allFiles = await Task.Run(() =>
            {
                return FileHelper.GetAllFilesFromDirectory(directoryInfo);
            });

            // Если коллекция файлов пуста - то ничего не делать
            if (allFiles.IsNullOrEmpty())
                return result;

            // Итерация по всем файлам указанной директории
            for (int i = 0; i < allFiles.Count; i++)
            {
                // Экземпляр класса 'File'
                FileInfo? file = allFiles[i];
                // Обновление прогресса загрузки
                Progress = (i / (double)allFiles.Count) * 100;

                // Проверка, является ли текущий файл бинарным.
                // Если бинарный, то пропускаем итерацию
                if (await FileHelper.FileIsBinary(file.FullName))
                    continue;

                // Определение кодировки файла
                var encoding = await FileHelper.GetEncodingAsync(file.FullName);

                // Формирование потока считывания из файла
                using var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
                var counter = 0;
                List<WordInfoM> wordInfos = new();

                await Task.Run(() =>
                {
                    // Формирование экземпляра класса для считывания строк из файла
                    using var reader = new StreamReader(fileStream, encoding);
                    // Пока не конец файла - идёт считывание строк
                    while (!reader.EndOfStream)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        // TODO: FileStream props usage to calc progress

                        ++counter;
                        // Считывание строки
                        var line = reader.ReadLine();
                        // Если строка из файла не пустая и эта строка подходит под регулярное выражение, то оно добавляется в список результатов
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

                if (foundFolderVM.TryAddChild(fileInfoVM) && LoadStatusEnum != LoadStatusEnum.Completed)
                    LoadStatusEnum = LoadStatusEnum.Completed;
            }

            if (FolderInfos.Count == 0)
                LoadStatusEnum = LoadStatusEnum.NotFound;

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
