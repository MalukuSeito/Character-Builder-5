using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL.Features
{
    public class FormsCompanionsFeature: Feature
    {
        public FormsCompanionsFeature()
        {
            Action = Base.ActionType.ForceHidden;
        }
        public string DisplayName { get; set; }
        public string UniqueID { get; set; }
        public string FormsCompanionsOptions { get; set; }
        public int FormsCompanionsCount { get; set; }
        public override string Displayname()
        {
            return "Forms/Companions Feature";
        }
    }
}
