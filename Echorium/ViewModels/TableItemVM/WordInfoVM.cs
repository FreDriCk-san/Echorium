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


        private string? GetWordText()
        {
            // TODO: Somehow resolve treeview stuck on big data
            if (_wordInfoModel?.WordMatch is null)
                return null;

            return _wordInfoModel.WordMatch.Length > 60 
                ? $"{_wordInfoModel.WordMatch.Substring(0, 59)} [TOO LONG TEXT!!!!]" 
                : _wordInfoModel.WordMatch;
        }
    }
}
