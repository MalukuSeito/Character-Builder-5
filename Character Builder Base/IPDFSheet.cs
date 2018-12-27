using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public interface IPDFSheet: IDisposable
    {
        void Add(IPDFEditor sheet);
        Task AddBlankPages(string file);
    }
}
