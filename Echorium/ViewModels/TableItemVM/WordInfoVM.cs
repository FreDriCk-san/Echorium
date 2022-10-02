using Echorium.Models.TableItemM;

namespace Echorium.ViewModels.TableItemVM
{
    public class WordInfoVM : BaseInfoVM
    {
        private WordInfoM _wordInfoModel { get; }


        public string WordText 
            => GetWordText() ?? "Not initialized";

        public string WordLine 
            => _wordInfoModel?.WordLine.ToString() ?? "Not initialized";



        public WordInfoVM(FileInfoVM aFileInfoVM, WordInfoM aWordInfoModel) : base(aFileInfoVM)
        {
            _wordInfoModel = aWordInfoModel;
        }


        private string? GetWordText(int wordLength = 80)
        {
            // TODO: Somehow resolve treeview stuck on big data
            if (_wordInfoModel?.WordMatch is null || wordLength < 0)
                return null;

            return _wordInfoModel.WordMatch.Length > wordLength 
                ? $"{_wordInfoModel.WordMatch[..(wordLength - 1)]} ...." 
                : _wordInfoModel.WordMatch;
        }
    }
}
