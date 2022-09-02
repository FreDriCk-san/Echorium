using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        public static Task<bool> FileIsBinary(string path)
        {
            return Task.Run<bool>(() =>
            {
                try
                {
                    byte[] buff = new byte[256];
                    int size;

                    FileStream reader = new(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 256);
                    size = reader.Read(buff);
                    reader.Close();
                    reader.Dispose();

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
                catch (Exception)
                {
                    return false;
                }
            });
        }


        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static async Task<Encoding> GetEncodingAsync(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize:4, useAsync:true))
            {
                await file.ReadAsync(bom, 0, 4);
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
