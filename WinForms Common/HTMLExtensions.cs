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

        public static string Error(Exception ex) => "<html><body><b>Error generating output:</b><br>" + ex.Message + "<br>" + ex.InnerException + "<br>" + ex.StackTrace + "</body></html>";
        private static Dictionary<Type, XslCompiledTransform> Transforms = new Dictionary<Type, XslCompiledTransform>();
        private static XslCompiledTransform transform = new XslCompiledTransform();


        public static event LoadTransformEventHandler LoadTransform;

        public static void RemoveDescription(MemoryStream mem)
        {
            if (ConfigManager.Description) return;
            if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_RemoveDescription.FullName);
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
            LoadTransform += (t, o) => { if (o is Background) t.Load(ConfigManager.Transform_Backgrounds.FullName); };
            LoadTransform += (t, o) => { if (o is Feature) t.Load(ConfigManager.Transform_Features.FullName); };
            LoadTransform += (t, o) => { if (o is FeatureContainer) t.Load(ConfigManager.Transform_Features.FullName); };
            LoadTransform += (t, o) => { if (o is ClassDefinition) t.Load(ConfigManager.Transform_Classes.FullName); };
            LoadTransform += (t, o) => { if (o is Condition) t.Load(ConfigManager.Transform_Conditions.FullName); };
            LoadTransform += (t, o) => { if (o is Description) t.Load(ConfigManager.Transform_Description.FullName); };
            LoadTransform += (t, o) => { if (o is DescriptionContainer) t.Load(ConfigManager.Transform_Description.FullName); };
            LoadTransform += (t, o) => { if (o is Item) t.Load(ConfigManager.Transform_Items.FullName); };
            LoadTransform += (t, o) => { if (o is Language) t.Load(ConfigManager.Transform_Languages.FullName); };
            LoadTransform += (t, o) => { if (o is MagicProperty) t.Load(ConfigManager.Transform_Magic.FullName); };
            LoadTransform += (t, o) => { if (o is Race) t.Load(ConfigManager.Transform_Races.FullName); };
            LoadTransform += (t, o) => { if (o is Skill) t.Load(ConfigManager.Transform_Skills.FullName); };
            LoadTransform += (t, o) => { if (o is Spell) t.Load(ConfigManager.Transform_Spells.FullName); };
            LoadTransform += (t, o) => { if (o is SubClass) t.Load(ConfigManager.Transform_SubClasses.FullName); };
            LoadTransform += (t, o) => { if (o is SubRace) t.Load(ConfigManager.Transform_SubRaces.FullName); };
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
