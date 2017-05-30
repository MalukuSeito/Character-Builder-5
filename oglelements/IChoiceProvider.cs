using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public interface IChoiceProvider
    {
        int getChoiceOffset(Feature f, string uniqueID, int amount);
        int getChoiceTotal(string uniqueID);
        Choice getChoice(String ID);
        bool canMulticlass(ClassDefinition c, int level);
        bool matches(String expression, List<string> additionalKeywords = null, int classlevel = 0, int level = 0);
    }
}
