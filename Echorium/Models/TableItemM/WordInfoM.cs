using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echorium.Models.TableItemM
{
    /// <summary>
    /// Lower information level (3) which contains info about matched word
    /// </summary>
    public class WordInfoM
    {
        /// <summary>
        /// Word that has been matched
        /// </summary>
        public string WordMatch { get; }


        /// <summary>
        /// Line number of the matching word
        /// </summary>
        public int WordLine { get; }



        public WordInfoM(string aWordMatch, int aWordLine)
        {
            WordMatch = aWordMatch;
            WordLine = aWordLine;
        }
    }
}
