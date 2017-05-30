using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_Builder
{
    public delegate void HistoryButtonChangeEvent(object sender, bool CanUndo, bool CanRedo);
    public interface IHistoryManager
    {
        void MakeHistory(string id);
        bool Undo();
        bool Redo();
        bool CanUndo();
        bool CanRedo();
        event HistoryButtonChangeEvent ButtonChange;
    }
}
