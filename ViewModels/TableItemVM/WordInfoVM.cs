using Echorium.Models.TableItemM;

namespace Echorium.ViewModels.TableItemVM
{
    public class WordInfoVM : BaseInfoVM
    {
        private WordInfoM _wordInfoModel { get; }


        public string WordText 
            => _wordInfoModel?.WordMatch ?? "Not initialized";

        public string WordLine 
            => _wordInfoModel?.WordLine.ToString() ?? "Not initialized";



        public WordInfoVM(FileInfoVM aFileInfoVM, WordInfoM aWordInfoModel) : base(aFileInfoVM)
        {
            _wordInfoModel = aWordInfoModel;
        }
    }
}
