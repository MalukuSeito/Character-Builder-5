using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class Price
    {
        public int cp {get; set;}
        public int sp {get; set;}
        public int ep { get; set; }
        public int gp {get; set;}
        public int pp {get; set;}
        
        public Price()
        {
            cp = 0;
            sp = 0;
            gp = 0;
            pp = 0;
            ep = 0;
        }
        public Price(int copper, int silver, int gold)
        {
            cp = copper;
            sp = silver;
            gp = gold;
            pp = 0;
            ep = 0;
        }
        public Price(Price p, int multiplier)
        {
            cp = p.cp * multiplier;
            sp = p.sp * multiplier;
            gp = p.gp * multiplier;
            pp = p.pp * multiplier;
            ep = p.ep * multiplier;
        }
        public override string ToString()
        {
            String r = "";
            if (pp != 0) r += pp + " pp";
            if (gp != 0)
            {
                if (r.Length > 0) r += ", ";
                r += gp + " gp";
            }
            if (ep != 0)
            {
                if (r.Length > 0) r += ", ";
                r += ep + " ep";
            }
            if (sp != 0)
            {
                if (r.Length > 0) r += ", ";
                r += sp + " sp";
            }
            if (cp != 0)
            {
                if (r.Length > 0) r += ", ";
                r += cp + " cp";
            }
            return r;
        }
        public double Weight()
        {
            return ConfigManager.loaded.WeightOfACoin * (pp + gp + ep + sp + cp);
        }
    }
}
