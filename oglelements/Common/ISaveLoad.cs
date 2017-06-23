using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL.Common
{
    public interface ISaveLoad
    {
        byte[] LoadBytes(string path);
        string LoadText(string path);
        void SaveFile(string path, byte[] data);
        void SaveFile(string path, string data);
        bool Exists(string path);
    }
}
