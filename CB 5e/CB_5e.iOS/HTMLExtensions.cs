using CB_5e;
using CB_5e.ViewModels;
using CB_5e.ViewModels.Character.ChoiceOptions;
using Character_Builder;
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

namespace CB_5e.iOS
{
    public delegate void LoadTransformEventHandler(XslCompiledTransform transform, IXML obj);
    public static class HTMLExtensions
    {
        public static string Transform_Items = "Items.xsl";
        public static string Transform_Skills = "Skills.xsl";
        public static string Transform_Languages = "Languages.xsl";
        public static string Transform_Features = "Features.xsl";
        public static string Transform_Backgrounds = "Backgrounds.xsl";
        public static string Transform_Classes = "Classes.xsl";
        public static string Transform_SubClasses = "SubClasses.xsl";
        public static string Transform_Races = "Races.xsl";
        public static string Transform_SubRaces = "SubRaces.xsl";
        public static string Transform_Spells = "Spells.xsl";
        public static string Transform_Magic = "Magic.xsl";
        public static string Transform_Conditions = "Conditions.xsl";
        public static string Transform_Possession = "Possession.xsl";
        public static string Transform_Description = "Descriptions.xsl";
        public static string Transform_Scroll = "Scroll.xsl";
        public static string Transform_RemoveDescription = "NoDescription.xsl";
        public static string Error(Exception ex) => "<html><body><b>Error generating output:</b><br>" + ex.Message + "<br>" + ex.InnerException + "<br>" + ex.StackTrace + "</body></html>";
        public static Dictionary<Type, XslCompiledTransform> Transforms = new Dictionary<Type, XslCompiledTransform>();
        public static XslCompiledTransform Transform = new XslCompiledTransform();
        public static ConfigManager Config;

        public static event LoadTransformEventHandler LoadTransform;

        public static void RemoveDescription(MemoryStream mem)
        {
            if (ConfigManager.Description) return;
            if (Transform.OutputSettings == null) Transform.Load(App.Storage.Path + "/Data/" + Config.RemoveDescription_Transform);
            using (MemoryStream mem2 = new MemoryStream())
            {
                mem.Seek(0, SeekOrigin.Begin);
                XmlReader xr = XmlReader.Create(mem);
                using (XmlWriter xw = XmlWriter.Create(mem2))
                {
                    Transform.Transform(xr, xw);
                    mem.SetLength(0);
                    mem2.Seek(0, SeekOrigin.Begin);
                    mem2.CopyTo(mem);
                }
            }
        }

        static HTMLExtensions()
        {
            LoadTransform += (t, o) => { if (o is Background) t.Load(App.Storage.Path + "/Data/" + Config.Backgrounds_Transform); };
            LoadTransform += (t, o) => { if (o is Feature) t.Load(App.Storage.Path + "/Data/" + Config.Features_Transform); };
            LoadTransform += (t, o) => { if (o is TableValue) t.Load(App.Storage.Path + "/Data/" + Config.Features_Transform); };
            LoadTransform += (t, o) => { if (o is AbilityChoice) t.Load(App.Storage.Path + "/Data/" + Config.Features_Transform); };
            LoadTransform += (t, o) => { if (o is FeatureContainer) t.Load(App.Storage.Path + "/Data/" + Config.Features_Transform); };
            LoadTransform += (t, o) => { if (o is ClassDefinition) t.Load(App.Storage.Path + "/Data/" + Config.Classes_Transform); };
            LoadTransform += (t, o) => { if (o is Condition) t.Load(App.Storage.Path + "/Data/" + Config.Conditions_Transform); };
            LoadTransform += (t, o) => { if (o is Description) t.Load(App.Storage.Path + "/Data/" + Transform_Description); };
            LoadTransform += (t, o) => { if (o is DescriptionContainer) t.Load(App.Storage.Path + "/Data/" + Transform_Description); };
            LoadTransform += (t, o) => { if (o is Item) t.Load(App.Storage.Path + "/Data/" + Config.Items_Transform); };
            LoadTransform += (t, o) => { if (o is Language) t.Load(App.Storage.Path + "/Data/" + Config.Languages_Transform); };
            LoadTransform += (t, o) => { if (o is MagicProperty) t.Load(App.Storage.Path + "/Data/" + Config.Magic_Transform); };
            LoadTransform += (t, o) => { if (o is Race) t.Load(App.Storage.Path + "/Data/" + Config.Races_Transform); };
            LoadTransform += (t, o) => { if (o is Skill) t.Load(App.Storage.Path + "/Data/" + Config.Skills_Transform); };
            LoadTransform += (t, o) => { if (o is Spell) t.Load(App.Storage.Path + "/Data/" + Config.Spells_Transform); };
            LoadTransform += (t, o) => { if (o is SubClass) t.Load(App.Storage.Path + "/Data/" + Config.SubClasses_Transform); };
            LoadTransform += (t, o) => { if (o is SubRace) t.Load(App.Storage.Path + "/Data/" + Config.SubRaces_Transform); };
            LoadTransform += (t, o) => { if (o is DisplayPossession) t.Load(App.Storage.Path + "/Data/" + Transform_Possession); };
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
