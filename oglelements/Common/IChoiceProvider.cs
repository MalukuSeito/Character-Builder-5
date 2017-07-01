using OGL.Base;
using OGL.Features;
using System;
using System.Collections.Generic;

namespace OGL.Common
{
    public interface IChoiceProvider
    {
        int GetChoiceOffset(Feature f, string uniqueID, int amount);
        int GetChoiceTotal(string uniqueID);
        Choice GetChoice(String ID);
        bool CanMulticlass(ClassDefinition c, int level);
        bool Matches(String expression, List<string> additionalKeywords = null, int classlevel = 0, int level = 0);
    }
}
