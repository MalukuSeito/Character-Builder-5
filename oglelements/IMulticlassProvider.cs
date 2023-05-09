using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL
{
    public interface IMulticlassProvider
    {
        bool CanMulticlass(ClassDefinition c, int level);
    }
}
