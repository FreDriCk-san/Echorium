using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echorium.Models.TableItemM
{
    /// <summary>
    /// Higher information level (1) which contains info about current directory and matched files
    /// </summary>
    public class FolderInfoM
    {
        /// <summary>
        /// Current directory description
        /// </summary>
        public DirectoryInfo DirectoryDescription { get; }



        public FolderInfoM(DirectoryInfo aDirectoryDescription)
        {
            DirectoryDescription = aDirectoryDescription;
        }
    }
}
