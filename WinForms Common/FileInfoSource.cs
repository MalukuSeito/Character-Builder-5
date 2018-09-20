using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Character_Builder_Forms
{
    public class FileInfoSource
    {
        public FileInfo File { get; set; }
        public ZipArchiveEntry Archive { get; set; }
        public String FullName { get; set; }
        public String DirectoryName { get => Path.GetDirectoryName(FullName); }
        public string Source { get; set; }

        public TextReader GetReader()
        {
            if (Archive != null)
            {
                return new StreamReader(Archive.Open());
            }
            else
            {
                return new StreamReader(File.FullName);
            }
        }
    }
}
