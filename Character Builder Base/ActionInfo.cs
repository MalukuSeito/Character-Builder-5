using OGL.Base;
using OGL.Features;

namespace Character_Builder
{
    public class ActionInfo
    {
        public ActionType Type { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string Text { get; set; }
        public Feature Feature { get; set; }
        public override string ToString()
        {
            return Name + " (" + Type + "): " + Text + " - " + Source;
        }
    }
}