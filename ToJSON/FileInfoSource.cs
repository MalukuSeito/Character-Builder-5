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

        public Stream GetReader()
        {
            if (Archive != null)
            {
                return Archive.Open();
            }
            else
            {
                return new FileStream(File.FullName, FileMode.Open);
            }
        }
    }
}
