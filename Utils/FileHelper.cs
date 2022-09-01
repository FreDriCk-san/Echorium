using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Echorium.Utils
{
    public static class FileHelper
    {
        /// <summary>
        /// Get all inner files of current directory
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public static List<FileInfo> GetAllFilesFromDirectory(DirectoryInfo directoryInfo)
        {
            List<FileInfo> files = new ();

            if (directoryInfo is null)
                return files;

            try
            {
                files.AddRange(directoryInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly));

                foreach (var directory in directoryInfo.GetDirectories())
                    files.AddRange(GetAllFilesFromDirectory(directory));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return files;
        }


        /// <summary>
        /// Check if file is binary
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileIsBinary(string path)
        {
            byte[] buff = new byte[1024];
            int size;
            try
            {
                FileStream reader = new (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                size = reader.Read(buff, 0, buff.Length);
                reader.Close();
            }
            catch
            {
                return true;
            }

            if (size > 0)
            {
                for (int i = 0; i < size; ++i)
                {
                    if (buff[i] == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return Encoding.ASCII;
        }
    }
}
