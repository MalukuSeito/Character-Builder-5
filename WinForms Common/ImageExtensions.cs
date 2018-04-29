using OGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_Forms
{
    public static class ImageExtensions
    {
        public static Bitmap GetImage(this Background o) 
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this Background o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Bitmap GetImage(this ClassDefinition o)
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this ClassDefinition o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Bitmap GetImage(this Condition o)
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this Condition o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Bitmap GetImage(this Race o)
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this Race o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Bitmap GetImage(this SubRace o)
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this SubRace o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Bitmap GetImage(this SubClass o)
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this SubClass o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }

        public static Bitmap GetImage(this MagicProperty o)
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this MagicProperty o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }

        public static Bitmap GetImage(this Item o)
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this Item o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }

        public static Bitmap GetImage(this Language o)
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this Language o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }

        public static Bitmap GetImage(this Monster o)
        {
            if (o.ImageData == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.ImageData)) return new Bitmap(ms);
        }
        public static void SetImage(this Monster o, Bitmap value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.ImageData = ms.ToArray();
                }
        }
    }
}
