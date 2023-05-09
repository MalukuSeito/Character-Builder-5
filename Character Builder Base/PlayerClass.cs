using OGL;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Character_Builder
{
    public class PlayerClass
    {
        public List<int> ClassLevelAtLevel { get; set; }
        public List<int> HProllAtClassLevel { get; set; }
        [XmlElement(ElementName = "Class")]
        public String ClassName { get; set; }
        [XmlElement(ElementName = "SubClass")]
        public String SubClassName { get; set; }
        public PlayerClass()
        {
            ClassName = null;
            SubClassName = null;
            ClassLevelAtLevel = new List<int>();
            HProllAtClassLevel = new List<int>();
        }
        public PlayerClass(ClassDefinition classdefinition, int atlevel, int hproll)
        {
            SetClass(classdefinition);
            SubClassName = null;
            ClassLevelAtLevel = new List<int>() {atlevel};
            HProllAtClassLevel = new List<int>() {hproll};
        }

        public ClassDefinition GetClass(OGLContext context)
        {
            if (ClassName == null || ClassName == "") return null;
            return context.GetClass(ClassName, null);
        }

        public void SetClass(ClassDefinition value)
        {
            if (value == null) ClassName = "";
            else ClassName = value.Name + " " + ConfigManager.SourceSeperator + " " + value.Source;
        }

        public SubClass GetSubClass(OGLContext context)
        {
            if (SubClassName == null || SubClassName == "") return null;
            string source = null;
            if (GetClass(context) != null) source = GetClass(context).Source;
            return context.GetSubClass(SubClassName, source);
        }

        public void SetSubClass(SubClass value)
        {
            if (value == null) SubClassName = null;
            else SubClassName = value.Name + " " + ConfigManager.SourceSeperator + " " + value.Source;
        }

        public List<Feature> GetFeatures(int level, Player player, BuilderContext context)
        {
            if (GetClass(context) == null) return new List<Feature>();
            int classlevel = getClassLevelUpToLevel(level);
            bool secondclass = !ClassLevelAtLevel.Contains(1);
            List<Feature> fl = context.Plugins.FilterClassFeatures(GetClass(context), classlevel, GetClass(context).CollectFeatures(classlevel, secondclass, player, context), level, player, context);
            if (GetSubClass(context) != null) fl.AddRange(context.Plugins.FilterSubClassFeatures(GetSubClass(context), GetClass(context), classlevel, GetSubClass(context).CollectFeatures(classlevel, secondclass, player, context), level, player, context));
            return fl;
        }
        public int getClassLevelUpToLevel(int level)
        {
            if (level < ClassLevelAtLevel[0]) return 0;
            for (int c = 0; c < ClassLevelAtLevel.Count; c++) if (level < ClassLevelAtLevel[c]) return c;
            /*{
                if (level == ClassLevelAtLevel[c]) return c + 1;
                else if (level < ClassLevelAtLevel[c]) return c;
            }*/
            return ClassLevelAtLevel.Count;
        }
        public int getHP(int classlevel, int hpperlevel)
        {
            int hp = 0;
            for (int c = 0; c < classlevel; c++) hp += Math.Max(1, HProllAtClassLevel[c]+hpperlevel);
            return hp;
        }
        public bool AddLevel(int atlevel, int hproll)
        {
            ClassLevelAtLevel.Add(atlevel);
            ClassLevelAtLevel.Sort();
            HProllAtClassLevel.Insert(ClassLevelAtLevel.IndexOf(atlevel), hproll);
            return true;
        }
        public int getClassLevelAtLevel(int atlevel)
        {
            return ClassLevelAtLevel.IndexOf(atlevel) + 1;
        }
        public int HPRollAtLevel(int atlevel)
        {
            if (ClassLevelAtLevel.IndexOf(atlevel)<0) return 0;
            return HProllAtClassLevel[ClassLevelAtLevel.IndexOf(atlevel)];
        }
        public int HPAtLevel(int level)
        {
            int classlevel = getClassLevelUpToLevel(level);
            int hp = 0;
            for (int c = 0; c < classlevel; c++) hp += HProllAtClassLevel[c];
            return hp;
        }
        public void setHPRollAtLevel(int atlevel, int hproll)
        {
            if (ClassLevelAtLevel.IndexOf(atlevel) < 0) return;
            HProllAtClassLevel[ClassLevelAtLevel.IndexOf(atlevel)] = hproll;
        }
        public bool DeleteLevel(int atlevel)
        {
            if (ClassLevelAtLevel.Count < 2) return false;
            int classlevel = ClassLevelAtLevel.IndexOf(atlevel);
            if (classlevel < 0) return false;
            HProllAtClassLevel.RemoveAt(classlevel);
            ClassLevelAtLevel.RemoveAt(classlevel);
            return true;
        }
        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public string ToString(OGLContext context, int level)
        {
            OGL.SubClass s = GetSubClass(context);
            if (s != null && s.SheetName != null && s.SheetName.Length > 0) return s.SheetName + " (" + getClassLevelUpToLevel(level) + ")";
            if (SubClassName != null && SubClassName != "") return SourceInvariantComparer.NoSource(SubClassName) + " (" + getClassLevelUpToLevel(level) + ")";
            return SourceInvariantComparer.NoSource(ClassName) + " (" + getClassLevelUpToLevel(level) + ")";
        }

        public int getMulticlassingLevel(OGLContext context, int level)
        {
            int classlevel = getClassLevelUpToLevel(level);
            if (classlevel == 0) return 0;
            SubClass sc = GetSubClass(context);
            if (sc!=null && sc.MulticlassingSpellLevels != null && sc.MulticlassingSpellLevels.Count >= classlevel) return sc.MulticlassingSpellLevels[classlevel - 1];
            ClassDefinition cl= GetClass(context);
            if (cl == null) return 0;
            if (cl.MulticlassingSpellLevels != null && cl.MulticlassingSpellLevels.Count >= classlevel) return cl.MulticlassingSpellLevels[classlevel - 1];
            return 0;
        }

        public List<TableDescription> CollectTables(OGLContext context)
        {
            List<TableDescription> res = new List<TableDescription>();
            if (GetClass(context) != null) foreach (Description d in GetClass(context).Descriptions) if (d is TableDescription) res.Add(d as TableDescription);
            if (GetSubClass(context) != null) foreach (Description d in GetSubClass(context).Descriptions) if (d is TableDescription) res.Add(d as TableDescription);
            return res;
        }
    }
}
