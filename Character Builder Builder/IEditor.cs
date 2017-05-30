using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_Builder
{
    interface IEditor<T>
    {
        T edit(IHistoryManager history);
    }
}
