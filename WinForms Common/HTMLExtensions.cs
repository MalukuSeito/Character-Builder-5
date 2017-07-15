using OGL;
using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using OGL.Items;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;

namespace Character_Builder_Forms
{
    public delegate void LoadTransformEventHandler(XslCompiledTransform transform, IXML obj);
    public static class HTMLExtensions
    {
        public static FileInfo Transform_Items = new FileInfo("Items.xsl");
        public static FileInfo Transform_Skills = new FileInfo("Skills.xsl");
        public static FileInfo Transform_Languages = new FileInfo("Languages.xsl");
        public static FileInfo Transform_Features = new FileInfo("Features.xsl");
        public static FileInfo Transform_Backgrounds = new FileInfo("Backgrounds.xsl");
        public static FileInfo Transform_Classes = new FileInfo("Classes.xsl");
        public static FileInfo Transform_SubClasses = new FileInfo("SubClasses.xsl");
        public static FileInfo Transform_Races = new FileInfo("Races.xsl");
        public static FileInfo Transform_SubRaces = new FileInfo("SubRaces.xsl");
        public static FileInfo Transform_Spells = new FileInfo("Spells.xsl");
        public static FileInfo Transform_Magic = new FileInfo("Magic.xsl");
        public static FileInfo Transform_Conditions = new FileInfo("Conditions.xsl");
        public static FileInfo Transform_Possession = new FileInfo("Possession.xsl");
        public static FileInfo Transform_Description = new FileInfo("Descriptions.xsl");
        public static FileInfo Transform_Scroll = new FileInfo("Scroll.xsl");
        public static FileInfo Transform_RemoveDescription = new FileInfo("NoDescription.xsl");
        public static string Error(Exception ex) => "<html><body><b>Error generating output:</b><br>" + ex.Message + "<br>" + ex.InnerException + "<br>" + ex.StackTrace + "</body></html>";
        private static Dictionary<Type, XslCompiledTransform> Transforms = new Dictionary<Type, XslCompiledTransform>();
        private static XslCompiledTransform transform = new XslCompiledTransform();


        public static event LoadTransformEventHandler LoadTransform;

        public static void RemoveDescription(MemoryStream mem)
        {
            if (ConfigManager.Description) return;
            if (transform.OutputSettings == null) transform.Load(Transform_RemoveDescription.FullName);
            using (MemoryStream mem2 = new MemoryStream())
            {
                mem.Seek(0, SeekOrigin.Begin);
                XmlReader xr = XmlReader.Create(mem);
                using (XmlWriter xw = XmlWriter.Create(mem2))
                {
                    transform.Transform(xr, xw);
                    mem.SetLength(0);
                    mem2.Seek(0, SeekOrigin.Begin);
                    mem2.CopyTo(mem);
                }
            }
        }

        static HTMLExtensions()
        {
            LoadTransform += (t, o) => { if (o is Background) t.Load(Transform_Backgrounds.FullName); };
            LoadTransform += (t, o) => { if (o is Feature) t.Load(Transform_Features.FullName); };
            LoadTransform += (t, o) => { if (o is FeatureContainer) t.Load(Transform_Features.FullName); };
            LoadTransform += (t, o) => { if (o is ClassDefinition) t.Load(Transform_Classes.FullName); };
            LoadTransform += (t, o) => { if (o is Condition) t.Load(Transform_Conditions.FullName); };
            LoadTransform += (t, o) => { if (o is Description) t.Load(Transform_Description.FullName); };
            LoadTransform += (t, o) => { if (o is DescriptionContainer) t.Load(Transform_Description.FullName); };
            LoadTransform += (t, o) => { if (o is Item) t.Load(Transform_Items.FullName); };
            LoadTransform += (t, o) => { if (o is Language) t.Load(Transform_Languages.FullName); };
            LoadTransform += (t, o) => { if (o is MagicProperty) t.Load(Transform_Magic.FullName); };
            LoadTransform += (t, o) => { if (o is Race) t.Load(Transform_Races.FullName); };
            LoadTransform += (t, o) => { if (o is Skill) t.Load(Transform_Skills.FullName); };
            LoadTransform += (t, o) => { if (o is Spell) t.Load(Transform_Spells.FullName); };
            LoadTransform += (t, o) => { if (o is SubClass) t.Load(Transform_SubClasses.FullName); };
            LoadTransform += (t, o) => { if (o is SubRace) t.Load(Transform_SubRaces.FullName); };
        }

        private static XslCompiledTransform GetTransform(IXML t)
        {
            if (t == null) return null;
            if (Transforms.ContainsKey(t.GetType())) return Transforms[t.GetType()];
            XslCompiledTransform trans = new XslCompiledTransform();
            Transforms.Add(t.GetType(), trans);
            LoadTransform?.Invoke(trans, t);
            if (trans.OutputSettings == null) throw new Exception("No Transform found for " + t.GetType()); 
            return trans;
        }



        public static string ToHTML(this IXML obj)
        {
            try
            {
                if (obj == null) return null;
                XslCompiledTransform trans = GetTransform(obj);
                using (MemoryStream mem = obj.ToXMLStream())
                {
                    RemoveDescription(mem);
                    mem.Seek(0, SeekOrigin.Begin);
                    XmlReader xr = XmlReader.Create(mem);
                    using (StringWriter textWriter = new StringWriter())
                    {
                        using (XmlWriter xw = XmlWriter.Create(textWriter))
                        {
                            trans.Transform(xr, xw);
                            return textWriter.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
