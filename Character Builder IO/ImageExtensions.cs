using OGL;
using SixLabors.ImageSharp;
using System.IO;

namespace Character_Builder_IO
{
    public static class ImageExtensions
    {
        public static Image GetImage(this Background o) 
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this Background o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Image GetImage(this ClassDefinition o)
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this ClassDefinition o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Image GetImage(this Condition o)
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this Condition o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Image GetImage(this Race o)
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this Race o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Image GetImage(this SubRace o)
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this SubRace o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }
        public static Image GetImage(this SubClass o)
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this SubClass o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }

        public static Image GetImage(this MagicProperty o)
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this MagicProperty o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }

        public static Image GetImage(this Item o)
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this Item o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }

        public static Image GetImage(this Language o)
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this Language o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }

        public static Image GetImage(this Monster o)
        {
            if (o.ImageData == null) return null;
            else return Image.Load(o.ImageData);
        }
        public static void SetImage(this Monster o, Image value)
        {
            if (value == null) o.ImageData = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.SaveAsPng(ms);
                    o.ImageData = ms.ToArray();
                }
        }
    }
}
