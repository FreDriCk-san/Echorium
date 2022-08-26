using Echorium.Models.TableItemM;

namespace Echorium.ViewModels.TableItemVM
{
    public class FolderInfoVM : BaseInfoVM
    {
        private FolderInfoM _folderInfoModel;


        public string FolderName 
            => _folderInfoModel?.DirectoryDescription?.FullName ?? "Not initialized";



        public FolderInfoVM(FolderInfoM aFolderInfoModel) : base(null)
        {
            _folderInfoModel = aFolderInfoModel;
        }
    }
}
