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
    /// Middle information level (2) which contains info about current file and matched words
    /// </summary>
    public class FileInfoM
    {
        /// <summary>
        /// Current file description
        /// </summary>
        public FileInfo FileDescription { get; }



        public FileInfoM(FileInfo aFileDescription)
        {
            FileDescription = aFileDescription;
        }
    }
}
