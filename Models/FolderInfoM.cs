using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echorium.Models
{
    /// <summary>
    /// Higher information level (1) which contains info about current directory and matched files
    /// </summary>
    public class FolderInfoM
    {
        /// <summary>
        /// Collection of matching files description
        /// </summary>
        public ObservableCollection<FileInfoM> FileInfos { get; }


        /// <summary>
        /// Current directory description
        /// </summary>
        public DirectoryInfo DirectoryDescription { get; }



        public FolderInfoM(DirectoryInfo aDirectoryDescription)
        {
            FileInfos = new ObservableCollection<FileInfoM>();
            DirectoryDescription = aDirectoryDescription;
        }



        public bool TryAddFileInfo(FileInfoM aFileInfoModel)
        {
            if (aFileInfoModel is null)
                return false;

            FileInfos.Add(aFileInfoModel);

            return true;
        }
    }
}
