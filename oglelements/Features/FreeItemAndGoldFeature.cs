using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OGL.Features
{
    public class FreeItemAndGoldFeature: Feature
    {
        public List<String> Items { get; set; }
        public int CP { get; set; }
        public int SP { get; set; }
        public int GP { get; set; }
        public FreeItemAndGoldFeature()
            : base(null, null, 1, true)
        {
            Action = Base.ActionType.ForceHidden;
            Items = new List<string>();
            CP = 0;
            SP = 0;
            GP = 0;
        }
        public FreeItemAndGoldFeature(string name, string text, int cp=0, int sp=0, int gp=0, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            Items = new List<string>();
            CP = cp;
            SP = sp;
            GP = gp;         
        }
        public FreeItemAndGoldFeature(string name, string text, Item item, int cp = 0, int sp = 0, int gp = 0, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            Items = new List<string>();
            Items.Add(item.Name);
            CP = cp;
            SP = sp;
            GP = gp;      
        }
        public FreeItemAndGoldFeature(string name, string text, Item item, Item item2, int cp = 0, int sp = 0, int gp = 0, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            Items = new List<string>();
            Items.Add(item.Name);
            Items.Add(item2.Name);
            CP = cp;
            SP = sp;
            GP = gp;      
        }
        public FreeItemAndGoldFeature(string name, string text, Item item, Item item2, Item item3, int cp = 0, int sp = 0, int gp = 0, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            Items = new List<string>();
            Items.Add(item.Name);
            Items.Add(item2.Name);
            Items.Add(item3.Name);
            CP = cp;
            SP = sp;
            GP = gp;      
        }
        public FreeItemAndGoldFeature(string name, string text, Item item, Item item2, Item item3, Item item4, int cp = 0, int sp = 0, int gp = 0, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            Items = new List<string>();
            Items.Add(item.Name);
            Items.Add(item2.Name);
            Items.Add(item3.Name);
            Items.Add(item4.Name);
            CP = cp;
            SP = sp;
            GP = gp;      
        }
        public virtual IEnumerable<Item> getItems(IChoiceProvider provider, OGLContext context)
        {
            return (from i in Items select context.GetItem(i, Source));
        }
        public override string Displayname()
        {
            return "Free Item / Gold Feature";
        }
    }
}
