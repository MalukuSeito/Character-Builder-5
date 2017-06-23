using System;

namespace OGL.Base
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
        public string toGold()
        {
            double res = gp + pp * 10.0 + sp / 10.0 + cp / 100.0 + ep / 2.0;
            return String.Format("{0:0.00}", res);
        }
        public double Weight()
        {
            return ConfigManager.loaded.WeightOfACoin * (pp + gp + ep + sp + cp);
        }
    }
}
