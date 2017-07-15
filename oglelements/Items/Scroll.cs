using OGL.Base;

namespace OGL.Items
{
    public class Scroll: Item
    {
        public Spell Spell;
        public Scroll()
        {

        }
        public Scroll(Spell spell) : base()
        {
            Spell = spell;
            Name = spell.Name;
            Price = new Price();
            Weight = 0;
            
        }
        public override string ToString()
        {
            return "Scroll of " + Name;
        }
    }
}
