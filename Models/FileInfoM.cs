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
    /// Middle information level (2) which contains info about current file and matched words
    /// </summary>
    public class FileInfoM
    {
        /// <summary>
        /// Collection of matching words information
        /// </summary>
        public ObservableCollection<WordInfoM> WordsInfos { get; }


        /// <summary>
        /// Current file description
        /// </summary>
        public FileInfo FileDescription { get; }



        public FileInfoM(FileInfo aFileDescription)
        {
            WordsInfos = new ObservableCollection<WordInfoM>();
            FileDescription = aFileDescription;
        }



        public bool TryAddWordInfo(WordInfoM aWordInfoModel)
        {
            if (aWordInfoModel is null)
                return false;

            WordsInfos.Add(aWordInfoModel);

            return true;
        }
    }
}
