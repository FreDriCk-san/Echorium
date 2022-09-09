using Echorium.Models.TableItemM;

namespace Echorium.ViewModels.TableItemVM
{
    public class FileInfoVM : BaseInfoVM
    {
        private FileInfoM _fileInfoModel;


        public string FileName 
            => _fileInfoModel?.FileDescription?.Name ?? "Not initialized";



        public FileInfoVM(FolderInfoVM aFolderInfoVM, FileInfoM aFileInfoModel) : base(aFolderInfoVM)
        {
            _fileInfoModel = aFileInfoModel;
        }
    }
}
