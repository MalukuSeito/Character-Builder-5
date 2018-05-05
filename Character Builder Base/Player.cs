using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using OGL.Items;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Character_Builder
{
    public class Player: IChoiceProvider
    {
        [XmlIgnore]
        public static CultureInfo Culture = CultureInfo.InvariantCulture;

        [XmlIgnore]
        public BuilderContext Context;
        [XmlIgnore]
        public Dictionary<Feature, int> ChoiceCounter;
        [XmlIgnore]
        public Dictionary<string, int> ChoiceTotal;
        [XmlIgnore]
        public object FilePath { get; set; }

        public List<Spellcasting> Spellcasting = new List<Spellcasting>();
        public List<string> Boons = new List<string>();
        public List<AbilityFeatChoice> AbilityFeatChoices = new List<AbilityFeatChoice>();
        public List<UsedResource> UsedResources = new List<UsedResource>();
        public List<Possession> Possessions = new List<Possession>();
        public List<string> Conditions = new List<string>();
        public List<string> HiddenFeatures = new List<string>();
        public List<string> ExcludedSources = new List<string>();
        public List<String> Items = new List<String>();
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Player));
        public int CurrentHPLoss { get; set; }
        public int TempHP { get; set; }
        public int BonusMaxHP { get; set; }
        public int FailedDeathSaves { get; set; }
        public int SuccessDeathSaves { get; set; }
        public List<int> UsedHitDice = new List<int>();
        public List<PlayerClass> Classes { get; set; }
        [XmlElement(ElementName = "Race")]
        public String RaceName { get; set; }
        [XmlElement(ElementName = "SubRace")]
        public String SubRaceName { get; set; }
        [XmlElement(ElementName = "Background")]
        public String BackgroundName { get; set; }
        public String PersonalityTrait { get; set; }
        public String Ideal { get; set; }
        public String Bond { get; set; }
        public String Flaw { get; set; }
        public int BaseStrength { get; set; }
        public int BaseDexterity { get; set; }
        public int BaseConstitution { get; set; }
        public int BaseIntelligence { get; set; }
        public int BaseWisdom { get; set; }
        public int BaseCharisma { get; set; }
        public List<string> Journal = new List<string>();
        public List<JournalEntry> ComplexJournal = new List<JournalEntry>();
        public List<string> ActiveHouseRules = new List<string>();
        [XmlElement("Portrait")]
        public string PortraitLocation = null;

        [XmlElement("PortraitData")]
        public byte[] Portrait { get; set; }
        public String Name { get; set; }
        public String FactionName { get; set;}
        public String FactionRank { get; set; }
        [XmlElement("FactionImage")]
        public string FactionImageLocation = null;


        [XmlElement("FactionImageData")]
        public byte[] FactionImage { get; set; }
        public String Allies { get; set; }
        public String Backstory { get; set; }
        public String Alignment { get; set; }
        public int Age { get; set; }
        public String Height { get; set; }
        public int Weight { get; set; }
        public String Eyes { get; set; }
        public String Skin { get; set; }
        public String Hair { get; set; }
        public int XP { get; set; }
        public String PlayerName { get; set; }
        public String DCI { get; set; }
        public bool Inspiration { get; set; }
        public int CP { get; set; }
        public int SP { get; set; }
        public int EP { get; set; }
        public int GP { get; set; }
        public int PP { get; set; }

        public List<FormsCompanionsChoice> FormsCompanionsChoices = new List<FormsCompanionsChoice>();
        public Player()
        {
            BonusMaxHP = 0;
            BaseStrength = 8;
            BaseDexterity = 8;
            BaseConstitution = 8;
            BaseIntelligence = 8;
            BaseWisdom = 8;
            BaseCharisma = 8;
            CurrentHPLoss = 0;
            FailedDeathSaves = 0;
            SuccessDeathSaves = 0;
            TempHP = 0;
            BackgroundName ="";
            PersonalityTrait ="";
            Ideal ="";
            Bond ="";
            Flaw ="";
            Portrait = null;
            Name ="";
            FactionName ="";
            FactionImage = null;
            Allies ="";
            Backstory ="";
            Alignment ="";
            Age =0;
            Height = "";
            Weight =0;
            Eyes ="";
            Skin ="";
            Hair ="";
            XP=0;
            CP = 0;
            SP = 0;
            EP = 0;
            GP = 0;
            PP = 0;
            ChoiceCounter = new Dictionary<Feature, int>(new ObjectIdentityEqualityComparer());
            ChoiceTotal = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            Journal = new List<string>();
        }

        public int GetSpellSlotsMax(String SpellcastingID = null)
        {
            if (SpellcastingID == null)
            {
                int max = 0;
                foreach (Feature f in GetFeatures())
                {
                    if (f is SpellcastingFeature scf && scf.SpellcastingID != "MULTICLASS")
                    {
                        List<int> sslots = GetSpellSlots(scf.SpellcastingID);
                        for (int level = sslots.Count - 1; level >= 0; level--)
                        {
                            if (sslots[level] > 0)
                            {
                                max = Math.Max(max, level + 1);
                                break;
                            }
                        }
                    }
                }
                return max;
            }
            List<int> slots = GetSpellSlots(SpellcastingID);
            for (int level = slots.Count - 1; level >= 0; level--)
            {
                if (slots[level] > 0) return level + 1;
            }
            return 0;
        }

        public List<Possession> GetItemsAndPossessions()
        {
            Dictionary<string, int> items = new Dictionary<string, int>(ConfigManager.SourceInvariantComparer);
            foreach (Item i in GetFreeItems())
            {
                if (!items.ContainsKey(i.Name)) items.Add(i.Name, 1);
                else items[i.Name]++;
            }
            foreach (string i in Items)
            {
                if (!items.ContainsKey(i)) items.Add(i, 1);
                else items[i]++;
            }
            List<Possession> result = new List<Possession>();
            foreach (Possession p in Possessions)
            {
                p.Context = Context;
                if (p.BaseItem != null && p.BaseItem != "")
                {
                    int stacksize = Context.GetItem(p.BaseItem, null).StackSize;
                    if (stacksize < 0) stacksize = 1;
                    int stack = (int)Math.Ceiling((double)p.Count / (double)stacksize);
                    if (items.ContainsKey(p.BaseItem) && items[p.BaseItem] >= stack)
                    {
                        if (p.Count > 0)
                        {
                            items[p.BaseItem] -= stack;
                        }
                        else
                        {
                            items[p.BaseItem]--;
                        }
                        result.Add(p);
                    }
                    else
                    {
                        //The player had that item, but lost it without losing the possession. That is probably from switching classes, better unequip it as well
                        p.Equipped = EquipSlot.None;
                        p.Attuned = false;
                        p.MagicProperties.Clear();
                    }
                }
                else result.Add(p);
            }
            foreach (string i in items.Keys) if (items[i] > 0) result.Add(new Possession(Context, i, items[i]));
            result.Sort();
            return result;
        }
        public void ChangePossessionAmountAndAddRemoveItemsAccordingly(Possession p, int count)
        {
            if (Possessions.Contains(p))
            {
                if (p.BaseItem != null && p.BaseItem != "")
                {
                    int stacksize = Context.GetItem(p.BaseItem, null).StackSize;
                    if (stacksize < 0) stacksize = 1;
                    int pcount = (int)Math.Ceiling((double)p.Count / (double)stacksize);
                    p.Count = count;
                    count = (int)Math.Ceiling((double)count / (double)stacksize);
                    Item b = p.Item;
                    string s = b?.Name + " " + ConfigManager.SourceSeperator + " " + b?.Source;
                    while (count != pcount) if (count < pcount)
                        {
                            if (b != null) {
                                int i = Items.FindIndex(j => ConfigManager.SourceInvariantComparer.Equals(j, s));
                                if (i >= 0) Items.RemoveAt(i);
                            }
                            else Items.Remove(p.BaseItem);
                            pcount--;
                        }
                        else
                        {
                            if (b != null) Items.Add(s);
                            else Items.Add(p.BaseItem);
                            pcount++;
                        }
                }
                else p.Count = count; 
            }
            else
            {
                if (p.BaseItem != null && p.BaseItem != "")
                {
                    int stacksize = Context.GetItem(p.BaseItem, null).StackSize;
                    if (stacksize < 0) stacksize = 1;
                    int pcount = (int)Math.Ceiling((double)p.Count / (double)stacksize);
                    p.Count = count;
                    count = (int)Math.Ceiling((double)count / (double)stacksize);
                    Item b = p.Item;
                    string s = b?.Name + " " + ConfigManager.SourceSeperator + " " + b?.Source;
                    while (count != pcount) if (count < pcount)
                        {
                            if (b != null)
                            {
                                int i = Items.FindIndex(j => ConfigManager.SourceInvariantComparer.Equals(j, s));
                                if (i >= 0) Items.RemoveAt(i);
                            }
                            else Items.Remove(p.BaseItem);
                            pcount--;
                        }
                        else
                        {
                            if (b != null) Items.Add(s);
                            else Items.Add(p.BaseItem);
                            pcount++;
                        }
                }
                else p.Count = count;
                Possessions.Add(p);
            }
        }
        public void RemovePossessionAndItems(Possession p)
        {
            bool worked = true;
            if (p.BaseItem != null && p.BaseItem != "")
            {
                Item b = p.Item;
                string s = b?.Name + " " + ConfigManager.SourceSeperator + " " + b?.Source;
                int stacksize = Context.GetItem(p.BaseItem, null).StackSize;
                if (stacksize < 0) stacksize = 1;
                int count = (int)Math.Ceiling((double)p.Count / (double)stacksize);
                worked = count > 0;
                for (int i = 0; i < count; i++)
                {
                    worked = false;
                    if (b != null)
                    {
                        int k = Items.FindIndex(j => s.Equals(j, StringComparison.OrdinalIgnoreCase));
                        if (k < 0) k = Items.FindIndex(j => ConfigManager.SourceInvariantComparer.Equals(j, s));
                        if (k >= 0) Items.RemoveAt(k);
                        else break;
                    }
                    else if (!Items.Remove(p.BaseItem)) break;
                    worked = true;
                }
            }
            if (worked)
            {
                if (Possessions.Contains(p)) Possessions.Remove(p);
            }
            else
            {
                p.Count = 0;
                p.Equipped = EquipSlot.None;
                p.MagicProperties.Clear();
                AddPossession(p);
            }
        }
        public void AddPossession(Possession p)
        {
            if (!Possessions.Contains(p)) Possessions.Add(p);
        }
        public double GetWeight()
        {
            List<Possession> items = GetItemsAndPossessions();
            double total=0.0;
            foreach (Possession o in items) total += o.GetWeight();
            total += GetMoney().Weight(Context);
            return total;
        }
        public void SetAbilityFeatChoices(AbilityScoreFeatFeature f, Ability ab1, Ability ab2,String feat, String source)
        {
            int level=GetLevel();
            AbilityFeatChoice afc = AbilityFeatChoices.Find(t => t.UniqueID == f.UniqueID && t.Level == f.Level);
            if (afc == null)
            {
                afc = new AbilityFeatChoice()
                {
                    Level = f.Level,
                    UniqueID = f.UniqueID
                };
                AbilityFeatChoices.Add(afc);
                foreach (PlayerClass p in Classes)
                {
                    if (p.GetFeatures(level, this, Context).Contains(f)) afc.Class = p.ClassName;
                }
            }
            afc.Ability1=ab1;
            afc.Ability2=ab2;
            afc.Feat = feat + " " + ConfigManager.SourceSeperator + " " + source;
        }
        public List<Feature> GetPossessionFeatures(int level = 0, bool reset = false, bool includeOnUseFeatures = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (level == 0) level = GetLevel();
            List<Feature> result=new List<Feature>();
            foreach (Possession p in Possessions)
            {
                result.AddRange(p.Collect(level, this, Context, false, includeOnUseFeatures));
            }
            return Context.Plugins.FilterPossessionFeatures(result, level, this, Context);
        }
        public AbilityFeatChoice GetAbilityFeatChoice(AbilityScoreFeatFeature f)
        {
            int level = GetLevel();
            AbilityFeatChoice afc = AbilityFeatChoices.Find(t => t.UniqueID == f.UniqueID && t.Level == f.Level);
            if (afc == null)
            {
                afc = new AbilityFeatChoice()
                {
                    Level = f.Level,
                    UniqueID = f.UniqueID
                };
                AbilityFeatChoices.Add(afc);
                foreach (PlayerClass p in Classes)
                {
                    if (p.GetFeatures(level, this, Context).Contains(f)) afc.Class = p.ClassName;
                }
            }
            return afc;
        }
        public IEnumerable<AbilityFeatChoice> GetAbilityFeatChoices(int level=0)
        {
            if (level==0) level = GetLevel();
            Dictionary<String, int> cl = GetClassLevelStrings(level);
            return from AbilityFeatChoice afc in AbilityFeatChoices where (afc.UniqueID != null && afc.UniqueID != "" && afc.Level <= level && (afc.Class == null || afc.Class == "")) || (cl.ContainsKey(afc.Class) && afc.Level <= cl[afc.Class]) select afc;
        }
        public Dictionary<ClassDefinition, int> GetClassLevels(int level=0)
        {
            if (level == 0) level = GetLevel();
            Dictionary<ClassDefinition, int> classlevels = new Dictionary<ClassDefinition, int>();
            foreach (PlayerClass p in Classes)
            {
                classlevels.Add(p.GetClass(Context), p.getClassLevelUpToLevel(level));
            }
            return classlevels;
        }
        public Spellcasting GetSpellcasting(string spellcastingID)
        {
            foreach (Spellcasting sc in Spellcasting) if (sc.SpellcastingID == spellcastingID) return sc;
            Spellcasting s = new Spellcasting()
            {
                SpellcastingID = spellcastingID
            };
            Spellcasting.Add(s);
            return s;
        }
        public int GetClassLevel(string spellcastingID, int level = 0)
        {
            if (level == 0) level = GetLevel();
            foreach (PlayerClass pc in Classes)
            {
                foreach (Feature f in pc.GetFeatures(level, this, Context)) if (f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID) return pc.getClassLevelUpToLevel(level);
            }
            return 0;
        }
        public SpellChoice GetSpellChoice(string spellcastingID, string uniqueID)
        {
            Spellcasting sc = GetSpellcasting(spellcastingID);
            int level = GetLevel();
            foreach (SpellChoicePerLevel spcl in sc.SpellChoicesPerLevel) if (spcl.Level == level)
                {
                    foreach (SpellChoice spc in spcl.Choices) if (spc.UniqueID == uniqueID) return spc;
                    SpellChoicePerLevel bestmatch = (from last in sc.SpellChoicesPerLevel where last.Level < level && last.Choices.Any(it => it.UniqueID == uniqueID) orderby last.Level descending select last).FirstOrDefault();
                    SpellChoice s;
                    if (bestmatch != null) {
                        s = new SpellChoice((from best in bestmatch.Choices where best.UniqueID == uniqueID select best).FirstOrDefault());
                    } else {
                        s = new SpellChoice();
                    }
                    s.UniqueID = uniqueID;
                    spcl.Choices.Add(s);
                    return s;
                }
            {
                SpellChoicePerLevel spcl = new SpellChoicePerLevel(level);
                SpellChoicePerLevel bestmatch = (from last in sc.SpellChoicesPerLevel where last.Level < level && last.Choices.Any(it => it.UniqueID == uniqueID) orderby last.Level descending select last).FirstOrDefault();
                SpellChoice s;
                if (bestmatch != null) {
                    s = new SpellChoice((from best in bestmatch.Choices where best.UniqueID == uniqueID select best).FirstOrDefault());
                } else {
                    s = new SpellChoice();
                }
                s.UniqueID = uniqueID;
                //sc.Spellchoices.Add(s);
                spcl.Choices.Add(s);
                sc.SpellChoicesPerLevel.Add(spcl);
                return s;
            }
        }

        

        public List<int> GetSpellSlots(string spellcastingID, int level = 0)
        {
            if (level == 0) level = GetLevel();
            int classlevel = 0;
            Spellcasting sc = GetSpellcasting(spellcastingID);
            int multiclassinglevel = 0;
            bool overwrittenbymulticlassing = false;
            foreach (PlayerClass pc in Classes)
            {
                foreach (Feature f in pc.GetFeatures(level, this, Context))
                {
                    if (f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID)
                    {
                        overwrittenbymulticlassing = ((SpellcastingFeature)f).OverwrittenByMulticlassing;
                        classlevel = pc.getClassLevelUpToLevel(level);
                    }
                }
                multiclassinglevel += pc.getMulticlassingLevel(Context, level);
            }
            int curlevel = 0;
            List<int> slots = null;
            List<Feature> features = GetFeatures();
            if (overwrittenbymulticlassing && features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == "MULTICLASS")) {
                spellcastingID="MULTICLASS";
                classlevel = multiclassinglevel;
            }
            List<SpellSlotsFeature> ssfs = new List<SpellSlotsFeature>(from f in features where f is SpellSlotsFeature select (SpellSlotsFeature)f);
            foreach (SpellSlotsFeature ssf in ssfs)
            {
                if (ssf.SpellcastingID == spellcastingID)
                    if (ssf.Level > curlevel && ssf.Level <= classlevel)
                    {
                        slots = ssf.Slots;
                        curlevel = ssf.Level;
                    }
            }
            if (slots == null) slots = new List<int>();
            return slots;
        }
        public List<int> GetUsedSpellSlots(string spellcastingID)
        {
            List<Feature> features = GetFeatures();
            if (features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID && ((SpellcastingFeature)f).OverwrittenByMulticlassing) && features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == "MULTICLASS")) spellcastingID = "MULTICLASS";
            Spellcasting sc = GetSpellcasting(spellcastingID);
            return sc.UsedSlots;
        }
        public void SetSpellSlot(string spellcastingID, int spelllevel, int value)
        {
            List<Feature> features = GetFeatures();
            if (features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID && ((SpellcastingFeature)f).OverwrittenByMulticlassing) && features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == "MULTICLASS")) spellcastingID = "MULTICLASS";
            Spellcasting sc = GetSpellcasting(spellcastingID);
            while (sc.UsedSlots.Count < spelllevel) sc.UsedSlots.Add(0);
            sc.UsedSlots[spelllevel - 1] = value;
        }
        public void ResetSpellSlots(string spellcastingID)
        {
            List<Feature> features = GetFeatures();
            if (features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID && ((SpellcastingFeature)f).OverwrittenByMulticlassing) && features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == "MULTICLASS")) spellcastingID = "MULTICLASS";
            Spellcasting sc = GetSpellcasting(spellcastingID);
            sc.UsedSlots = new List<int>();
        }
        public List<SpellSlotInfo> GetSpellSlotInfo(string SpellcastingID, int level = 0)
        {
            List<int> slots = GetSpellSlots(SpellcastingID, level);
            List<int> usedSlots = GetUsedSpellSlots(SpellcastingID);
            List<SpellSlotInfo> res = new List<SpellSlotInfo>();
            for (int spelllevel = 0; spelllevel < slots.Count; spelllevel++)
            {
                if (slots[spelllevel] > 0)
                    if (spelllevel < usedSlots.Count) res.Add(new SpellSlotInfo(SpellcastingID, spelllevel + 1, slots[spelllevel], usedSlots[spelllevel]));
                    else res.Add(new SpellSlotInfo(SpellcastingID, spelllevel + 1, slots[spelllevel], 0));
            }
            return res;
        }
        public Dictionary<String, int> GetClassLevelStrings(int level = 0)
        {
            if (level == 0) level = GetLevel();
            Dictionary<String, int> classlevels = new Dictionary<String, int>(StringComparer.OrdinalIgnoreCase);
            if (Classes == null) Classes = new List<PlayerClass>();
            foreach (PlayerClass p in Classes)
            {
                int classlevel = p.getClassLevelUpToLevel(level);
                if (classlevel>0)
                    classlevels.Add(p.ClassName, classlevel);
            }
            return classlevels;
        }
        
        public bool AddClass(ClassDefinition classdefinition, int atLevel)
        {
            if (classdefinition == null) return false;
            int hproll = classdefinition.AverageHPPerLevel;
            if (atLevel == 1) hproll = classdefinition.HPFirstLevel;
            PlayerClass found = null;
            foreach (PlayerClass p in Classes)
            {
                if (p.getClassLevelAtLevel(atLevel) > 0) { return false; }
                if (p.GetClass(Context) == classdefinition) found = p;
            }
            if (found != null) return found.AddLevel(atLevel, hproll);
            Classes.Add(new PlayerClass(classdefinition, atLevel, hproll));
            return true;
        }
        public ClassDefinition GetClass(int atLevel)
        {
            foreach (PlayerClass p in Classes)
            {
                if (p.getClassLevelAtLevel(atLevel) > 0)
                {
                    return p.GetClass(Context);
                }
            }
            return null;
        }

        public string GetClassName(int atLevel)
        {
            foreach (PlayerClass p in Classes)
            {
                if (p.getClassLevelAtLevel(atLevel) > 0)
                {
                    return p.ClassName;
                }
            }
            return null;
        }

        public int GetClassLevel(int atLevel)
        {
            foreach (PlayerClass p in Classes)
            {
                if (p.getClassLevelAtLevel(atLevel) > 0)
                {
                    return p.getClassLevelUpToLevel(atLevel);
                }
            }
            return 0;
        }
        public bool DeleteClass(int atLevel)
        {
            PlayerClass todelete=null;
            foreach (PlayerClass p in Classes)
            {
                if (p.getClassLevelAtLevel(atLevel) > 0)
                {
                    todelete = p;
                    if (p.DeleteLevel(atLevel)) return true;
                }
            }
            if (todelete != null) Classes.Remove(todelete);
            return false;
        }
        public void SetHPRoll(int atLevel, int hproll)
        {
            foreach (PlayerClass p in Classes)
            {
                if (p.getClassLevelAtLevel(atLevel) > 0)
                {
                    p.setHPRollAtLevel(atLevel, hproll);
                }
            }
        }
        public int GetHPRoll(int atLevel)
        {
            foreach (PlayerClass p in Classes)
            {
                if (p.getClassLevelAtLevel(atLevel) > 0)
                {
                    return p.HPRollAtLevel(atLevel);
                }
            }
            return 0;
        }
        public List<ClassDefinition> GetClassesByLevel()
        {
            int level=GetLevel();
            List<ClassDefinition> cd=new List<ClassDefinition>();
            if (Classes == null) Classes = new List<PlayerClass>();
            for (int c = 1; c <= level; c++) {
                PlayerClass pc = Classes.Find(p => p.getClassLevelAtLevel(c) > 0);
                if (pc == null) cd.Add(null);
                else cd.Add(pc.GetClass(Context));
            }
            return cd;
        }
        public List<int> GetHProllsByLevel()
        {
            int level = GetLevel();
            List<int> chp = new List<int>();
            for (int c = 1; c <= level; c++)
            {
                PlayerClass pc = Classes.Find(p => p.getClassLevelAtLevel(c) > 0);
                if (pc == null) chp.Add(0);
                else chp.Add(pc.HPRollAtLevel(c));
            }
            return chp;
        }
        public List<ClassInfo> GetClassInfos(int level=0)
        {
            if (level == 0) level = GetLevel();
            List<ClassInfo> ci = new List<ClassInfo>();
            for (int c = 1; c <= level; c++)
            {
                PlayerClass pc = Classes.Find(p => p.getClassLevelAtLevel(c) > 0);
                if (pc != null) ci.Add(new ClassInfo(pc.GetClass(Context), c, pc.HPRollAtLevel(c), pc.getClassLevelUpToLevel(c)));
                else ci.Add(new ClassInfo(null,c,0,0));
            }
            return ci;
        }
        public int GetChoiceOffset(Feature f,string uniqueID, int amount) {
            if (!ChoiceCounter.ContainsKey(f))
            {
                ChoiceCounter.Add(f, GetChoiceTotal(uniqueID));
                ChoiceTotal[uniqueID] += amount;
            }
            return ChoiceCounter[f];
        }
        public void ResetChoices()
        {
            
        }
        public int GetChoiceTotal(string uniqueID) {
            if (!ChoiceTotal.ContainsKey(uniqueID)) ChoiceTotal.Add(uniqueID, 0);
            return ChoiceTotal[uniqueID];
        }
        public List<Choice> Choices = new List<Choice>();
        [XmlIgnore]
        public Background Background
        {
            get
            {
                if (BackgroundName == null || BackgroundName == "") return null;
                return Context.GetBackground(BackgroundName, null);
            }
            set
            {
                if (value == null) BackgroundName = "";
                else BackgroundName = value.Name + " " + ConfigManager.SourceSeperator + " " + value.Source;
            }
        }
        [XmlIgnore]
        public Race Race
        {
            get
            {
                if (RaceName == null || RaceName == "") return null;
                return Context.GetRace(RaceName, null);
            }
            set
            {
                if (value == null)
                {
                    RaceName = "";
                    SubRaceName = "";
                }
                else RaceName = value.Name + " " + ConfigManager.SourceSeperator + " " + value.Source;
            }
        }
        [XmlIgnore]
        public SubRace SubRace
        {
            get
            {
                if (SubRaceName == null || SubRaceName == "") return null;
                return Context.GetSubRace(SubRaceName, null);
            }
            set
            {
                if (value == null) SubRaceName = "";
                else SubRaceName = value.Name + " " + ConfigManager.SourceSeperator + " " + value.Source;
            }
        }

        public bool AllRituals { get; set; }

        public List<Feature> GetBackgroundFeatures(int level=0, bool reset = false, bool includeOnUseFeatures = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            if (level == 0) level = GetLevel();
            List<Feature> fl = new List<Feature>();
            if (Background != null) fl.AddRange(Context.Plugins.FilterBackgroundFeatures(Background, Background.CollectFeatures(level, this, Context), level, this, Context));
            fl.AddRange(GetBoons(level, false));
            fl.AddRange(GetPossessionFeatures(level, false, includeOnUseFeatures));
            return fl;
        }

        public List<Feature> GetOnlyBackgroundFeatures(int level = 0, bool reset = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            if (level == 0) level = GetLevel();
            List<Feature> fl = new List<Feature>();
            if (Background != null) fl.AddRange(Context.Plugins.FilterBackgroundFeatures(Background, Background.CollectFeatures(level, this, Context), level, this, Context));
            return fl;
        }
        public bool CanMulticlass(ClassDefinition c, int level)
        {
            List<string> skills = new List<string>();
            var features = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, f => f is SkillProficiencyFeature || f is SkillProficiencyChoiceFeature, level);
            foreach (FeatureClass fc in features)
            {
                if (fc.feature is SkillProficiencyFeature && ((SkillProficiencyFeature)fc.feature).ProficiencyMultiplier > 0.999) foreach (String s in ((SkillProficiencyFeature)fc.feature).Skills) skills.Add(s);
                else if (fc.feature is SkillProficiencyChoiceFeature && ((SkillProficiencyChoiceFeature)fc.feature).ProficiencyMultiplier > 0.999) skills.AddRange(((SkillProficiencyChoiceFeature)fc.feature).getSkills(this, Context).Select(s=>s.Name));
            }
            
            
            if (Classes.Count > 0 && Classes[0].GetClass(Context) != null)
            {
                if (Classes[0].GetClass(Context) == c) return true;
                if (!Utils.CanMulticlass(Context, Classes[0].GetClass(Context), asa, skills)) return false;
            }
            else return false; //Can not multiclass without first class.
            foreach (var d in Classes) if (d.GetClass(Context) != c && d.GetClass(Context).Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase)) return false; // Can not multiclass into the same class from a different sourcebook
            return Utils.CanMulticlass(Context, c, asa, skills);
        }
        public List<Feature> GetCommonFeaturesAndFeats(int level = 0, bool reset = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            if (level == 0) level = GetLevel();
            List<Feature> fl = new List<Feature>();
            foreach (Feature f in Context.Config.CommonFeatures)
                fl.AddRange(f.Collect(level, this, Context));
            fl = Context.Plugins.FilterCommonFeatures(fl, level, this, Context);
            fl.AddRange(GetFeats(level, false));
            return fl;
        }
        public List<Feature> GetClassFeatures(int level=0, bool reset = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            if (level == 0) level = GetLevel();
            List<Feature> fl = new List<Feature>();
            if (Classes!=null && Classes.Count > 0)
            {
                int multiclassing = 0;
                int multiclassinglevel = 0;
                foreach (PlayerClass p in Classes)
                {
                    if (p.getClassLevelUpToLevel(level) > 0)
                    {
                        fl.AddRange(p.GetFeatures(level, this, Context));
                        multiclassing++;
                        multiclassinglevel += p.getMulticlassingLevel(Context, level);
                    }
                }
                if (multiclassinglevel < 1) multiclassinglevel = 1;
                if (multiclassing > 1)
                {
                    List<Feature> res = new List<Feature>();
                    foreach (Feature f in Context.Config.MultiClassFeatures)
                        res.AddRange(f.Collect(multiclassinglevel, this, Context));
                    fl.AddRange(Context.Plugins.FilterClassFeatures(null, multiclassinglevel, res, level, this, Context));
                }
            }
            return fl;
        }
        public List<Feature> GetRaceFeatures(int level = 0, bool reset = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            if (level == 0) level = GetLevel();
            List<Feature> fl = new List<Feature>();
            if (Race == null) return fl;
            fl.AddRange(GetSubRaceFeatures(level, false));
            fl.AddRange(Context.Plugins.FilterRaceFeatures(Race, Race.CollectFeatures(level, this, Context), level, this, Context));
            return fl;
        }
        public List<Feature> GetSubRaceFeatures(int level = 0, bool reset = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            if (level == 0) level = GetLevel();
            if (SubRace == null) return new List<Feature>();
            return Context.Plugins.FilterSubRaceFeatures(SubRace, Race, SubRace.CollectFeatures(level, this, Context), level, this, Context); 
        }
        public void AddSubclass(string cd, string subclass)
        {
            foreach (PlayerClass p in Classes) if (ConfigManager.SourceInvariantComparer.Equals(p.ClassName, cd)) p.SubClassName = subclass;
        }
        public SubClass GetSubclass(string cd)
        {
            foreach (PlayerClass p in Classes) if (ConfigManager.SourceInvariantComparer.Equals(p.ClassName, cd)) return p.GetSubClass(Context);
            return null;
        }
        public string GetSubclassName(string cd)
        {
            foreach (PlayerClass p in Classes) if (ConfigManager.SourceInvariantComparer.Equals(p.ClassName, cd)) return p.SubClassName;
            return null;
        }
        public void RemoveSubclass(string cd)
        {
            foreach (PlayerClass p in Classes) if (ConfigManager.SourceInvariantComparer.Equals(p.ClassName, cd)) p.SetSubClass(null);
        }
        public List<Feature> GetBoons(int level=0, bool reset = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            if (Boons == null) return new List<Feature>();
            List<Feature> res = new List<Feature>();
            if (level == 0) level = GetLevel();
            foreach (string s in Boons) res.AddRange(Context.GetBoon(s, null).Collect(level, this, Context));
            return Context.Plugins.FilterBoons(res, level, this, Context);
        }
        public List<Feature> GetFeats(int level = 0, bool reset = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            if (AbilityFeatChoices == null) return new List<Feature>();
            List<Feature> res = new List<Feature>();
            if (level == 0) level = GetLevel();
            List<Feature> feats = Context.GetFeatureCollection("");
            foreach (AbilityFeatChoice s in GetAbilityFeatChoices(level)) 
            {
                if (s.Ability1 == Ability.None && s.Ability2 == Ability.None && s.Feat != null && s.Feat != "")
                {
                    Feature feat = feats.Find(f => string.Equals(f.Name + " " + ConfigManager.SourceSeperator + " " + f.Source, s.Feat, StringComparison.OrdinalIgnoreCase));
                    if (feat == null) feat = feats.Find(f => ConfigManager.SourceInvariantComparer.Equals(f.Name + " " + ConfigManager.SourceSeperator + " " + f.Source, s.Feat));
                    if (feat != null) res.AddRange(feat.Collect(level, this, Context));
                    else ConfigManager.LogError("Missing Feat: " + s.Feat);
                }
            }
            return Context.Plugins.FilterFeats(res, level, this, Context);
        }
        public List<string> GetFeatNames(int level = 0)
        {
            if (AbilityFeatChoices == null) return new List<string>();
            List<string> res = new List<string>();
            if (level==0) level = GetLevel();
            List<Feature> feats = Context.GetFeatureCollection("");
            foreach (AbilityFeatChoice s in GetAbilityFeatChoices(level)) if (s.Ability1 == Ability.None && s.Ability2 == Ability.None && s.Feat != null && s.Feat != "") res.Add(SourceInvariantComparer.NoSource(s.Feat));
            return res;
        }
        public IEnumerable<AbilityScoreFeatFeature> GetAbilityIncreases(int level = 0)
        {
            return from Feature f in GetFeatures(level) where f is AbilityScoreFeatFeature select (AbilityScoreFeatFeature)f;
        }
        public List<Feature> GetFeatures(int level=0, bool reset = false, bool includeOnUseFeatures = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            List<Feature> res = new List<Feature>();
            res.AddRange(GetBackgroundFeatures(level, false, includeOnUseFeatures));
            res.AddRange(GetRaceFeatures(level, false));
            res.AddRange(GetClassFeatures(level, false));
            res.AddRange(GetCommonFeaturesAndFeats(level, false));
            return res;
        }
        public Choice GetChoice(String ID)
        {
            return (from c in Choices where c.UniqueID == ID select c).FirstOrDefault<Choice>();
        }

        public void SetChoice(String ID, String Value)
        {
            Choice c = GetChoice(ID);
            if (c != null) c.Value = Value;
            else Choices.Add(new Choice(ID, Value));
        }
        public Price GetMoney(bool includeJournal = true)
        {
            int cp = CP;
            int sp = SP;
            int ep = EP;
            int gp = GP;
            int pp = PP;
            foreach (Feature f in GetFeatures()) if (f is FreeItemAndGoldFeature)
                {
                    cp += ((FreeItemAndGoldFeature)f).CP;
                    sp += ((FreeItemAndGoldFeature)f).SP;
                    gp += ((FreeItemAndGoldFeature)f).GP;
                }
            Price p = new Price(cp, sp, gp)
            {
                ep = ep,
                pp = pp
            };
            if (includeJournal)  foreach (JournalEntry e in ComplexJournal)
            {
                p.cp += e.CP;
                p.gp += e.GP;
                p.ep += e.EP;
                p.sp += e.SP;
                p.pp += e.PP;
            }
            while (p.cp < 0)
            {
                p.sp -= 1;
                p.cp += 10;
            }
            while (p.sp < 0)
            {
                p.gp -= 1;
                p.sp += 10;
            }
            while (p.ep < 0)
            {
                p.gp -= 1;
                p.ep += 2;
            }
            while (p.gp < 0 && p.pp > 0)
            {
                p.pp -= 1;
                p.gp += 10;
            }
            return p;
        }

        public void SetMoney(int cp, int sp, int ep, int gp, int pp)
        {
            foreach (Feature f in GetFeatures()) if (f is FreeItemAndGoldFeature)
                {
                    cp -= ((FreeItemAndGoldFeature)f).CP;
                    sp -= ((FreeItemAndGoldFeature)f).SP;
                    gp -= ((FreeItemAndGoldFeature)f).GP;
                }
            foreach (JournalEntry e in ComplexJournal)
            {
                cp -= e.CP;
                gp -= e.GP;
                ep -= e.EP;
                sp -= e.SP;
                pp -= e.PP;
            }
            CP = cp;
            SP = sp;
            EP = ep;
            GP = gp;
            PP = pp;
        }

        public Dictionary<string,int> GetResources(int level = 0)
        {
            if (level == 0) level = GetLevel();
            Dictionary<string, string> exclusion = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, int> res = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var features = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is ResourceFeature);
            foreach (FeatureClass fc in features)
            {
                Feature f = fc.feature;
                if (f is ResourceFeature rf)
                {
                    if (!res.ContainsKey(rf.ResourceID))
                    {
                        int v = Utils.Evaluate(Context, rf, asa, null, fc.classlevel, level);
                        if (rf.ValueBonus != Ability.None) v = Math.Max(1, v);
                        res.Add(rf.ResourceID, v);
                        if (rf.ExclusionID != null && rf.ExclusionID != "" && !exclusion.ContainsKey(rf.ResourceID)) exclusion.Add(rf.ResourceID, rf.ExclusionID);
                    }
                    else
                    {
                        res[rf.ResourceID] += Utils.Evaluate(Context, rf, asa, null, fc.classlevel, level);
                        if (rf.ExclusionID != null && rf.ExclusionID != "" && !exclusion.ContainsKey(rf.ResourceID)) exclusion.Add(rf.ResourceID, rf.ExclusionID);
                    }
                }
            }
            HashSet<string> excluded = new HashSet<string>(exclusion.Values);
            foreach (string ex in excluded) {
                List<string> ids = new List<string>();
                foreach (string r in exclusion.Keys) if (String.Equals(ex, exclusion[r])) ids.Add(r);
                if (ids.Count > 1)
                {
                    ids.Sort((a, b) => res[b].CompareTo(res[a]));
                    for (int i = 1; i < ids.Count; i++) res.Remove(ids[i]);
                }
            }
            return res;
        }
        public Dictionary<string, RechargeModifier> GetResourceRecharge()
        {
            Dictionary<string, RechargeModifier> res = new Dictionary<string, RechargeModifier>(StringComparer.OrdinalIgnoreCase);
            foreach (Feature f in GetFeatures())
            {
                if (f is ResourceFeature rf)
                {
                    if (!res.ContainsKey(rf.ResourceID))
                    {
                        res.Add(rf.ResourceID, rf.Recharge);
                    }
                    else
                    {
                        if (res[rf.ResourceID] < rf.Recharge) res[rf.ResourceID] = rf.Recharge; //Bigger is better (more often)
                    }
                }
            }
            return res;
        }
        public Dictionary<string, ResourceInfo> GetResourceInfo(bool displayUsed, int level = 0)
        {
            if (level == 0) level = GetLevel();
            Dictionary<string, string> exclusion = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, ResourceInfo> res = new Dictionary<string, ResourceInfo>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, int> used = new Dictionary<string, int>();
            foreach (UsedResource ur in UsedResources) used[ur.ResourceID] = ur.Used;
            foreach (FeatureClass fc in GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is ResourceFeature))
            {
                Feature f = fc.feature;
                if (f is ResourceFeature rf)
                {
                    if (!res.ContainsKey(rf.ResourceID))
                    {
                        int v = Utils.Evaluate(Context, rf, asa, null, fc.classlevel, level);
                        res.Add(rf.ResourceID, new ResourceInfo(rf.ResourceID, rf.Name, GetUsedResources(rf.ResourceID), v, rf.Recharge, displayUsed));
                        if (rf.ExclusionID != null && rf.ExclusionID != "" && !exclusion.ContainsKey(rf.ResourceID)) exclusion.Add(rf.ResourceID, rf.ExclusionID);
                    }
                    else
                    {
                        ResourceInfo r = res[rf.ResourceID];
                        if (r.Recharge < rf.Recharge) r.Recharge = rf.Recharge; //Bigger is better (more often)
                        r.Max += Utils.Evaluate(Context, rf, asa, null, fc.classlevel, level);
                        res[rf.ResourceID] = r;
                        if (rf.ExclusionID != null && rf.ExclusionID != "" && !exclusion.ContainsKey(rf.ResourceID)) exclusion.Add(rf.ResourceID, rf.ExclusionID);
                    }
                }
            }
            HashSet<string> excluded = new HashSet<string>(exclusion.Values);
            foreach (string ex in excluded)
            {
                List<string> ids = new List<string>();
                foreach (string r in exclusion.Keys) if (String.Equals(ex, exclusion[r])) ids.Add(r);
                if (ids.Count > 1)
                {
                    ids.Sort((a, b) => res[b].CompareTo(res[a]));
                    for (int i = 1; i < ids.Count; i++) res.Remove(ids[i]);
                }
            }
            return res;
        }
        public FeatureContainer GetResourceFeatures(String resourceID) //.toHTML for description
        {
            return new FeatureContainer(from f in GetFeatures() where f is ResourceFeature && ((ResourceFeature)f).ResourceID == resourceID orderby f.Level select f);
        }
        public int GetUsedResources(String resourceID)
        {
            if (UsedResources.Count == 0) return 0;
            List<UsedResource> ur=new List<UsedResource>(from f in UsedResources where f.ResourceID == resourceID select f);
            if (ur.Count > 0) return ur[0].Used;
            return 0;
        }
        public void SetUsedResources(String resourceID, int value)
        {
            if (UsedResources.Count == 0)
            {
                UsedResources.Add(new UsedResource(resourceID, value));
                return;
            }
            List<UsedResource> ur=new List<UsedResource>(from f in UsedResources where f.ResourceID == resourceID select f);
            if (ur.Count == 0) UsedResources.Add(new UsedResource(resourceID, value));
            else ur[0].Used = value;
        }
        public void RemoveChoice(String ID)
        {
            Choices.RemoveAll(c => c.UniqueID == ID);
        }
        
        public IEnumerable<Skill> GetSkillProficiencies()
        {
            List<Skill> skills=new List<Skill>();
            foreach (Feature f in GetFeatures())
            {
                if (f is SkillProficiencyFeature && ((SkillProficiencyFeature)f).ProficiencyMultiplier > 0.999) foreach (String s in ((SkillProficiencyFeature)f).Skills) skills.Add(Context.GetSkill(s, f.Source));
                else if (f is SkillProficiencyChoiceFeature && ((SkillProficiencyChoiceFeature)f).ProficiencyMultiplier > 0.999) skills.AddRange(((SkillProficiencyChoiceFeature)f).getSkills(this, Context));
            }
            return skills.Distinct<Skill>();
        }
        public Ability GetSaveProficiencies()
        {
            Ability saves = Ability.None;
            foreach (Feature f in GetFeatures()) if (f is SaveProficiencyFeature) saves |= ((SaveProficiencyFeature)f).Ability;
            return saves;
        }
        public IEnumerable<Language> GetLanguages()
        {
            List<Language> langs = new List<Language>();
            foreach (Feature f in GetFeatures())
            {
                if (f is LanguageProficiencyFeature) foreach (String s in ((LanguageProficiencyFeature)f).Languages) langs.Add(Context.GetLanguage(s, f.Source));
                else if (f is LanguageChoiceFeature) langs.AddRange(((LanguageChoiceFeature)f).getLanguages(this, Context));
            }
            langs.Sort();
            return langs.Distinct<Language>();
        }
        public IEnumerable<Tool> GetToolProficiencies()
        {
            List<Tool> tools = new List<Tool>();
            List<ToolKWProficiencyFeature> kwpf = new List<ToolKWProficiencyFeature>();
            foreach (Feature f in GetFeatures())
            {
                if (f is ToolProficiencyFeature) foreach (String s in ((ToolProficiencyFeature)f).Tools) tools.Add(Context.GetItem(s, f.Source).AsTool());
                else if (f is ToolProficiencyChoiceConditionFeature) tools.AddRange(((ToolProficiencyChoiceConditionFeature)f).getTools(this, Context));
                if (f is ToolKWProficiencyFeature) kwpf.Add(((ToolKWProficiencyFeature)f));
            }
            tools.Sort();
            tools.RemoveAll(t=>kwpf.Find(p=>Utils.Matches(Context, p, t, 0))!=null);
            return tools.Distinct<Tool>();
        }
        public IEnumerable<ModifiedSpell> GetBonusSpells(bool FilterAtWill=false)
        {
            List<ModifiedSpell> spells = new List<ModifiedSpell>();
            foreach (Feature f in GetFeatures())
            {
                if (f is BonusSpellFeature bsf)
                {
                    Spell s = Context.GetSpell(bsf.Spell, bsf.Source);
                    if (!FilterAtWill || (bsf.SpellCastModifier < RechargeModifier.AtWill && bsf.SpellCastModifier != RechargeModifier.Unmodified) || (bsf.SpellCastModifier < RechargeModifier.AtWill && s.Level > 0))
                        spells.Add(new ModifiedSpell(Context.GetSpell (bsf.Spell, bsf.Source), bsf.KeywordsToAdd, bsf.SpellCastingAbility, bsf.SpellCastModifier));
                }
                else if (f is BonusSpellKeywordChoiceFeature) if (!FilterAtWill || ((BonusSpellKeywordChoiceFeature)f).SpellCastModifier < RechargeModifier.AtWill) spells.AddRange((Utils.GetSpells(Context, (BonusSpellKeywordChoiceFeature)f)).Where(s => s.Level > 0 || !FilterAtWill));
            }
            Dictionary<ModifiedSpell, ModifiedSpell> distinct = new Dictionary<ModifiedSpell, ModifiedSpell>();
            foreach (ModifiedSpell ms in spells)
            {
                if (!distinct.ContainsKey(ms)) distinct.Add(ms, ms);
                else
                {
                    ModifiedSpell other = distinct[ms];
                    other.AdditionalKeywords.AddRange(ms.AdditionalKeywords);
                    other.AdditionalKeywords = other.AdditionalKeywords.OrderBy(k => k.Name).Distinct().ToList();
                    other.Modifikations.AddRange(ms.Modifikations);
                    other.Modifikations = other.Modifikations.OrderBy(k => k.Name).Distinct().ToList();
                    other.count++;
                }
            }
            return distinct.Keys.OrderBy(ss => ss.Name);
        }
        public IEnumerable<string> GetToolKWProficiencies()
        {
            List<string> tools = new List<string>();
            foreach (Feature f in GetFeatures())
            {
                if (f is ToolKWProficiencyFeature) tools.Add(((ToolKWProficiencyFeature)f).Description);
            }
            return tools;
        }
        public IEnumerable<string> GetOtherProficiencies()
        {
            return from f in GetFeatures() where f is OtherProficiencyFeature orderby f.Text select f.Text;
        }

        public IEnumerable<string> GetImmunities()
        {
            return GetFeatures().Where(f => f is OtherProficiencyFeature).SelectMany(f => (f as ResistanceFeature).Immunities).OrderBy(s=>s).Distinct();
        }
        public IEnumerable<string> GetVulnerabilities()
        {
            return GetFeatures().Where(f => f is OtherProficiencyFeature).SelectMany(f => (f as ResistanceFeature).Vulnerabilities).OrderBy(s => s).Distinct();
        }
        public IEnumerable<string> GetResistances()
        {
            return GetFeatures().Where(f => f is OtherProficiencyFeature).SelectMany(f => (f as ResistanceFeature).Resistances).OrderBy(s => s).Distinct();
        }
        public IEnumerable<string> GetSavingThrowAdvantages()
        {
            return GetFeatures().Where(f => f is OtherProficiencyFeature).SelectMany(f => (f as ResistanceFeature).SavingThrowAdvantages).OrderBy(s => s).Distinct();
        }
        public List<Item> GetFreeItems()
        {
            List<Item> items = new List<Item>();
            foreach (Feature f in GetFeatures())
            {
                if (f is ItemChoiceConditionFeature) items.AddRange(((ItemChoiceConditionFeature)f).getItems(this, Context));
                else if (f is ItemChoiceFeature) items.AddRange(((ItemChoiceFeature)f).getItems(this, Context));
                else if (f is FreeItemAndGoldFeature) foreach (String s in ((FreeItemAndGoldFeature)f).Items) items.Add(Context.GetItem(s, f.Source));
            }
            items.Sort();
            return items;
        }
        public int GetExtraAttacks()
        {
            int extraattacks = 0;
            foreach (Feature f in GetFeatures()) if (f is ExtraAttackFeature) extraattacks=Math.Max(extraattacks,((ExtraAttackFeature)f).ExtraAttacks);
            return extraattacks;
        }
        public string GetRaceSubName()
        {
            if (SubRaceName != null && SubRaceName != "") return SourceInvariantComparer.NoSource(SubRaceName);
            if (RaceName != null && RaceName != "") return SourceInvariantComparer.NoSource(RaceName);
            return "";
        }
        public int GetSpeed()
        {
            int extraspeed=0;
            int basespeed = 0;
            int basespeedIgnoreArmor = -10;
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item mainHand = GetMainHand();

            List<string> additionalKW = new List<string>();
            if (armor == null)
            {
                additionalKW.Add("unarmored");
                armor = new Item()
                {
                    Name = "No Armor"
                };
            }
            if (offHand == null && !(mainHand is Weapon && mainHand.Keywords.Exists(t => t.Name == "two-handed"))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            List<ACFeature> ways = new List<ACFeature>();
            var features = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is SpeedFeature);
            foreach (FeatureClass fc in features)
            {
                Feature f = fc.feature;
                if (((SpeedFeature)f).Condition != null && ((SpeedFeature)f).Condition.Length > 0)
                {
                    if (!Utils.Matches(Context, armor, ((SpeedFeature)f).Condition, fc.classlevel, additionalKW, asa)) continue;
                }
                extraspeed += Utils.Evaluate(Context, ((SpeedFeature)f).ExtraSpeed, asa, additionalKW, fc.classlevel, 0, armor);
                if (basespeed < ((SpeedFeature)f).BaseSpeed) basespeed = ((SpeedFeature)f).BaseSpeed;
                if (((SpeedFeature)f).IgnoreArmor && basespeedIgnoreArmor < ((SpeedFeature)f).BaseSpeed) basespeedIgnoreArmor = ((SpeedFeature)f).BaseSpeed;
            }
            int ArmorPenalty = 0;
            if (armor != null && armor is Armor && ((Armor)armor).StrengthRequired > asa.Strength) ArmorPenalty = 10;
            return Math.Max(basespeed + extraspeed - ArmorPenalty, basespeedIgnoreArmor + extraspeed);
        }
        public int GetVisionRange()
        {
            int range = 0;
            foreach (Feature f in GetFeatures()) if (f is VisionFeature) range += ((VisionFeature)f).Range;
            return range;
        }
        public struct FeatureClass
        {
            public Feature feature;
            public int classlevel;
        }
        private List<FeatureClass> GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, Predicate<Feature> match, int level = 0, IEnumerable<Feature> additional = null, bool reset = false)
        {
            //if (reset) ChoiceCounter.Clear();
            if (reset) ResetChoices();
            if (level == 0) level = GetLevel();
            asa = new AbilityScoreArray(BaseStrength, BaseDexterity, BaseConstitution, BaseIntelligence, BaseWisdom, BaseCharisma);
            max = new AbilityScoreArray(Context.Scores.Max, Context.Scores.Max, Context.Scores.Max, Context.Scores.Max, Context.Scores.Max, Context.Scores.Max);
            AbilityScoreArray asaset = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            AbilityScoreArray maxset = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            AbilityScoreArray bonus = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            AbilityScoreArray bonusmax = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            List<FeatureClass> found = new List<FeatureClass>();
            foreach (AbilityFeatChoice afc in GetAbilityFeatChoices(level)) {
                if (afc.Ability1 == Ability.Strength) asa.Strength++;                
                else if (afc.Ability1 == Ability.Dexterity) asa.Dexterity++;
                else if (afc.Ability1 == Ability.Constitution) asa.Constitution++;
                else if (afc.Ability1 == Ability.Intelligence) asa.Intelligence++;
                else if (afc.Ability1 == Ability.Wisdom) asa.Wisdom++;
                else if (afc.Ability1 == Ability.Charisma) asa.Charisma++;

                if (afc.Ability2 == Ability.Strength) asa.Strength++;
                else if (afc.Ability2 == Ability.Dexterity) asa.Dexterity++;
                else if (afc.Ability2 == Ability.Constitution) asa.Constitution++;
                else if (afc.Ability2 == Ability.Intelligence) asa.Intelligence++;
                else if (afc.Ability2 == Ability.Wisdom) asa.Wisdom++;
                else if (afc.Ability2 == Ability.Charisma) asa.Charisma++;
            }

            int multiclassing = 0;
            int multiclassinglevel = 0;
            if (Classes != null && Classes.Count > 0)
            {
                foreach (PlayerClass p in Classes)
                {
                    int classlevel = p.getClassLevelUpToLevel(level);
                    if (classlevel > 0)
                    {
                        foreach (Feature f in p.GetFeatures(level, this, Context))
                        {
                            if (f is AbilityScoreFeature asf)
                            {
                                AbilityScoreArray cur = asa;
                                AbilityScoreArray curset = asaset;

                                if (asf.Modifier.HasFlag(AbilityScoreModifikation.Maximum))
                                {
                                    if (asf.Modifier.HasFlag(AbilityScoreModifikation.Set))
                                    {
                                        if (asf.Modifier.HasFlag(AbilityScoreModifikation.Bonus))
                                        {
                                            bonusmax.Strength = Math.Max(bonusmax.Strength, asf.Strength);
                                            bonusmax.Constitution = Math.Max(bonusmax.Constitution, asf.Constitution);
                                            bonusmax.Dexterity = Math.Max(bonusmax.Dexterity, asf.Dexterity);
                                            bonusmax.Intelligence = Math.Max(bonusmax.Intelligence, asf.Intelligence);
                                            bonusmax.Wisdom = Math.Max(bonusmax.Wisdom, asf.Wisdom);
                                            bonusmax.Charisma = Math.Max(bonusmax.Charisma, asf.Charisma);
                                        }
                                        else
                                        {
                                            if (asf.Strength > curset.Strength) maxset.Strength = asf.Strength;
                                            if (asf.Dexterity > curset.Dexterity) maxset.Dexterity = asf.Dexterity;
                                            if (asf.Constitution > curset.Constitution) maxset.Constitution = asf.Constitution;
                                            if (asf.Wisdom > curset.Wisdom) maxset.Wisdom = asf.Wisdom;
                                            if (asf.Intelligence > curset.Intelligence) maxset.Intelligence = asf.Intelligence;
                                            if (asf.Charisma > curset.Charisma) maxset.Charisma = asf.Charisma;
                                        }
                                    }
                                    else
                                    {
                                        if (asf.Modifier.HasFlag(AbilityScoreModifikation.Bonus))
                                        {
                                            bonusmax.Strength += asf.Strength;
                                            bonusmax.Constitution += asf.Constitution;
                                            bonusmax.Dexterity += asf.Dexterity;
                                            bonusmax.Intelligence += asf.Intelligence;
                                            bonusmax.Wisdom += asf.Wisdom;
                                            bonusmax.Charisma += asf.Charisma;
                                        }
                                        else
                                        {
                                            max.Strength += asf.Strength;
                                            max.Constitution += asf.Constitution;
                                            max.Dexterity += asf.Dexterity;
                                            max.Intelligence += asf.Intelligence;
                                            max.Wisdom += asf.Wisdom;
                                            max.Charisma += asf.Charisma;
                                        }
                                    }
                                }
                                else
                                {
                                    if (asf.Modifier.HasFlag(AbilityScoreModifikation.Set))
                                    {
                                        if (asf.Modifier.HasFlag(AbilityScoreModifikation.Bonus))
                                        {
                                            bonus.Strength = Math.Max(bonus.Strength, asf.Strength);
                                            bonus.Constitution = Math.Max(bonus.Constitution, asf.Constitution);
                                            bonus.Dexterity = Math.Max(bonus.Dexterity, asf.Dexterity);
                                            bonus.Intelligence = Math.Max(bonus.Intelligence, asf.Intelligence);
                                            bonus.Wisdom = Math.Max(bonus.Wisdom, asf.Wisdom);
                                            bonus.Charisma = Math.Max(bonus.Charisma, asf.Charisma);
                                        }
                                        else
                                        {
                                            if (asf.Strength > curset.Strength) curset.Strength = asf.Strength;
                                            if (asf.Dexterity > curset.Dexterity) curset.Dexterity = asf.Dexterity;
                                            if (asf.Constitution > curset.Constitution) curset.Constitution = asf.Constitution;
                                            if (asf.Wisdom > curset.Wisdom) curset.Wisdom = asf.Wisdom;
                                            if (asf.Intelligence > curset.Intelligence) curset.Intelligence = asf.Intelligence;
                                            if (asf.Charisma > curset.Charisma) curset.Charisma = asf.Charisma;
                                        }
                                    }
                                    else
                                    {
                                        if (asf.Modifier.HasFlag(AbilityScoreModifikation.Bonus))
                                        {
                                            bonus.Strength += asf.Strength;
                                            bonus.Constitution += asf.Constitution;
                                            bonus.Dexterity += asf.Dexterity;
                                            bonus.Intelligence += asf.Intelligence;
                                            bonus.Wisdom += asf.Wisdom;
                                            bonus.Charisma += asf.Charisma;
                                        }
                                        else
                                        {
                                            cur.Strength += asf.Strength;
                                            cur.Constitution += asf.Constitution;
                                            cur.Dexterity += asf.Dexterity;
                                            cur.Intelligence += asf.Intelligence;
                                            cur.Wisdom += asf.Wisdom;
                                            cur.Charisma += asf.Charisma;
                                        }
                                    }
                                }
                            }
                            else if (match.Invoke(f)) found.Add(new FeatureClass { feature = f, classlevel = classlevel });
                        }
                        multiclassing++;
                        multiclassinglevel += p.getMulticlassingLevel(Context, level);
                    }
                }
                if (multiclassinglevel < 1) multiclassinglevel = 1;
                
            }

            List<Feature> feats = new List<Feature>();
            feats.AddRange(GetBackgroundFeatures(level, false));
            feats.AddRange(GetRaceFeatures(level, false));
            //feats.AddRange(getClassFeatures(level, false));
            feats.AddRange(GetCommonFeaturesAndFeats(level, false));
            if (multiclassing > 1)
            {
                List<Feature> res = new List<Feature>();
                foreach (Feature f in Context.Config.MultiClassFeatures)
                    res.AddRange(f.Collect(multiclassinglevel, this, Context));
                feats.AddRange(Context.Plugins.FilterClassFeatures(null, multiclassinglevel, res, level, this, Context));
            }
            if (additional!=null) feats.AddRange(additional);
            foreach (Feature f in feats)
            {
                if (f is AbilityScoreFeature asf)
                {
                    AbilityScoreArray cur = asa;
                    AbilityScoreArray curset = asaset;
                    if (asf.Modifier.HasFlag(AbilityScoreModifikation.Maximum))
                    {
                        if (asf.Modifier.HasFlag(AbilityScoreModifikation.Set))
                        {
                            if (asf.Strength > curset.Strength) maxset.Strength = asf.Strength;
                            if (asf.Dexterity > curset.Dexterity) maxset.Dexterity = asf.Dexterity;
                            if (asf.Constitution > curset.Constitution) maxset.Constitution = asf.Constitution;
                            if (asf.Wisdom > curset.Wisdom) maxset.Wisdom = asf.Wisdom;
                            if (asf.Intelligence > curset.Intelligence) maxset.Intelligence = asf.Intelligence;
                            if (asf.Charisma > curset.Charisma) maxset.Charisma = asf.Charisma;
                        }
                        else
                        {
                            max.Strength += asf.Strength;
                            max.Constitution += asf.Constitution;
                            max.Dexterity += asf.Dexterity;
                            max.Intelligence += asf.Intelligence;
                            max.Wisdom += asf.Wisdom;
                            max.Charisma += asf.Charisma;
                        }
                    }
                    else
                    {
                        if (asf.Modifier.HasFlag(AbilityScoreModifikation.Set))
                        {
                            if (asf.Strength > curset.Strength) curset.Strength = asf.Strength;
                            if (asf.Dexterity > curset.Dexterity) curset.Dexterity = asf.Dexterity;
                            if (asf.Constitution > curset.Constitution) curset.Constitution = asf.Constitution;
                            if (asf.Wisdom > curset.Wisdom) curset.Wisdom = asf.Wisdom;
                            if (asf.Intelligence > curset.Intelligence) curset.Intelligence = asf.Intelligence;
                            if (asf.Charisma > curset.Charisma) curset.Charisma = asf.Charisma;

                        }
                        else
                        {
                            cur.Strength += asf.Strength;
                            cur.Constitution += asf.Constitution;
                            cur.Dexterity += asf.Dexterity;
                            cur.Intelligence += asf.Intelligence;
                            cur.Wisdom += asf.Wisdom;
                            cur.Charisma += asf.Charisma;
                        }
                    }
                }
                else if (match.Invoke(f)) found.Add(new FeatureClass { feature = f, classlevel = level });
            }
            max.Max(asaset);
            asa.Max(asaset);
            max.Max(maxset);
            max.Add(bonusmax);
            asa.Add(bonus);
            asa.Min(max);
            return found;
        }
        public int GetStrength()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.Strength;
        }
        public int GetDexterity()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.Dexterity;
        }
        public int GetConstitution()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.Constitution;
        }
        public int GetIntelligence()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.Intelligence;
        }
        public int GetWisdom()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.Wisdom;
        }
        public int GetCharisma()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.Charisma;
        }
        public int GetStrengthMod()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.StrMod;
        }
        public int GetDexterityMod()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.DexMod;
        }

        public int GetConstitutionMod()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.ConMod;
        }
        public int GetIntelligenceMod()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.IntMod;
        }
        public int GetWisdomMod()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.WisMod;
        }
        public int GetCharismaMod()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa.ChaMod;
        }
        public int GetHitpointMax()
        {
            int hp = 0;
            int level=GetLevel();
            int hpperlevel=GetConstitutionMod();
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is HitPointsFeature);
            foreach (PlayerClass p in Classes)
            {
                hp += p.getHP(p.getClassLevelUpToLevel(level));
            }
            foreach (FeatureClass fc in fa)
            {
                hp += Utils.Evaluate(Context, fc.feature as HitPointsFeature, asa, null, fc.classlevel, level);
            }
            return hp + hpperlevel * level + BonusMaxHP;
        }
        public List<HitDie> GetHitDie()
        {
            List<int> hd = new List<int>();
            List<HitDie> hitdie = new List<HitDie>();
            int level=GetLevel();
            foreach (PlayerClass p in Classes)
            {
                while (hd.Count <= p.GetClass(Context).HitDie) hd.Add(0);
                hd[p.GetClass(Context).HitDie] += p.getClassLevelUpToLevel(level) * Math.Max(1, p.GetClass(Context).HitDieCount);
            }
            for (int h = 0; h < hd.Count; h++) if (hd[h] > 0) hitdie.Add(new HitDie(h, hd[h], (UsedHitDice.Count > h?UsedHitDice[h]:0)));
            return hitdie;
        }

        public AbilityScoreArray GetFinalAbilityScores()
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => false);
            return asa;
        }
        public AbilityScoreArray GetFinalAbilityScores(out AbilityScoreArray max)
        {
            GetFeatureAndAbility(out AbilityScoreArray asa, out max, t => false);
            return asa;
        }
        public int GetSave(Ability ab)
        {
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is SaveProficiencyFeature);
            Ability saves = Ability.None;
            foreach (FeatureClass f in fa) saves |= ((SaveProficiencyFeature)f.feature).Ability;
            int value = asa.Apply(ab);
            if ((ab & saves) != Ability.None) value += GetProficiency();
            return value;
        }
        public List<SkillInfo> GetSkills()
        {
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is SkillProficiencyChoiceFeature || t is SkillProficiencyFeature || t is BonusFeature);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            Dictionary<Skill, double> ProfModifier = new Dictionary<Skill, double>();
            Dictionary<Skill, double> ProfModifierAlwaysBonus = new Dictionary<Skill, double>();
            Dictionary<Skill, double> ProfModifierOnlyBonus = new Dictionary<Skill, double>();
            Dictionary<Skill, int> res = new Dictionary<Skill, int>();
            foreach (Skill s in Context.Skills.Values) res.Add(s, 0);
            int generalBonus=0;
            double generalModifier = 0;
            double generalAlwaysModifier = 0;
            double generalOnlyModifier = 0;
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is SkillProficiencyChoiceFeature spcf)
                {
                    foreach (Skill s in spcf.getSkills(this, Context))
                    {
                        double mod = spcf.ProficiencyMultiplier;
                        switch (spcf.BonusType)
                        {
                            case ProficiencyBonus.AddOnlyIfNotProficient:
                                if (!ProfModifier.ContainsKey(s)) ProfModifier.Add(s, mod);
                                else if (ProfModifier[s] < mod) ProfModifier[s] = mod;
                                break;
                            case ProficiencyBonus.AddOnlyIfProficient:
                                if (!ProfModifierOnlyBonus.ContainsKey(s)) ProfModifierOnlyBonus.Add(s, mod);
                                else if (ProfModifierOnlyBonus[s] < mod) ProfModifierOnlyBonus[s] = mod;
                                break;
                            case ProficiencyBonus.AddRegardless:
                                if (!ProfModifierAlwaysBonus.ContainsKey(s)) ProfModifierAlwaysBonus.Add(s, mod);
                                else if (ProfModifierAlwaysBonus[s] < mod) ProfModifierAlwaysBonus[s] = mod;
                                break;
                        }
                    }
                }
                else if (f is SkillProficiencyFeature spf)
                {
                    double mod = spf.ProficiencyMultiplier;
                    if (spf.Skills.Count == 0)
                    {
                        switch (spf.BonusType)
                        {
                            case ProficiencyBonus.AddOnlyIfNotProficient:
                                if (generalModifier < mod) generalModifier = mod;
                                break;
                            case ProficiencyBonus.AddOnlyIfProficient:
                                if (generalModifier < mod) generalModifier = mod;
                                break;
                            case ProficiencyBonus.AddRegardless:
                                if (generalModifier < mod) generalModifier = mod;
                                break;
                        }
                    }
                    foreach (String sst in spf.Skills)
                    {
                        Skill s = Context.GetSkill(sst, f.Source);
                        switch (spf.BonusType)
                        {
                            case ProficiencyBonus.AddOnlyIfNotProficient:
                                if (!ProfModifier.ContainsKey(s)) ProfModifier.Add(s, mod);
                                else if (ProfModifier[s] < mod) ProfModifier[s] = mod;
                                break;
                            case ProficiencyBonus.AddOnlyIfProficient:
                                if (!ProfModifierOnlyBonus.ContainsKey(s)) ProfModifierOnlyBonus.Add(s, mod);
                                else if (ProfModifierOnlyBonus[s] < mod) ProfModifierOnlyBonus[s] = mod;
                                break;
                            case ProficiencyBonus.AddRegardless:
                                if (!ProfModifierAlwaysBonus.ContainsKey(s)) ProfModifierAlwaysBonus.Add(s, mod);
                                else if (ProfModifierAlwaysBonus[s] < mod) ProfModifierAlwaysBonus[s] = mod;
                                break;
                        }
                    }
                }
                else if (f is BonusFeature bf && !bf.SkillPassive && bf.SkillBonus != null && bf.SkillBonus.Trim() != "" && bf.SkillBonus.Trim() != "0" && Utils.Matches(Context, bf, armor, fc.classlevel, additionalKW, asa, true))
                { 
                    int v = Utils.Evaluate(Context, bf.SkillBonus, asa, additionalKW, fc.classlevel, 0);
                    if (bf.Skills.Count == 0) generalBonus += v;
                    foreach (string s in bf.Skills) res[Context.GetSkill(s, f.Source)] += v;
                }
            }
            int prof = GetProficiency();
            List<SkillInfo> result = new List<SkillInfo>();
            foreach (Skill s in Context.Skills.Values)
            {
                double multiplier = generalModifier + generalAlwaysModifier;
                if (multiplier > 0.999) multiplier += generalOnlyModifier;
                if (ProfModifier.ContainsKey(s) && ProfModifier[s]>multiplier) multiplier=ProfModifier[s];
                if (ProfModifierAlwaysBonus.ContainsKey(s)) multiplier += ProfModifierAlwaysBonus[s];
                if (multiplier > 0.999 && ProfModifierOnlyBonus.ContainsKey(s)) multiplier += ProfModifierOnlyBonus[s];
                res[s]+=asa.ApplyMod(s.Base) + (int)Math.Floor(prof * multiplier);
                result.Add(new SkillInfo(s, res[s], s.Base));
            }
            result.Sort();
            return result;
        }
        public int GetSkill(Skill s, Ability ability=Ability.None)
        {
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is SkillProficiencyChoiceFeature || t is SkillProficiencyFeature || t is BonusFeature);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            double modifier = 0;
            double onlymodifier = 0;
            double alwaysmodifier = 0;
            int bonus = 0;
            if (ability == Ability.None) ability = s.Base;
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is SkillProficiencyChoiceFeature spcf)
                {
                    if (spcf.getSkills(this, Context).Contains(s))
                    {
                        switch (spcf.BonusType)
                        {
                            case ProficiencyBonus.AddOnlyIfNotProficient:
                                if (modifier < spcf.ProficiencyMultiplier) modifier = spcf.ProficiencyMultiplier;
                                break;
                            case ProficiencyBonus.AddOnlyIfProficient:
                                if (onlymodifier < spcf.ProficiencyMultiplier) onlymodifier = spcf.ProficiencyMultiplier;
                                break;
                            case ProficiencyBonus.AddRegardless:
                                if (alwaysmodifier < spcf.ProficiencyMultiplier) alwaysmodifier = spcf.ProficiencyMultiplier;
                                break;
                        }
                    }
                }
                else if (f is SkillProficiencyFeature spf)
                {
                    double mod = spf.ProficiencyMultiplier;
                    switch (spf.BonusType)
                    {
                        case ProficiencyBonus.AddOnlyIfNotProficient:
                            if (spf.Skills.Count == 0) { if (modifier < mod) modifier = mod; }
                            else if (spf.Skills.Contains(s.Name)) if (modifier < mod) modifier = mod;
                            break;
                        case ProficiencyBonus.AddOnlyIfProficient:
                            if (spf.Skills.Count == 0) { if (onlymodifier < mod) onlymodifier = mod; }
                            else if (spf.Skills.Contains(s.Name)) if (onlymodifier < mod) onlymodifier = mod;
                            break;
                        case ProficiencyBonus.AddRegardless:
                            if (spf.Skills.Count == 0) { if (alwaysmodifier < mod) alwaysmodifier = mod; }
                            else if (spf.Skills.Contains(s.Name)) if (alwaysmodifier < mod) alwaysmodifier = mod;
                            break;
                    }
                }
                else if (f is BonusFeature bf && !bf.SkillPassive && bf.SkillBonus != null && bf.SkillBonus.Trim() != "" && bf.SkillBonus.Trim() != "0" && Utils.Matches(Context, bf, armor, fc.classlevel, additionalKW, asa, true))
                {
                    if (bf.Skills.Count == 0 || bf.Skills.Contains(s.Name)) bonus = Utils.Evaluate(Context, bf.SkillBonus, asa, additionalKW, fc.classlevel, 0);
                }
            }
            int prof = GetProficiency();
            if (modifier > 0.999) modifier += onlymodifier;
            modifier += alwaysmodifier;
            return asa.ApplyMod(ability) + (int)Math.Floor(prof * modifier) + bonus;
        }
        public int GetPassiveSkill(Skill s)
        {
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is SkillProficiencyChoiceFeature || t is SkillProficiencyFeature || t is BonusFeature);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            double modifier = 0;
            double onlymodifier = 0;
            double alwaysmodifier = 0;
            int bonus = 0;
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is SkillProficiencyChoiceFeature spcf)
                {
                    if (spcf.getSkills(this, Context).Contains(s))
                    {
                        switch (spcf.BonusType)
                        {
                            case ProficiencyBonus.AddOnlyIfNotProficient:
                                if (modifier < spcf.ProficiencyMultiplier) modifier = spcf.ProficiencyMultiplier;
                                break;
                            case ProficiencyBonus.AddOnlyIfProficient:
                                if (onlymodifier < spcf.ProficiencyMultiplier) onlymodifier = spcf.ProficiencyMultiplier;
                                break;
                            case ProficiencyBonus.AddRegardless:
                                if (alwaysmodifier < spcf.ProficiencyMultiplier) alwaysmodifier = spcf.ProficiencyMultiplier;
                                break;
                        }
                    }
                }
                else if (f is SkillProficiencyFeature spf)
                {
                    double mod = spf.ProficiencyMultiplier;
                    switch (spf.BonusType)
                    {
                        case ProficiencyBonus.AddOnlyIfNotProficient:
                            if (spf.Skills.Count == 0) { if (modifier < mod) modifier = mod; }
                            else if (spf.Skills.Contains(s.Name)) if (modifier < mod) modifier = mod;
                            break;
                        case ProficiencyBonus.AddOnlyIfProficient:
                            if (spf.Skills.Count == 0) { if (onlymodifier < mod) onlymodifier = mod; }
                            else if (spf.Skills.Contains(s.Name)) if (onlymodifier < mod) onlymodifier = mod;
                            break;
                        case ProficiencyBonus.AddRegardless:
                            if (spf.Skills.Count == 0) { if (alwaysmodifier < mod) alwaysmodifier = mod; }
                            else if (spf.Skills.Contains(s.Name)) if (alwaysmodifier < mod) alwaysmodifier = mod;
                            break;
                    }
                }
                else if (f is BonusFeature bf && bf.SkillBonus != null && bf.SkillBonus.Trim() != "" && bf.SkillBonus.Trim() != "0" && Utils.Matches(Context, bf, armor, fc.classlevel, additionalKW, asa, true))
                {
                    if (bf.Skills.Count == 0 || bf.Skills.Contains(s.Name)) bonus = Utils.Evaluate(Context, bf.SkillBonus, asa, additionalKW, fc.classlevel, 0);
                }
            }
            int prof = GetProficiency();
            Dictionary<Skill, int> res = new Dictionary<Skill, int>();
            return 10 + bonus + asa.ApplyMod(s.Base) + (int)Math.Floor(prof * modifier);
        }

        public AbilityScoreArray GetSavingThrowsBoni()
        {
            AbilityScoreArray res = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is BonusFeature);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");

            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).SavingThrowBonus != null && ((BonusFeature)fc.feature).SavingThrowBonus.Trim() != "" && ((BonusFeature)fc.feature).SavingThrowBonus.Trim() != "0" && Utils.Matches(Context, ((BonusFeature)fc.feature).Condition, asa, additionalKW, fc.classlevel, 0))
                {
                    res.AddBonus(Utils.Evaluate(Context, null, ((BonusFeature)fc.feature).SavingThrowBonus, asa, additionalKW, fc.classlevel), ((BonusFeature)fc.feature).SavingThrowAbility);

                }
            return res;
        }

        public Dictionary<Ability,int> GetSaves()
        {
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is SaveProficiencyFeature || t is BonusFeature);
            AbilityScoreArray bonus = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            Ability saves = Ability.None;
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).SavingThrowBonus != null && ((BonusFeature)fc.feature).SavingThrowBonus.Trim() != "" && ((BonusFeature)fc.feature).SavingThrowBonus.Trim() != "0" && Utils.Matches(Context, ((BonusFeature)fc.feature).Condition, asa, additionalKW, fc.classlevel, 0))
                {
                    bonus.AddBonus(Utils.Evaluate(Context, null, ((BonusFeature)fc.feature).SavingThrowBonus, asa, additionalKW, fc.classlevel), ((BonusFeature)fc.feature).SavingThrowAbility);

                }
            Dictionary<Ability,int> res=new Dictionary<Ability,int>();
            foreach (FeatureClass f in fa)
            {
                if (f.feature is SaveProficiencyFeature) saves |= ((SaveProficiencyFeature)f.feature).Ability;
            }
            int prof = GetProficiency();
            res.Add(Ability.Strength, asa.StrMod + (saves.HasFlag(Ability.Strength) ? prof : 0) + bonus.Strength);
            res.Add(Ability.Dexterity, asa.DexMod + (saves.HasFlag(Ability.Dexterity) ? prof : 0) + bonus.Dexterity);
            res.Add(Ability.Constitution, asa.ConMod + (saves.HasFlag(Ability.Constitution) ? prof : 0) + bonus.Constitution);
            res.Add(Ability.Intelligence, asa.IntMod + (saves.HasFlag(Ability.Intelligence) ? prof : 0) + bonus.Intelligence);
            res.Add(Ability.Wisdom, asa.WisMod + (saves.HasFlag(Ability.Wisdom) ? prof : 0) + bonus.Wisdom);
            res.Add(Ability.Charisma, asa.ChaMod + (saves.HasFlag(Ability.Charisma) ? prof : 0) + bonus.Charisma);
            return res;
        }
        public int GetAC(int level = 0)
        {
            if (level == 0) level = GetLevel();
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is ACFeature || t is BonusFeature);
            Item armor=GetArmor();
            Item offHand=GetOffHand();
            Item mainHand=GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null)
            {
                additionalKW.Add("unarmored");
                armor = new Item()
                {
                    Name = "No Armor"
                };
            }
            if (offHand == null && !(mainHand is Weapon && mainHand.Keywords.Exists(t=>t.Name=="two-handed"))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (mainHand is Weapon) additionalKW.Add("mainhand");
            if (offHand is Shield) additionalKW.Add("shield");
            Dictionary<ACFeature, int> ways = new Dictionary<ACFeature, int>(new ObjectIdentityEqualityComparer());
            
            int bonus=0;
            foreach (FeatureClass fc in fa) {
                Feature f = fc.feature;
                if (f is ACFeature) ways.Add((ACFeature)f, fc.classlevel);
                if (f is BonusFeature && ((BonusFeature)f).ACBonus != null && ((BonusFeature)f).ACBonus.Trim() != "" && ((BonusFeature)f).ACBonus.Trim() != "0")
                {
                    BonusFeature b = (BonusFeature)f;
                    if (Utils.Matches(Context, b, armor, fc.classlevel, additionalKW, asa)) bonus+=Utils.Evaluate(Context, b.ACBonus, asa, additionalKW, fc.classlevel, level, armor);
                }
            }
            int AC = 0;
            int shieldbonus = 0;
            if (mainHand is Shield) shieldbonus = ((Shield)offHand).ACBonus;
            if (offHand is Shield && shieldbonus < ((Shield)offHand).ACBonus) shieldbonus = ((Shield)offHand).ACBonus;
            foreach (KeyValuePair<ACFeature, int> acf in ways)
            {
                int tac = Utils.CalcAC(Context, acf.Key,armor,shieldbonus,additionalKW, asa, bonus, acf.Value);
                if (AC < tac) AC = tac;
            }
            return AC + bonus;
        }
        public Item GetArmor()
        {
            foreach (Possession p in Possessions) if (string.Equals(p.Equipped, EquipSlot.Armor, StringComparison.OrdinalIgnoreCase)) return p.Item;
            return null;
        }
        public Item GetMainHand()
        {
            foreach (Possession p in Possessions) if (string.Equals(p.Equipped, EquipSlot.MainHand, StringComparison.OrdinalIgnoreCase)) return p.Item;
            return null;
        }
        public Item GetOffHand()
        {
            foreach (Possession p in Possessions) if (string.Equals(p.Equipped, EquipSlot.OffHand, StringComparison.OrdinalIgnoreCase)) return p.Item;
            return null;
        }
        public AttackInfo GetAttack(Possession p,int level=0)
        {
            if (level == 0) level = GetLevel();
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is BonusFeature || t is ExtraAttackFeature || t is ToolKWProficiencyFeature || t is ToolProficiencyChoiceConditionFeature || t is ToolProficiencyFeature, level, p.CollectOnUse(level, this, Context));
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item mainHand = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (mainHand is Weapon) additionalKW.Add("mainhand");
            List<Weapon> countsAs = new List<Weapon>();
            Weapon weapon = null;
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is BonusFeature b)
                {
                    if ((b.BaseItemChange != null && b.BaseItemChange.Trim() != ""))
                    {
                        if (Utils.Matches(Context, b, Ability.Strength | Ability.Dexterity, "Weapon", "Weapon", fc.classlevel, additionalKW) || (p.Item != null && Utils.Matches(Context, b, p.Item, fc.classlevel, additionalKW, asa)))
                        {
                            try
                            {
                                Item baseitem = Context.GetItem(((BonusFeature)f).BaseItemChange, ((BonusFeature)f).Source);
                                if (baseitem is Weapon) weapon = baseitem as Weapon;
                                break;
                            }
                            catch (Exception) { }
                        }
                    }
                    if ((b.ProficiencyOptions != null && b.ProficiencyOptions.Count > 0))
                    {
                        if (Utils.Matches(Context, b, Ability.Strength | Ability.Dexterity, "Weapon", "Weapon", fc.classlevel, additionalKW))
                        {
                            foreach (String i in b.ProficiencyOptions)
                            {
                                try
                                {
                                    Item it = Context.GetItem(i, ((BonusFeature)f).Source);
                                    if (it is Weapon) weapon = it as Weapon;
                                    break;
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }
            }
            if (weapon == null && (p.Item == null || !(p.Item is Weapon)))
            {
                return null;
            }
            if (weapon == null)
            {
                weapon = p.Item as Weapon;
            }
            countsAs.Add(weapon);
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k=>k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            Ability baseAbility = Ability.Strength;
            

            if (weapon.Keywords.Exists(kw => kw.Name == "finesse")) baseAbility = baseAbility | Ability.Dexterity;
            if (weapon.Keywords.Exists(kw => kw.Name == "ranged"))
            {
                baseAbility = Ability.Dexterity;
            }
            int attackbonus = 0;
            int damagebonus = 0;
            bool profbonus = false;
            int extraatk = 0;
            String damage = weapon.Damage;
            string damagetype = weapon.DamageType;
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is BonusFeature b)
                {
                    if ((b.DamageBonus != null && b.DamageBonus.Trim() != "" && b.DamageBonus.Trim() != "0") || (b.DamageBonusText != null && b.DamageBonusText != "") || b.DamageBonusModifier != Ability.None || (b.AttackBonus != null && b.AttackBonus.Trim() != "" && b.AttackBonus.Trim() != "0"))
                    {
                        if (Utils.Matches(Context, b, weapon, fc.classlevel, additionalKW, asa) || Utils.Matches(Context, b, baseAbility, "Weapon", "Weapon", fc.classlevel, additionalKW))
                        {
                            attackbonus += Utils.Evaluate(Context, b.AttackBonus, asa, additionalKW, fc.classlevel, level, weapon);
                            damagebonus += Utils.Evaluate(Context, b, asa, additionalKW, fc.classlevel, level, weapon);
                            if (b.DamageBonusText != null && b.DamageBonusText != "") damage += " " + b.DamageBonusText;
                            if (b.BaseAbility != Ability.None) baseAbility = baseAbility | b.BaseAbility;
                        }
                    }
                }
                else if (f is ExtraAttackFeature) extraatk = ((ExtraAttackFeature)f).ExtraAttacks;
                else if (f is ToolProficiencyFeature)
                {
                    foreach (Weapon w in countsAs)
                    {
                        if (((ToolProficiencyFeature)f).Tools.Exists(t => t.ToLowerInvariant() == w.Name.ToLowerInvariant())) profbonus = true;
                    }
                }
                else if (f is ToolProficiencyChoiceConditionFeature)
                {
                    foreach (Weapon w in countsAs)
                    {
                        if (((ToolProficiencyChoiceConditionFeature)f).getTools(this, Context).Exists(t => t.Name.ToLowerInvariant() == w.Name.ToLowerInvariant())) profbonus = true;
                    }
                }
                else if (f is ToolKWProficiencyFeature)
                {
                    foreach (Weapon w in countsAs)
                    {
                        if (Utils.Matches(Context, ((ToolKWProficiencyFeature)f), w, fc.classlevel)) profbonus = true;
                    }
                }
            }
            int ability = asa.ApplyMod(baseAbility); ;
            attackbonus += ability;
            damagebonus += ability;
            if (profbonus) attackbonus += GetProficiency(level);
            if (damagebonus > 0) damage += "+" + damagebonus;
            else if (damagebonus < 0) damage += damagebonus;
            extraatk++;
            if (extraatk > 1) damage = extraatk.ToString() + " x " + damage;
            return new AttackInfo(attackbonus,damage,damagetype);
        }
        public AttackInfo GetAttack(Spell s, Ability spellcastingModifier, int level = 0)
        {
            if (level == 0) level = GetLevel();
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is BonusFeature, level);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (GetMainHand() is Weapon) additionalKW.Add("mainhand");
            additionalKW.Add("Spell");
            int attackbonus = asa.ApplyMod(spellcastingModifier);
            attackbonus += GetProficiency(level);
            int savedc = 8 + attackbonus;
            int damagebonus = 0;
            String damage = s.GetCantripDamage(level);
            string damagetype = s.GetDamageType();
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is BonusFeature b)
                {
                    if ((b.DamageBonus != null && b.DamageBonus.Trim() != "" && b.DamageBonus.Trim() != "0") || (b.DamageBonusText != null && b.DamageBonusText != "") || b.DamageBonusModifier != Ability.None || (b.AttackBonus != null && b.AttackBonus.Trim() != "" && b.AttackBonus.Trim() != "0") || (b.SaveDCBonus != null && b.SaveDCBonus.Trim() != "" && b.SaveDCBonus.Trim() != "0"))
                    {
                        if (Utils.Matches(Context, b, s, fc.classlevel, additionalKW) || Utils.Matches(Context, b, spellcastingModifier, "Spell", "Spell", fc.classlevel, additionalKW))
                        {
                            attackbonus += Utils.Evaluate(Context, s, b.AttackBonus, asa, additionalKW, fc.classlevel, level);
                            damagebonus += Utils.Evaluate(Context, s, b, asa, additionalKW, fc.classlevel, level);
                            savedc += Utils.Evaluate(Context, s, b.SaveDCBonus, asa, additionalKW, fc.classlevel, level);
                            damagebonus += asa.ApplyMod(b.DamageBonusModifier);
                            if (b.DamageBonusText != null && b.DamageBonusText != "") damage += "+" + b.DamageBonusText;
                        }
                    }
                }
            }
            if (damagebonus > 0) damage += "+" + damagebonus;
            else if (damagebonus < 0) damage += damagebonus;
            if (Utils.Matches(Context, s, "Save", null)) return new AttackInfo(savedc.ToString(), damage, damagetype);
            else if (Utils.Matches(Context, s, "Attack", null)) return new AttackInfo(attackbonus, damage, damagetype);
            else return null;
        }
        public int GetInitiative(int level = 0)
        {
            if (level == 0) level = GetLevel();
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is BonusFeature);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            Ability baseAbility = Ability.Dexterity;
            int bonus = 0;
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).InitiativeBonus != null && ((BonusFeature)fc.feature).InitiativeBonus.Trim() != "" && ((BonusFeature)fc.feature).InitiativeBonus.Trim() != "0" && Utils.Matches(Context, fc.feature as BonusFeature, weapon, fc.classlevel, additionalKW, asa)) bonus += Utils.Evaluate(Context, ((BonusFeature)fc.feature).InitiativeBonus, asa, null, fc.classlevel, level);
            return asa.ApplyMod(baseAbility) + bonus;
        }
        public int GetLevel()
        {
            return Context.Levels.Get(GetXP());
        }
        public int GetProficiency(int level = 0)
        {
            if (level == 0) level = GetLevel();
            int prof = Context.Levels.GetProficiency(level);
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is BonusFeature);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            int bonus = 0;
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).ProficiencyBonus != null && ((BonusFeature)fc.feature).ProficiencyBonus.Trim() != "" && ((BonusFeature)fc.feature).ProficiencyBonus.Trim() != "0" && Utils.Matches(Context, fc.feature as BonusFeature, weapon, fc.classlevel, additionalKW, asa)) bonus += Utils.Evaluate(Context, ((BonusFeature)fc.feature).ProficiencyBonus, asa, null, fc.classlevel, level);
            return prof + bonus;
        }
        public int GetSpellSaveDC(string SpellcastingID, Ability baseAbility) {
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is BonusFeature);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            additionalKW.Add("Spell");
            int bonus = 0;
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).SaveDCBonus != null && ((BonusFeature)fc.feature).SaveDCBonus.Trim() != "" && ((BonusFeature)fc.feature).SaveDCBonus.Trim() != "0" && Utils.Matches(Context, ((BonusFeature)fc.feature), baseAbility, SpellcastingID, "Spell", fc.classlevel)) bonus += Utils.Evaluate(Context, null, ((BonusFeature)fc.feature).SaveDCBonus, asa, additionalKW, fc.classlevel);
            return 8+GetProficiency()+  asa.ApplyMod(baseAbility) + bonus;
        }
        public int GetSpellAttack(string SpellcastingID, Ability baseAbility)
        {
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is BonusFeature);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            additionalKW.Add("Spell");
            int bonus = 0;
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).AttackBonus != null && ((BonusFeature)fc.feature).AttackBonus.Trim() != "" && ((BonusFeature)fc.feature).AttackBonus.Trim() != "0" && Utils.Matches(Context, ((BonusFeature)fc.feature), baseAbility, SpellcastingID, "Spell", fc.classlevel, additionalKW)) bonus += Utils.Evaluate(Context, null, ((BonusFeature)fc.feature).AttackBonus, asa, additionalKW, fc.classlevel);
            return GetProficiency() + asa.ApplyMod(baseAbility) + bonus;
        }

        public void Pay(Price price)
        {
            CP -= price.cp;
            SP -= price.sp;
            EP -= price.ep;
            GP -= price.gp;
            PP -= price.pp;

            while (CP < 0)
            {
                SP -= 1;
                CP += 10;
            }
            while (SP < 0)
            {
                GP -= 1;
                SP += 10;
            }
            while (EP < 0)
            {
                GP -= 1;
                EP += 2;
            }
            while (GP < 0 && PP>0)
            {
                PP -= 1;
                GP += 10;
            }

        }


        public void UseHitDie(int dice)
        {
            if (dice < 1) return;
            while (UsedHitDice.Count <= dice) UsedHitDice.Add(0);
            UsedHitDice[dice]++;
        }

        public bool Matches(String expression, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            return Utils.Matches(Context, expression, new AbilityScoreArray(BaseStrength, BaseDexterity, BaseConstitution, BaseIntelligence, BaseWisdom, BaseCharisma), additionalKeywords, classlevel, level);
        }

        public int GetXP(bool onlyJournal = false)
        {
            int x = onlyJournal ? 0 : XP;
            foreach (JournalEntry e in ComplexJournal)
            {
                x += e.XP;
            }
            return x;
        }
        public void SetXP(int xp)
        {
            foreach (JournalEntry e in ComplexJournal)
            {
                xp -= e.XP;
            }
            XP = xp;
        }

        public void RemoveBoon(Feature feature)
        {
            string boon = feature.Name + " " + ConfigManager.SourceSeperator + " " + feature.Source;
            int index = Boons.FindIndex(s => ConfigManager.SourceInvariantComparer.Equals(s, boon));
            if (index >= 0) Boons.RemoveAt(index);
        }
        public List<TableDescription> CollectTables()
        {
            List<TableDescription> res = new List<TableDescription>();
            if (Background != null) foreach (Description d in Background.Descriptions) if (d is TableDescription) res.Add(d as TableDescription);
            if (Race != null) foreach (Description d in Race.Descriptions) if (d is TableDescription) res.Add(d as TableDescription);
            if (SubRace != null) foreach (Description d in SubRace.Descriptions) if (d is TableDescription) res.Add(d as TableDescription);
            foreach (PlayerClass pc in Classes) res.AddRange(pc.CollectTables(Context));
            return res;
        }
        public double GetCarryCapacity(int level = 0)
        {
            int size = 0;
            if (level == 0) level = GetLevel();
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is BonusFeature);
            Item armor = GetArmor();
            Item offHand = GetOffHand();
            Item weapon = GetMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.OrdinalIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            int bonus = 0;
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature bf && bf.SizeChange != 0 && Utils.Matches(Context, bf, weapon, fc.classlevel, additionalKW, asa)) bonus += bf.SizeChange;
            if (Race != null)
            {
                switch (Race.Size)
                {
                    case OGL.Base.Size.Tiny:
                        size = -2;
                        break;
                    case OGL.Base.Size.Small:
                        size = -1;
                        break;
                    case OGL.Base.Size.Medium:
                        size = 0;
                        break;
                    case OGL.Base.Size.Large:
                        size = 1;
                        break;
                    case OGL.Base.Size.Huge:
                        size = 2;
                        break;
                    case OGL.Base.Size.Gargantuan:
                        size = 3;
                        break;
                }
            }
            return asa.Strength * 15 * Math.Pow(2, size + bonus);
        }
        public IEnumerable<String> GetClassesStrings()
        {
            int l = GetLevel();
            return from c in Classes select c.ToString(Context, l);
        }
        public virtual ActionType GetActualAction(Feature f)
        {
            if (f is BonusSpellFeature || f is BonusSpellKeywordChoiceFeature)
            {
                return f.Action;
            }
            if (f.Action != ActionType.DetectAction)
            {
                return f.Action;
            }
            if (f.Text != null && Culture.CompareInfo.IndexOf(f.Text, "as a bonus action", CompareOptions.IgnoreCase) >= 0) return ActionType.BonusAction;
            if (f.Text != null && Culture.CompareInfo.IndexOf(f.Text, "take a bonus action", CompareOptions.IgnoreCase) >= 0) return ActionType.BonusAction;
            if (f.Text != null && Culture.CompareInfo.IndexOf(f.Text, "as a reaction", CompareOptions.IgnoreCase) >= 0) return ActionType.Reaction;
            if (f.Text != null && Culture.CompareInfo.IndexOf(f.Text, "take a reaction", CompareOptions.IgnoreCase) >= 0) return ActionType.Reaction;
            if (f.Text != null && Culture.CompareInfo.IndexOf(f.Text, "as a move action", CompareOptions.IgnoreCase) >= 0) return ActionType.MoveAction;
            if (f.Text != null && Culture.CompareInfo.IndexOf(f.Text, "take a move action", CompareOptions.IgnoreCase) >= 0) return ActionType.MoveAction;
            if (f.Text != null && Culture.CompareInfo.IndexOf(f.Text, "as an action", CompareOptions.IgnoreCase) >= 0) return ActionType.Action;
            if (f.Text != null && Culture.CompareInfo.IndexOf(f.Text, "take an action", CompareOptions.IgnoreCase) >= 0) return ActionType.Action;
            return ActionType.DetectAction;
        }

        public List<ActionInfo> GetActions()
        {
            var res = new List<ActionInfo>();
            foreach (var f in GetFeatures(0,false, true))
            {
                if (f is BonusSpellFeature bsf)
                {
                    ActionType a = GetActualAction(bsf);
                    if (a == ActionType.ForceHidden) continue;
                    Spell s = Context.GetSpell(bsf.Spell, bsf.Source);
                    res.Add(new ActionInfo()
                    {
                        Name = s.Name,
                        Type = a == ActionType.DetectAction ? s.Action : a,
                        Text = bsf.Text,
                        Source = bsf.Name != "" && bsf.Name != null ? bsf.Name + " - " + bsf.Source : bsf.Source,
                        Feature = f
                    });
                } else if (f is BonusSpellKeywordChoiceFeature bskcf)
                {
                    ActionType a = GetActualAction(bskcf);
                    if (a == ActionType.ForceHidden) continue;
                    foreach (var s in Utils.GetSpells(Context, bskcf)) {
                        res.Add(new ActionInfo()
                        {
                            Name = s.Name,
                            Type = a == ActionType.DetectAction ? s.Action : a,
                            Text = bskcf.Text,
                            Source = bskcf.Name != "" && bskcf.Name != null ? bskcf.Name + " - " + bskcf.Source : bskcf.Source,
                            Feature = f
                        });
                    }
                } else
                {
                    ActionType a = GetActualAction(f);
                    if (a == ActionType.ForceHidden || a == ActionType.DetectAction) continue;
                    res.Add(new ActionInfo()
                    {
                        Name = f.Name,
                        Type = a,
                        Text = f.Text,
                        Source = f.Source,
                        Feature = f
                    });
                }
            }
            return res;
        }

        public List<FormsCompanionInfo> GetFormsCompanionChoices(int level = 0)
        {
            if (level == 0) level = GetLevel();
            Dictionary<string, FormsCompanionInfo> res = new Dictionary<string, FormsCompanionInfo>();
            List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>();
            HashSet<Spell> Spells = new HashSet<Spell>(new SpellEqualityComparer());
            List<FeatureClass> fa = GetFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, t => t is SpellcastingFeature || t is BonusSpellFeature || t is BonusSpellKeywordChoiceFeature || t is FormsCompanionsFeature || t is FormsCompanionsBonusFeature);
            Dictionary<string, List<Feature>> modifiers = new Dictionary<string, List<Feature>>();
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is SpellcastingFeature scf && scf.SpellcastingID != "MULTICLASS") spellcasts.Add(scf);
                else if (f is BonusSpellFeature bsf)
                {
                    Spell s = Context.GetSpell(bsf.Spell, bsf.Source);
                    Spells.Add(new ModifiedSpell(Context.GetSpell(bsf.Spell, bsf.Source), bsf.KeywordsToAdd, bsf.SpellCastingAbility, bsf.SpellCastModifier));
                }
                else if (f is BonusSpellKeywordChoiceFeature bskcf) Spells.UnionWith((Utils.GetSpells(Context, bskcf)));
                else if (f is FormsCompanionsFeature fcf && fcf.UniqueID != null)
                {
                    if (res.ContainsKey(fcf.UniqueID))
                    {
                        FormsCompanionInfo fci = res[fcf.UniqueID];
                        fci.Count = fci.Count < 0 || fcf.FormsCompanionsCount < 0 ? -1 : fci.Count + fcf.FormsCompanionsCount;
                        fci.Options.Add(fcf.FormsCompanionsOptions);
                        if (fci.Source is Feature ff) fci.Modifiers.Add(ff);
                        fci.Source = fcf;
                        if (fci.DisplayName == null || fci.DisplayName == "") fci.DisplayName = fcf.DisplayName;
                        if (fci.SourceHint == null) fci.SourceHint = fcf.Source;
                    }
                    else
                    {
                        res.Add(fcf.UniqueID, new FormsCompanionInfo()
                        {
                            ID = fcf.UniqueID,
                            DisplayName = fcf.DisplayName,
                            Options = new List<string>() { fcf.FormsCompanionsOptions },
                            SourceHint = fcf.Source,
                            Source = fcf,
                            Count = fcf.FormsCompanionsCount,
                            ClassLevel = fc.classlevel
                        });
                    }
                }
                else if (f is FormsCompanionsBonusFeature fcbf && fcbf.UniqueID != null)
                {
                    if (modifiers.ContainsKey(fcbf.UniqueID)) modifiers[fcbf.UniqueID].Add(fcbf);
                    else modifiers.Add(fcbf.UniqueID, new List<Feature>() { fcbf });
                }
            }

            foreach (SpellcastingFeature scf in spellcasts)
            {
                if (scf.SpellcastingID != "MULTICLASS")
                {
                    Spellcasting sc = GetSpellcasting(scf.SpellcastingID);
                    int classlevel = GetClassLevel(scf.SpellcastingID);
                    if (scf.Preparation == PreparationMode.ClassList)
                    {
                        Spells.UnionWith(sc.GetAdditionalClassSpells(this, Context));
                        Spells.UnionWith(Utils.FilterSpell(Context, scf.PrepareableSpells, scf.SpellcastingID, classlevel));
                        Spells.UnionWith(sc.GetPrepared(this, Context));
                    }
                    else if (scf.Preparation == PreparationMode.Spellbook)
                    {
                        Spells.UnionWith(sc.GetSpellbook(this, Context));
                        Spells.UnionWith(sc.GetPrepared(this, Context));
                    }
                    else
                    {
                        Spells.UnionWith(sc.GetPrepared(this, Context));
                    }
                    Spells.UnionWith(sc.GetLearned(this, Context));
                }
            }
            foreach (Spell s in Spells)
            {
                if (s.FormsCompanionsFilter != null && s.FormsCompanionsFilter != "" && s.FormsCompanionsFilter != "false")
                {
                    string id = s.Name.ToLowerInvariant();
                    if (res.ContainsKey(id))
                    {
                        FormsCompanionInfo fci = res[id];
                        fci.Count = fci.Count < 0 || s.FormsCompanionsCount < 0 ? -1 : fci.Count + s.FormsCompanionsCount;
                        fci.Options.Add(s.FormsCompanionsFilter);
                        if (fci.Source is Feature f) fci.Modifiers.Add(f);
                        fci.Source = s;
                    } else
                    {
                        res.Add(id, new FormsCompanionInfo()
                        {
                            ID = id,
                            DisplayName = s.Name,
                            Options = new List<string>() { s.FormsCompanionsFilter },
                            SourceHint = s.Source,
                            Source = s,
                            Count = s.FormsCompanionsCount,
                            ClassLevel = level
                        });
                    }
                }
            }
            foreach (FormsCompanionsChoice fcc in FormsCompanionsChoices)
            {
                if (res.ContainsKey(fcc.ChoiceID))
                {
                    res[fcc.ChoiceID].Choices.AddRange(from s in fcc.FormsCompanions select Context.GetMonster(s, res[fcc.ChoiceID].SourceHint));
                }
            }
            foreach (var e in modifiers.AsEnumerable()) if (res.ContainsKey(e.Key)) res[e.Key].Modifiers.AddRange(e.Value);
            return res.Values.OrderBy(s => s.DisplayName).ToList();
        }

        public void RemoveFormCompanion(string iD, Monster m)
        {
            foreach (FormsCompanionsChoice fcc in FormsCompanionsChoices)
            {
                if (StringComparer.Ordinal.Equals(iD, fcc.ChoiceID))
                {
                    String name = m?.Name + " " + ConfigManager.SourceSeperator + " " + m?.Source;
                    if (!fcc.FormsCompanions.Remove(name))
                    {
                        int index = fcc.FormsCompanions.FindIndex(s => ConfigManager.SourceInvariantComparer.Equals(s, name));
                        if (index >= 0) fcc.FormsCompanions.RemoveAt(index);
                    }
                        
                }
            }
        }

        public void RemoveAllFormCompanion(string iD)
        {
            foreach (FormsCompanionsChoice fcc in FormsCompanionsChoices)
            {
                if (StringComparer.Ordinal.Equals(iD, fcc.ChoiceID))
                {
                    fcc.FormsCompanions.Clear();
                }
            }
        }

        public void AddFormCompanion(string iD, Monster m)
        {
            foreach (FormsCompanionsChoice fcc in FormsCompanionsChoices)
            {
                if (StringComparer.Ordinal.Equals(iD, fcc.ChoiceID))
                {
                    fcc.FormsCompanions.Add(m?.Name + " " + ConfigManager.SourceSeperator + " " + m?.Source);
                    return;
                }
            }
            FormsCompanionsChoices.Add(new FormsCompanionsChoice() { ChoiceID = iD, FormsCompanions = new List<string>() { m?.Name + " " + ConfigManager.SourceSeperator + " " + m?.Source } });
        }
    }
}
