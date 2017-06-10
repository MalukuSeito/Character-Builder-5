using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Character_Builder_5
{
    public class Player: IChoiceProvider
    {
        [XmlIgnore]
        public static LinkedList<Player> UndoBuffer = new LinkedList<Player>();
        [XmlIgnore]
        public static LinkedList<Player> RedoBuffer = new LinkedList<Player>();
        [XmlIgnore]
        public static int UnsavedChanges = 0;
        [XmlIgnore]
        public static int MaxBuffer = 200;
        [XmlIgnore]
        public static Player current = new Player();
        [XmlIgnore]
        public Dictionary<Feature, int> ChoiceCounter;
        [XmlIgnore]
        public Dictionary<string, int> ChoiceTotal;
        [XmlIgnore]
        public static string lastid = "";
        public static void MakeHistory(string id = null)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                if (id==null) id = "";
                if (id == "" || id != lastid)
                {
                    Bitmap port = current.Portrait;
                    Bitmap fac = current.FactionImage;
                    current.Portrait = null;
                    current.FactionImage = null;
                    serializer.Serialize(mem, current);
                    mem.Seek(0, SeekOrigin.Begin);
                    UndoBuffer.AddLast((Player)serializer.Deserialize(mem));
                    current.Portrait = port;
                    current.FactionImage = fac;
                    UndoBuffer.Last.Value.Portrait = port;
                    UndoBuffer.Last.Value.FactionImage = fac;
                    foreach (Possession pos in UndoBuffer.Last.Value.Possessions) if (pos.Description != null) pos.Description = pos.Description.Replace("\n", Environment.NewLine);
                    for (int i = 0; i < UndoBuffer.Last.Value.Journal.Count; i++) UndoBuffer.Last.Value.Journal[i] = UndoBuffer.Last.Value.Journal[i].Replace("\n", Environment.NewLine);
                    for (int i = 0; i < UndoBuffer.Last.Value.ComplexJournal.Count; i++) if (UndoBuffer.Last.Value.ComplexJournal[i].Text != null) UndoBuffer.Last.Value.ComplexJournal[i].Text = UndoBuffer.Last.Value.ComplexJournal[i].Text.Replace("\n", Environment.NewLine);
                    UndoBuffer.Last.Value.Allies = UndoBuffer.Last.Value.Allies.Replace("\n", Environment.NewLine);
                    UndoBuffer.Last.Value.Backstory = UndoBuffer.Last.Value.Backstory.Replace("\n", Environment.NewLine);
                    Program.MainWindow.undoToolStripMenuItem.Enabled = true;
                    Program.MainWindow.redoToolStripMenuItem.Enabled = false;
                    RedoBuffer.Clear();
                    if (UndoBuffer.Count > MaxBuffer) UndoBuffer.RemoveFirst();
                    UnsavedChanges++;
                }
                lastid=id;
            }
        }
        public static bool Undo()
        {
            if (UndoBuffer.Count > 0)
            {
                lastid = "";
                RedoBuffer.AddLast(current);
                current = UndoBuffer.Last.Value;
                UndoBuffer.RemoveLast();
                if (UnsavedChanges > 0) UnsavedChanges--;
                return true;
            }
            return false;
        }
        public static bool Redo()
        {
            if (RedoBuffer.Count > 0)
            {
                lastid = "";
                UndoBuffer.AddLast(current);
                current = RedoBuffer.Last.Value;
                RedoBuffer.RemoveLast();
                UnsavedChanges++;
                return true;
            }
            return false;
        }
        public static bool CanUndo()
        {
            return UndoBuffer.Count > 0;
        }
        public static bool CanRedo()
        {
            return RedoBuffer.Count > 0;
        }


        public List<Spellcasting> Spellcasting = new List<Spellcasting>();
        public List<string> Boons = new List<string>();
        public List<AbilityFeatChoice> AbilityFeatChoices = new List<AbilityFeatChoice>();
        public List<UsedResource> UsedResources = new List<UsedResource>();
        public List<Possession> Possessions = new List<Possession>();
        public List<string> Conditions = new List<string>();
        public List<string> HiddenFeatures = new List<string>();
        public List<String> Items = new List<String>();
        private static XmlSerializer serializer = new XmlSerializer(typeof(Player));
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
        [XmlElement("Portrait")]
        public string PortraitLocation = null;
        [XmlIgnore]
        public Bitmap Portrait { get; set; }

        [XmlElement("PortraitData")]
        public byte[] PortraitSerialized
        {
            get
            { // serialize
                if (Portrait == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    Portrait.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    Portrait = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        Portrait = new Bitmap(ms);
                    }
                }
            }
        }
        public String Name { get; set; }
        public String FactionName { get; set;}
        [XmlElement("FactionImage")]
        public string FactionImageLocation = null;
        [XmlIgnore]
        public Bitmap FactionImage { get; set; }

        [XmlElement("FactionImageData")]
        public byte[] FactionImageSerialized
        {
            get
            { // serialize
                if (FactionImage == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    FactionImage.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    FactionImage = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        FactionImage = new Bitmap(ms);
                    }
                }
            }
        }
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
        public bool Inspiration { get; set; }
        public int CP { get; set; }
        public int SP { get; set; }
        public int EP { get; set; }
        public int GP { get; set; }
        public int PP { get; set; }
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

        public int getSpellSlotsMax(String SpellcastingID)
        {
            List<int> slots = getSpellSlots(SpellcastingID);
            for (int level = slots.Count - 1; level >= 0; level--)
            {
                if (slots[level] > 0) return level + 1;
            }
            return 0;
        }

        public List<Possession> getItemsAndPossessions()
        {
            Dictionary<string, int> items = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (Item i in getFreeItems())
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
                if (p.BaseItem != null && p.BaseItem != "")
                {
                    int stacksize = Item.Get(p.BaseItem, null).StackSize;
                    if (stacksize < 0) stacksize = 1;
                    int stack = (int)Math.Ceiling((double)p.Count / (double)stacksize);
                    if (items.ContainsKey(p.BaseItem) && items[p.BaseItem]>=stack)
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
                }
                else result.Add(p);
            }
            foreach (string i in items.Keys) if (items[i] > 0) result.Add(new Possession(i,items[i]));
            result.Sort();
            return result;
        }
        public void changePossessionAmountAndAddRemoveItemsAccordingly(Possession p, int count)
        {
            if (Possessions.Contains(p))
            {
                if (p.BaseItem != null && p.BaseItem != "")
                {
                    int stacksize = Item.Get(p.BaseItem, null).StackSize;
                    if (stacksize < 0) stacksize = 1;
                    int pcount = (int)Math.Ceiling((double)p.Count / (double)stacksize);
                    p.Count = count;
                    count = (int)Math.Ceiling((double)count / (double)stacksize);
                    while (count != pcount) if (count < pcount)
                        {
                            Items.Remove(p.BaseItem);
                            pcount--;
                        }
                        else
                        {
                            Items.Add(p.BaseItem);
                            pcount++;
                        }
                }
                else p.Count = count; 
            }
            else
            {
                if (p.BaseItem != null && p.BaseItem != "")
                {
                    int stacksize = Item.Get(p.BaseItem, null).StackSize;
                    if (stacksize < 0) stacksize = 1;
                    int pcount = (int)Math.Ceiling((double)p.Count / (double)stacksize);
                    p.Count = count;
                    count = (int)Math.Ceiling((double)count / (double)stacksize);
                    while (count != pcount) if (count < pcount)
                        {
                            Items.Remove(p.BaseItem);
                            pcount--;
                        }
                        else
                        {
                            Items.Add(p.BaseItem);
                            pcount++;
                        }
                }
                else p.Count = count;
                Possessions.Add(p);
            }
        }
        public void removePossessionAndItems(Possession p)
        {
            bool worked = true;
            if (p.BaseItem != null && p.BaseItem != "")
            {
                int stacksize = Item.Get(p.BaseItem, null).StackSize;
                if (stacksize < 0) stacksize = 1;
                int count = (int)Math.Ceiling((double)p.Count / (double)stacksize);
                for (int i = 0; i < count; i++)
                {
                    worked = false;
                    if (!Items.Remove(p.BaseItem)) break;
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
                AddPossession(p);
            }
        }
        public void AddPossession(Possession p)
        {
            if (!Possessions.Contains(p)) Possessions.Add(p);
        }
        public double getWeight()
        {
            List<Possession> items = getItemsAndPossessions();
            double total=0.0;
            foreach (Possession o in items) total += o.getWeight();
            total += getMoney().Weight();
            return total;
        }
        public void setAbilityFeatChoices(AbilityScoreFeatFeature f,Ability ab1, Ability ab2,String feat)
        {
            int level=getLevel();
            AbilityFeatChoice afc = AbilityFeatChoices.Find(t => t.UniqueID == f.UniqueID && t.Level == f.Level);
            if (afc == null)
            {
                afc = new AbilityFeatChoice();
                afc.Level = f.Level;
                afc.UniqueID = f.UniqueID;
                AbilityFeatChoices.Add(afc);
                foreach (PlayerClass p in Classes)
                {
                    if (p.getFeatures(level, this).Contains(f)) afc.Class = p.ClassName;
                }
            }
            afc.Ability1=ab1;
            afc.Ability2=ab2;
            afc.Feat=feat;
        }
        public List<Feature> getPossessionFeatures(int level=0)
        {
            if (level == 0) level = getLevel();
            List<Feature> result=new List<Feature>();
            foreach (Possession p in Possessions)
            {
                result.AddRange(p.Collect(level, this));
            }
            return result;
        }
        public AbilityFeatChoice getAbilityFeatChoice(AbilityScoreFeatFeature f)
        {
            int level = getLevel();
            AbilityFeatChoice afc = AbilityFeatChoices.Find(t => t.UniqueID == f.UniqueID && t.Level == f.Level);
            if (afc == null)
            {
                afc = new AbilityFeatChoice();
                afc.Level = f.Level;
                afc.UniqueID = f.UniqueID;
                AbilityFeatChoices.Add(afc);
                foreach (PlayerClass p in Classes)
                {
                    if (p.getFeatures(level, this).Contains(f)) afc.Class = p.ClassName;
                }
            }
            return afc;
        }
        public IEnumerable<AbilityFeatChoice> getAbilityFeatChoices(int level=0)
        {
            if (level==0) level = getLevel();
            Dictionary<String, int> cl = getClassLevelStrings(level);
            return from AbilityFeatChoice afc in AbilityFeatChoices where (afc.UniqueID != null && afc.UniqueID != "" && afc.Level <= level && (afc.Class == null || afc.Class == "")) || (cl.ContainsKey(afc.Class) && afc.Level <= cl[afc.Class]) select afc;
        }
        public Dictionary<ClassDefinition, int> getClassLevels(int level=0)
        {
            if (level == 0) level = getLevel();
            Dictionary<ClassDefinition, int> classlevels = new Dictionary<ClassDefinition, int>();
            foreach (PlayerClass p in Classes)
            {
                classlevels.Add(p.Class, p.getClassLevelUpToLevel(level));
            }
            return classlevels;
        }
        public Spellcasting getSpellcasting(string spellcastingID)
        {
            foreach (Spellcasting sc in Spellcasting) if (sc.SpellcastingID == spellcastingID) return sc;
            Spellcasting s = new Spellcasting();
            s.SpellcastingID = spellcastingID;
            Spellcasting.Add(s);
            return s;
        }
        public int getClassLevel(string spellcastingID, int level = 0)
        {
            if (level == 0) level = getLevel();
            foreach (PlayerClass pc in Classes)
            {
                foreach (Feature f in pc.getFeatures(level, this)) if (f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID) return pc.getClassLevelUpToLevel(level);
            }
            return 0;
        }
        public SpellChoice getSpellChoice(string spellcastingID, string uniqueID)
        {
            Spellcasting sc = getSpellcasting(spellcastingID);
            int level = getLevel();
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

        

        public List<int> getSpellSlots(string spellcastingID, int level = 0)
        {
            if (level == 0) level = getLevel();
            int classlevel = 0;
            Spellcasting sc = getSpellcasting(spellcastingID);
            int multiclassinglevel = 0;
            bool overwrittenbymulticlassing = false;
            foreach (PlayerClass pc in Classes)
            {
                foreach (Feature f in pc.getFeatures(level, this))
                {
                    if (f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID)
                    {
                        overwrittenbymulticlassing = ((SpellcastingFeature)f).OverwrittenByMulticlassing;
                        classlevel = pc.getClassLevelUpToLevel(level);
                    }
                }
                multiclassinglevel += pc.getMulticlassingLevel(level);
            }
            int curlevel = 0;
            List<int> slots = null;
            List<Feature> features = getFeatures();
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
        public List<int> getUsedSpellSlots(string spellcastingID)
        {
            List<Feature> features = getFeatures();
            if (features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID && ((SpellcastingFeature)f).OverwrittenByMulticlassing) && features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == "MULTICLASS")) spellcastingID = "MULTICLASS";
            Spellcasting sc = getSpellcasting(spellcastingID);
            return sc.UsedSlots;
        }
        public void setSpellSlot(string spellcastingID, int spelllevel, int value)
        {
            List<Feature> features = getFeatures();
            if (features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID && ((SpellcastingFeature)f).OverwrittenByMulticlassing) && features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == "MULTICLASS")) spellcastingID = "MULTICLASS";
            Spellcasting sc = getSpellcasting(spellcastingID);
            while (sc.UsedSlots.Count < spelllevel) sc.UsedSlots.Add(0);
            sc.UsedSlots[spelllevel - 1] = value;
        }
        public void resetSpellSlots(string spellcastingID)
        {
            List<Feature> features = getFeatures();
            if (features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcastingID && ((SpellcastingFeature)f).OverwrittenByMulticlassing) && features.Exists(f => f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == "MULTICLASS")) spellcastingID = "MULTICLASS";
            Spellcasting sc = getSpellcasting(spellcastingID);
            sc.UsedSlots = new List<int>();
        }
        public List<SpellSlotInfo> getSpellSlotInfo(string SpellcastingID, int level = 0)
        {
            List<int> slots = getSpellSlots(SpellcastingID, level);
            List<int> usedSlots = getUsedSpellSlots(SpellcastingID);
            List<SpellSlotInfo> res = new List<SpellSlotInfo>();
            for (int spelllevel = 0; spelllevel < slots.Count; spelllevel++)
            {
                if (slots[spelllevel] > 0)
                    if (spelllevel < usedSlots.Count) res.Add(new SpellSlotInfo(SpellcastingID, spelllevel + 1, slots[spelllevel], usedSlots[spelllevel]));
                    else res.Add(new SpellSlotInfo(SpellcastingID, spelllevel + 1, slots[spelllevel], 0));
            }
            return res;
        }
        public Dictionary<String, int> getClassLevelStrings(int level = 0)
        {
            if (level == 0) level = getLevel();
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
                if (p.Class == classdefinition) found = p;
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
                    return p.Class;
                }
            }
            return null;
        }
        public bool DeleteClass(int atLevel)
        {
            PlayerClass todelete=null;
            foreach (PlayerClass p in Classes)
            {
                if (p.getClassLevelAtLevel(atLevel) > 0)
                {
                    todelete = p;
                    if (p.deleteLevel(atLevel)) return true;
                }
            }
            if (todelete != null) Classes.Remove(todelete);
            return false;
        }
        public void setHPRoll(int atLevel, int hproll)
        {
            foreach (PlayerClass p in Classes)
            {
                if (p.getClassLevelAtLevel(atLevel) > 0)
                {
                    p.setHPRollAtLevel(atLevel, hproll);
                }
            }
        }
        public List<ClassDefinition> getClassesByLevel()
        {
            int level=getLevel();
            List<ClassDefinition> cd=new List<ClassDefinition>();
            if (Classes == null) Classes = new List<PlayerClass>();
            for (int c = 1; c <= level; c++) {
                PlayerClass pc = Classes.Find(p => p.getClassLevelAtLevel(c) > 0);
                if (pc == null) cd.Add(null);
                else cd.Add(pc.Class);
            }
            return cd;
        }
        public List<int> getHProllsByLevel()
        {
            int level = getLevel();
            List<int> chp = new List<int>();
            for (int c = 1; c <= level; c++)
            {
                PlayerClass pc = Classes.Find(p => p.getClassLevelAtLevel(c) > 0);
                if (pc == null) chp.Add(0);
                else chp.Add(pc.HPRollAtLevel(c));
            }
            return chp;
        }
        public List<ClassInfo> getClassInfos(int level=0)
        {
            if (level == 0) level = getLevel();
            List<ClassInfo> ci = new List<ClassInfo>();
            for (int c = 1; c <= level; c++)
            {
                PlayerClass pc = Classes.Find(p => p.getClassLevelAtLevel(c) > 0);
                if (pc != null) ci.Add(new ClassInfo(pc.Class, c, pc.HPRollAtLevel(c), pc.getClassLevelUpToLevel(c)));
                else ci.Add(new ClassInfo(null,c,0,0));
            }
            return ci;
        }
        public int getChoiceOffset(Feature f,string uniqueID, int amount) {
            if (!ChoiceCounter.ContainsKey(f))
            {
                ChoiceCounter.Add(f, getChoiceTotal(uniqueID));
                ChoiceTotal[uniqueID] += amount;
            }
            return ChoiceCounter[f];
        }
        public int getChoiceTotal(string uniqueID) {
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
                return Background.Get(BackgroundName, null);
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
                return Race.Get(RaceName, null);
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
                return SubRace.Get(SubRaceName, null);
            }
            set
            {
                if (value == null) SubRaceName = "";
                else SubRaceName = value.Name + " " + ConfigManager.SourceSeperator + " " + value.Source;
            }
        }
        public List<Feature> getBackgroundFeatures(int level=0, bool reset=true)
        {
            if (level == 0) level = getLevel();
            List<Feature> fl = new List<Feature>();
            if (Background != null) fl.AddRange(Background.CollectFeatures(level, this));
            fl.AddRange(getBoons(level));
            fl.AddRange(getPossessionFeatures(level));
            return fl;
        }
        public bool canMulticlass(ClassDefinition c, int level)
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, f => false, level);
            if (Classes.Count > 0 && Classes[0].Class != null)
            {
                if (Classes[0].Class == c) return true;
                if (!Utils.canMulticlass(Classes[0].Class, asa)) return false;
            }
            else return false; //Can not multiclass without first class.
            foreach (var d in Classes) if (d.Class != c && d.Class.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase)) return false; // Can not multiclass into the same class from a different sourcebook
            return Utils.canMulticlass(c, asa);
        }
        public List<Feature> getCommonFeaturesAndFeats(int level = 0, bool reset = true)
        {
            if (level == 0) level = getLevel();
            List<Feature> fl = new List<Feature>();
            foreach (Feature f in ConfigManager.CommonFeatures)
                fl.AddRange(f.Collect(level, this));
            fl.AddRange(getFeats(level));
            return fl;
        }
        public List<Feature> getClassFeatures(int level=0, bool reset = true)
        {
            if (level == 0) level = getLevel();
            List<Feature> fl = new List<Feature>();
            if (Classes!=null && Classes.Count > 0)
            {
                int multiclassing = 0;
                int multiclassinglevel = 0;
                foreach (PlayerClass p in Classes)
                {
                    if (p.getClassLevelUpToLevel(level) > 0)
                    {
                        fl.AddRange(p.getFeatures(level, this));
                        multiclassing++;
                        multiclassinglevel += p.getMulticlassingLevel();
                    }
                }
                if (multiclassinglevel < 1) multiclassinglevel = 1;
                if (multiclassing > 1)
                {
                    foreach (Feature f in ConfigManager.MultiClassFeatures)
                        fl.AddRange(f.Collect(multiclassinglevel, this));
                }
            }
            return fl;
        }
        public List<Feature> getRaceFeatures(int level = 0, bool reset = true)
        {
            if (level == 0) level = getLevel();
            List<Feature> fl = new List<Feature>();
            if (Race == null) return fl;
            fl.AddRange(getSubRaceFeatures(level, reset));
            fl.AddRange(Race.CollectFeatures(level, this));
            return fl;
        }
        public List<Feature> getSubRaceFeatures(int level = 0, bool reset = true)
        {
            if (level == 0) level = getLevel();
            if (SubRace == null) return new List<Feature>();
            return SubRace.CollectFeatures(level, this); 
        }
        public void AddSubclass(string cd, string subclass)
        {
            foreach (PlayerClass p in Classes) if (ConfigManager.SourceInvariantComparer.Equals(p.ClassName, cd)) p.SubClassName = subclass;
        }
        public SubClass getSubclass(string cd)
        {
            foreach (PlayerClass p in Classes) if (ConfigManager.SourceInvariantComparer.Equals(p.ClassName, cd)) return p.SubClass;
            return null;
        }
        public void RemoveSubclass(string cd)
        {
            foreach (PlayerClass p in Classes) if (ConfigManager.SourceInvariantComparer.Equals(p.ClassName, cd)) p.SubClass = null;
        }
        public List<Feature> getBoons(int level=0, bool reset = true)
        {
            if (Boons == null) return new List<Feature>();
            List<Feature> res = new List<Feature>();
            if (level == 0) level = getLevel();
            foreach (string s in Boons) res.AddRange(FeatureCollection.getBoon(s, null).Collect(level, this));
            return res;
        }
        public List<Feature> getFeats(int level = 0, bool reset = true)
        {
            if (AbilityFeatChoices == null) return new List<Feature>();
            List<Feature> res = new List<Feature>();
            if (level == 0) level = getLevel();
            List<Feature> feats = FeatureCollection.Get("");
            foreach (AbilityFeatChoice s in getAbilityFeatChoices(level)) 
            {
                if (s.Ability1 == Ability.None && s.Ability2 == Ability.None && s.Feat != null && s.Feat != "") res.AddRange(feats.Find(f => f.Name == s.Feat).Collect(level, this));
            }
            return res;
        }
        public List<string> getFeatNames(int level = 0)
        {
            if (AbilityFeatChoices == null) return new List<string>();
            List<string> res = new List<string>();
            if (level==0) level = getLevel();
            List<Feature> feats = FeatureCollection.Get("");
            foreach (AbilityFeatChoice s in getAbilityFeatChoices(level)) if (s.Ability1 == Ability.None && s.Ability2 == Ability.None && s.Feat != null && s.Feat != "") res.Add(s.Feat);
            return res;
        }
        public IEnumerable<AbilityScoreFeatFeature> getAbilityIncreases(int level = 0)
        {
            return from Feature f in getFeatures(level) where f is AbilityScoreFeatFeature select (AbilityScoreFeatFeature)f;
        }
        public List<Feature> getFeatures(int level=0, bool reset = true)
        {
            List<Feature> res = new List<Feature>();
            res.AddRange(getBackgroundFeatures(level, false));
            res.AddRange(getRaceFeatures(level, false));
            res.AddRange(getClassFeatures(level, false));
            res.AddRange(getCommonFeaturesAndFeats(level, false));
            return res;
        }
        public Choice getChoice(String ID)
        {
            return (from c in Choices where c.UniqueID == ID select c).FirstOrDefault<Choice>();
        }

        public void setChoice(String ID, String Value)
        {
            Choice c = getChoice(ID);
            if (c != null) c.Value = Value;
            else Choices.Add(new Choice(ID, Value));
        }
        public Price getMoney()
        {
            int cp = CP;
            int sp = SP;
            int ep = EP;
            int gp = GP;
            int pp = PP;
            foreach (Feature f in getFeatures()) if (f is FreeItemAndGoldFeature)
                {
                    cp += ((FreeItemAndGoldFeature)f).CP;
                    sp += ((FreeItemAndGoldFeature)f).SP;
                    gp += ((FreeItemAndGoldFeature)f).GP;
                }
            Price p = new Price(cp,sp,gp);
            p.ep=ep;
            p.pp=pp;
            foreach (JournalEntry e in ComplexJournal)
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

        public void setMoney(int cp, int sp, int ep, int gp, int pp)
        {
            foreach (Feature f in getFeatures()) if (f is FreeItemAndGoldFeature)
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

        public Dictionary<string,int> getResources(int level = 0)
        {
            if (level == 0) level = getLevel();
            AbilityScoreArray asa;
            AbilityScoreArray max;
            Dictionary<string, string> exclusion = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, int> res = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (FeatureClass fc in getFeatureAndAbility(out asa, out max, t => t is ResourceFeature))
            {
                Feature f = fc.feature;
                if (f is ResourceFeature)
                {
                    ResourceFeature rf = ((ResourceFeature)f);
                    if (!res.ContainsKey(rf.ResourceID))
                    {
                        int v = Utils.evaluate(rf, asa, null, fc.classlevel, level);
                        if (rf.ValueBonus != Ability.None) v = Math.Max(1, v);
                        res.Add(rf.ResourceID, v);
                        if (rf.ExclusionID != null && rf.ExclusionID != "" && !exclusion.ContainsKey(rf.ResourceID)) exclusion.Add(rf.ResourceID, rf.ExclusionID);
                    }
                    else
                    {
                        res[rf.ResourceID] += Utils.evaluate(rf, asa, null, fc.classlevel, level);
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
                    for (int i = 0; i < ids.Count; i++) res.Remove(ids[i]);
                }
            }
            return res;
        }
        public Dictionary<string, RechargeModifier> getResourceRecharge()
        {
            Dictionary<string, RechargeModifier> res = new Dictionary<string, RechargeModifier>(StringComparer.OrdinalIgnoreCase);
            foreach (Feature f in getFeatures())
            {
                if (f is ResourceFeature)
                {
                    ResourceFeature rf = ((ResourceFeature)f);
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
        public Dictionary<string, ResourceInfo> getResourceInfo(bool displayUsed, int level = 0)
        {
            if (level == 0) level = getLevel();
            AbilityScoreArray asa;
            AbilityScoreArray max;
            Dictionary<string, string> exclusion = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, ResourceInfo> res = new Dictionary<string, ResourceInfo>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, int> used = new Dictionary<string, int>();
            foreach (UsedResource ur in UsedResources) used[ur.ResourceID] = ur.Used;
            foreach (FeatureClass fc in getFeatureAndAbility(out asa, out max, t => t is ResourceFeature))
            {
                Feature f = fc.feature;
                if (f is ResourceFeature)
                {
                    ResourceFeature rf = ((ResourceFeature)f);
                    if (!res.ContainsKey(rf.ResourceID))
                    {
                        int v = Utils.evaluate(rf, asa, null, fc.classlevel, level);
                        res.Add(rf.ResourceID, new ResourceInfo(rf.ResourceID, rf.Name, getUsedResources(rf.ResourceID), v, rf.Recharge, displayUsed));
                        if (rf.ExclusionID != null && rf.ExclusionID != "" && !exclusion.ContainsKey(rf.ResourceID)) exclusion.Add(rf.ResourceID, rf.ExclusionID);
                    }
                    else
                    {
                        ResourceInfo r = res[rf.ResourceID];
                        if (r.Recharge < rf.Recharge) r.Recharge = rf.Recharge; //Bigger is better (more often)
                        r.Max += Utils.evaluate(rf, asa, null, fc.classlevel, level);
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
                    for (int i = 0; i < ids.Count; i++) res.Remove(ids[i]);
                }
            }
            return res;
        }
        public FeatureContainer getResourceFeatures(String resourceID) //.toHTML for description
        {
            return new FeatureContainer(from f in getFeatures() where f is ResourceFeature && ((ResourceFeature)f).ResourceID == resourceID orderby f.Level select f);
        }
        public int getUsedResources(String resourceID)
        {
            if (UsedResources.Count == 0) return 0;
            List<UsedResource> ur=new List<UsedResource>(from f in UsedResources where f.ResourceID == resourceID select f);
            if (ur.Count > 0) return ur[0].Used;
            return 0;
        }
        public void setUsedResources(String resourceID, int value)
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
        public void Save(FileStream fs)
        {
            serializer.Serialize(fs, this);
        }
        public void Save(TextWriter fs)
        {
            serializer.Serialize(fs, this);
        }
        public static Player Load(FileStream fs)
        {
            try
            {
                Player p = (Player)serializer.Deserialize(fs);

                p.Allies = p.Allies.Replace("\n", Environment.NewLine);
                p.Backstory = p.Backstory.Replace("\n", Environment.NewLine);
                foreach (Possession pos in p.Possessions) if (pos.Description != null) pos.Description = pos.Description.Replace("\n", Environment.NewLine);
                for (int i = 0; i < p.Journal.Count; i++) p.Journal[i] = p.Journal[i].Replace("\n", Environment.NewLine);
                for (int i = 0; i < p.ComplexJournal.Count; i++) if (p.ComplexJournal[i].Text != null) p.ComplexJournal[i].Text = p.ComplexJournal[i].Text.Replace("\n", Environment.NewLine);
                if (p.Portrait == null && p.PortraitLocation != null && File.Exists(p.PortraitLocation)) p.Portrait = new Bitmap(p.PortraitLocation);
                if (p.FactionImage == null && p.FactionImageLocation != null && File.Exists(p.FactionImageLocation)) p.FactionImage = new Bitmap(p.FactionImageLocation);
                p.PortraitLocation = null;
                p.FactionImageLocation = null;
                foreach (Spellcasting sc in p.Spellcasting) {
                    sc.postLoad(p.getLevel());
                }
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<Skill> getSkillProficiencies()
        {
            List<Skill> skills=new List<Skill>();
            foreach (Feature f in getFeatures())
            {
                if (f is SkillProficiencyFeature && ((SkillProficiencyFeature)f).ProficiencyMultiplier > 0.999) foreach (String s in ((SkillProficiencyFeature)f).Skills) skills.Add(Skill.Get(s, f.Source));
                else if (f is SkillProficiencyChoiceFeature && ((SkillProficiencyChoiceFeature)f).ProficiencyMultiplier > 0.999) skills.AddRange(((SkillProficiencyChoiceFeature)f).getSkills(this));
            }
            return skills.Distinct<Skill>();
        }
        public Ability getSaveProficiencies()
        {
            Ability saves = Ability.None;
            foreach (Feature f in getFeatures()) if (f is SaveProficiencyFeature) saves |= ((SaveProficiencyFeature)f).Ability;
            return saves;
        }
        public IEnumerable<Language> getLanguages()
        {
            List<Language> langs = new List<Language>();
            foreach (Feature f in getFeatures())
            {
                if (f is LanguageProficiencyFeature) foreach (String s in ((LanguageProficiencyFeature)f).Languages) langs.Add(Language.Get(s, f.Source));
                else if (f is LanguageChoiceFeature) langs.AddRange(((LanguageChoiceFeature)f).getLanguages(this));
            }
            langs.Sort();
            return langs.Distinct<Language>();
        }
        public IEnumerable<Tool> getToolProficiencies()
        {
            List<Tool> tools = new List<Tool>();
            List<ToolKWProficiencyFeature> kwpf = new List<ToolKWProficiencyFeature>();
            foreach (Feature f in getFeatures())
            {
                if (f is ToolProficiencyFeature) foreach (String s in ((ToolProficiencyFeature)f).Tools) tools.Add(Item.Get(s, f.Source).asTool());
                else if (f is ToolProficiencyChoiceConditionFeature) tools.AddRange(((ToolProficiencyChoiceConditionFeature)f).getTools(this));
                if (f is ToolKWProficiencyFeature) kwpf.Add(((ToolKWProficiencyFeature)f));
            }
            tools.Sort();
            tools.RemoveAll(t=>kwpf.Find(p=>Utils.matches(p, t, 0))!=null);
            return tools.Distinct<Tool>();
        }
        public IEnumerable<ModifiedSpell> getBonusSpells(bool filterAtWill=false)
        {
            List<ModifiedSpell> spells = new List<ModifiedSpell>();
            foreach (Feature f in getFeatures())
            {
                if (f is BonusSpellFeature)
                {
                    BonusSpellFeature bsf=(BonusSpellFeature)f;
                    Spell s = Spell.Get(bsf.Spell, bsf.Source);
                    if (!filterAtWill || (bsf.SpellCastModifier < RechargeModifier.AtWill && bsf.SpellCastModifier != RechargeModifier.Unmodified) || (bsf.SpellCastModifier < RechargeModifier.AtWill && s.Level > 0))
                        spells.Add(new ModifiedSpell(Spell.Get(bsf.Spell, bsf.Source), bsf.KeywordsToAdd, bsf.SpellCastingAbility, bsf.SpellCastModifier));
                }
                else if (f is BonusSpellKeywordChoiceFeature) if (!filterAtWill || ((BonusSpellKeywordChoiceFeature)f).SpellCastModifier < RechargeModifier.AtWill) spells.AddRange((Utils.getSpells((BonusSpellKeywordChoiceFeature)f)).Where(s=>s.Level > 0 || !filterAtWill));
            }
            spells.Sort();
            return spells.Distinct<ModifiedSpell>();
        }
        public IEnumerable<string> getToolKWProficiencies()
        {
            List<string> tools = new List<string>();
            foreach (Feature f in getFeatures())
            {
                if (f is ToolKWProficiencyFeature) tools.Add(((ToolKWProficiencyFeature)f).Description);
            }
            return tools;
        }
        public IEnumerable<string> getOtherProficiencies()
        {
            return from f in getFeatures() where f is OtherProficiencyFeature orderby f.Text select f.Text;
        }
        public List<Item> getFreeItems()
        {
            List<Item> items = new List<Item>();
            foreach (Feature f in getFeatures())
            {
                if (f is ItemChoiceConditionFeature) items.AddRange(((ItemChoiceConditionFeature)f).getItems(this));
                else if (f is ItemChoiceFeature) items.AddRange(((ItemChoiceFeature)f).getItems(this));
                else if (f is FreeItemAndGoldFeature) foreach (String s in ((FreeItemAndGoldFeature)f).Items) items.Add(Item.Get(s, f.Source));
            }
            items.Sort();
            return items;
        }
        public int getExtraAttacks()
        {
            int extraattacks = 0;
            foreach (Feature f in getFeatures()) if (f is ExtraAttackFeature) extraattacks=Math.Max(extraattacks,((ExtraAttackFeature)f).ExtraAttacks);
            return extraattacks;
        }
        public string getRaceSubName()
        {
            if (SubRaceName != null && SubRaceName != "") return SourceInvariantComparer.NoSource(SubRaceName);
            if (RaceName != null && RaceName != "") return SourceInvariantComparer.NoSource(RaceName);
            return "";
        }
        public int getSpeed()
        {
            int extraspeed=0;
            int basespeed = 0;
            int basespeedIgnoreArmor = -10;
            AbilityScoreArray asa;
            AbilityScoreArray max;
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item mainHand = getMainHand();

            List<string> additionalKW = new List<string>();
            if (armor == null)
            {
                additionalKW.Add("unarmored");
                armor = new Item();
                armor.Name = "No Armor";
            }
            if (offHand == null && !(mainHand is Weapon && mainHand.Keywords.Exists(t => t.Name == "two-handed"))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            List<ACFeature> ways = new List<ACFeature>();
            foreach (FeatureClass fc in getFeatureAndAbility(out asa, out max, t => t is SpeedFeature))
            {
                Feature f = fc.feature;
                if (((SpeedFeature)f).Condition != null && ((SpeedFeature)f).Condition.Length > 0)
                {
                    if (!Utils.matches(armor, ((SpeedFeature)f).Condition, fc.classlevel, additionalKW, asa)) continue;
                }
                extraspeed += Utils.evaluate(((SpeedFeature)f).ExtraSpeed, asa, additionalKW, fc.classlevel, 0, armor);
                if (basespeed < ((SpeedFeature)f).BaseSpeed) basespeed = ((SpeedFeature)f).BaseSpeed;
                if (((SpeedFeature)f).IgnoreArmor && basespeedIgnoreArmor < ((SpeedFeature)f).BaseSpeed) basespeedIgnoreArmor = ((SpeedFeature)f).BaseSpeed;
            }
            int ArmorPenalty = 0;
            if (armor != null && armor is Armor && ((Armor)armor).StrengthRequired > asa.Strength) ArmorPenalty = 10;
            return Math.Max(basespeed + extraspeed - ArmorPenalty, basespeedIgnoreArmor + extraspeed);
        }
        public int getVisionRange()
        {
            int range = 0;
            foreach (Feature f in getFeatures()) if (f is VisionFeature) range += ((VisionFeature)f).Range;
            return range;
        }
        public struct FeatureClass
        {
            public Feature feature;
            public int classlevel;
        }
        private List<FeatureClass> getFeatureAndAbility(out AbilityScoreArray asa, out AbilityScoreArray max, Predicate<Feature> match, int level=0, IEnumerable<Feature> additional=null)
        {
            if (level == 0) level = getLevel();
            asa = new AbilityScoreArray(BaseStrength, BaseDexterity, BaseConstitution, BaseIntelligence, BaseWisdom, BaseCharisma);
            max = new AbilityScoreArray(AbilityScores.Max, AbilityScores.Max, AbilityScores.Max, AbilityScores.Max, AbilityScores.Max, AbilityScores.Max);
            AbilityScoreArray asaset = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            AbilityScoreArray maxset = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            AbilityScoreArray bonus = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            AbilityScoreArray bonusmax = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            List<FeatureClass> found = new List<FeatureClass>();
            foreach (AbilityFeatChoice afc in getAbilityFeatChoices(level)) {
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
                        foreach (Feature f in p.getFeatures(level, this))
                        {
                            if (f is AbilityScoreFeature)
                            {
                                AbilityScoreFeature asf = ((AbilityScoreFeature)f);
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
                                        else {
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
                                        else {
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
                        multiclassinglevel += p.getMulticlassingLevel();
                    }
                }
                if (multiclassinglevel < 1) multiclassinglevel = 1;
                
            }

            List<Feature> feats = new List<Feature>();
            feats.AddRange(getBackgroundFeatures(level, false));
            feats.AddRange(getRaceFeatures(level, false));
            //feats.AddRange(getClassFeatures(level, false));
            feats.AddRange(getCommonFeaturesAndFeats(level, false));
            if (multiclassing > 1)
            {
                foreach (Feature f in ConfigManager.MultiClassFeatures)
                    feats.AddRange(f.Collect(multiclassinglevel, this));
            }
            if (additional!=null) feats.AddRange(additional);
            foreach (Feature f in feats)
            {
                if (f is AbilityScoreFeature)
                {
                    AbilityScoreFeature asf = ((AbilityScoreFeature)f);
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
        public int getStrength()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.Strength;
        }
        public int getDexterity()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.Dexterity;
        }
        public int getConstitution()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.Constitution;
        }
        public int getIntelligence()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.Intelligence;
        }
        public int getWisdom()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.Wisdom;
        }
        public int getCharisma()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.Charisma;
        }
        public int getStrengthMod()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.strmod;
        }
        public int getDexterityMod()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.dexmod;
        }

        public int getConstitutionMod()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.conmod;
        }
        public int getIntelligenceMod()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.intmod;
        }
        public int getWisdomMod()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.wismod;
        }
        public int getCharismaMod()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa.chamod;
        }
        public int getHitpointMax()
        {
            int hp = 0;
            int level=getLevel();
            int hpperlevel=getConstitutionMod();
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is HitPointsFeature);
            foreach (PlayerClass p in Classes)
            {
                hp += p.getHP(p.getClassLevelUpToLevel(level));
            }
            foreach (FeatureClass fc in fa)
            {
                hp += Utils.evaluate(fc.feature as HitPointsFeature, asa, null, fc.classlevel, level);
            }
            return hp + hpperlevel * level + BonusMaxHP;
        }
        public List<HitDie> getHitDie()
        {
            List<int> hd = new List<int>();
            List<HitDie> hitdie = new List<HitDie>();
            int level=getLevel();
            foreach (PlayerClass p in Classes)
            {
                while (hd.Count <= p.Class.HitDie) hd.Add(0);
                hd[p.Class.HitDie] += p.getClassLevelUpToLevel(level);
            }
            for (int h = 0; h < hd.Count; h++) if (hd[h] > 0) hitdie.Add(new HitDie(h, hd[h], (UsedHitDice.Count > h?UsedHitDice[h]:0)));
            return hitdie;
        }

        public AbilityScoreArray getFinalAbilityScores()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa;
        }
        public AbilityScoreArray getFinalAbilityScores(out AbilityScoreArray max)
        {
            AbilityScoreArray asa;
            getFeatureAndAbility(out asa, out max, t => false);
            return asa;
        }
        public int getSave(Ability ab)
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List < FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is SaveProficiencyFeature);
            Ability saves = Ability.None;
            foreach (FeatureClass f in fa) saves |= ((SaveProficiencyFeature)f.feature).Ability;
            int value = asa.Apply(ab);
            if ((ab & saves) != Ability.None) value += getProficiency();
            return value;
        }
        public List<SkillInfo> getSkills()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is SkillProficiencyChoiceFeature || t is SkillProficiencyFeature || t is BonusFeature);
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item weapon = getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            Dictionary<Skill, double> ProfModifier = new Dictionary<Skill, double>();
            Dictionary<Skill, int> res = new Dictionary<Skill, int>();
            foreach (Skill s in Skill.skills.Values) res.Add(s, 0);
            int generalBonus=0;
            double generalModifier = 0;
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is SkillProficiencyChoiceFeature)
                {
                    foreach (Skill s in ((SkillProficiencyChoiceFeature)f).getSkills(this))
                    {
                        double mod = ((SkillProficiencyChoiceFeature)f).ProficiencyMultiplier;
                        if (!ProfModifier.ContainsKey(s)) ProfModifier.Add(s, mod);
                        else if (ProfModifier[s] < mod) ProfModifier[s] = mod;
                    }
                }
                else if (f is SkillProficiencyFeature)
                {
                    double mod = ((SkillProficiencyFeature)f).ProficiencyMultiplier;
                    if (((SkillProficiencyFeature)f).Skills.Count == 0) if (generalModifier < mod) generalModifier = mod;
                    foreach (String sst in ((SkillProficiencyFeature)f).Skills)
                    {
                        Skill s = Skill.Get(sst, f.Source);
                        if (!ProfModifier.ContainsKey(s)) ProfModifier.Add(s, mod);
                        else if (ProfModifier[s] < mod) ProfModifier[s] = mod;
                    }
                }
                else if (f is BonusFeature && !((BonusFeature)f).SkillPassive && ((BonusFeature)f).SkillBonus != null && ((BonusFeature)f).SkillBonus.Trim() != "" && ((BonusFeature)f).SkillBonus.Trim() != "0" && Utils.matches((BonusFeature)f, armor, fc.classlevel, additionalKW, asa, true))
                { 
                    if (((BonusFeature)f).Skills.Count == 0) generalBonus+= Utils.evaluate(((BonusFeature)f).SkillBonus, asa, additionalKW, fc.classlevel, 0);
                    foreach (string s in ((BonusFeature)f).Skills) res[Skill.Get(s, f.Source)]+= Utils.evaluate(((BonusFeature)f).SkillBonus, asa, additionalKW, fc.classlevel, 0);
                }
            }
            int prof = getProficiency();
            List<SkillInfo> result = new List<SkillInfo>();
            foreach (Skill s in Skill.skills.Values)
            {
                double multiplier=generalModifier;
                if (ProfModifier.ContainsKey(s)) multiplier=ProfModifier[s];
                res[s]+=asa.ApplyMod(s.Base) + (int)Math.Floor(prof * multiplier);
                result.Add(new SkillInfo(s, res[s], s.Base));
            }
            result.Sort();
            return result;
        }
        public int getSkill(Skill s, Ability ability=Ability.None)
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is SkillProficiencyChoiceFeature || t is SkillProficiencyFeature || t is BonusFeature);
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item weapon = getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            double modifier = 0;
            int bonus = 0;
            if (ability == Ability.None) ability = s.Base;
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is SkillProficiencyChoiceFeature)
                {
                    if (((SkillProficiencyChoiceFeature)f).getSkills(this).Contains(s))
                        if (modifier < ((SkillProficiencyChoiceFeature)f).ProficiencyMultiplier)
                            modifier = ((SkillProficiencyChoiceFeature)f).ProficiencyMultiplier;
                }
                else if (f is SkillProficiencyFeature)
                {
                    double mod = ((SkillProficiencyFeature)f).ProficiencyMultiplier;
                    if (((SkillProficiencyFeature)f).Skills.Count == 0) { if (modifier < mod) modifier = mod; }
                    else if (((SkillProficiencyFeature)f).Skills.Contains(s.Name)) if (modifier < mod) modifier = mod;
                }
                else if (f is BonusFeature && !((BonusFeature)f).SkillPassive  && ((BonusFeature)f).SkillBonus != null && ((BonusFeature)f).SkillBonus.Trim() != "" && ((BonusFeature)f).SkillBonus.Trim() != "0" && Utils.matches((BonusFeature)f, armor, fc.classlevel, additionalKW, asa, true))
                {
                    if (((BonusFeature)f).Skills.Count == 0 || ((BonusFeature)f).Skills.Contains(s.Name)) bonus = Utils.evaluate(((BonusFeature)f).SkillBonus, asa, additionalKW, fc.classlevel, 0);
                }
            }
            int prof = getProficiency();
            Dictionary<Skill, int> res = new Dictionary<Skill, int>();
            return asa.ApplyMod(ability) + (int)Math.Floor(prof * modifier);
        }
        public int getPassiveSkill(Skill s)
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List <FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is SkillProficiencyChoiceFeature || t is SkillProficiencyFeature || t is BonusFeature);
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item weapon = getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            double modifier = 0;
            int bonus = 0;
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is SkillProficiencyChoiceFeature)
                {
                    if (((SkillProficiencyChoiceFeature)f).getSkills(this).Contains(s))
                        if (modifier < ((SkillProficiencyChoiceFeature)f).ProficiencyMultiplier)
                            modifier = ((SkillProficiencyChoiceFeature)f).ProficiencyMultiplier;
                }
                else if (f is SkillProficiencyFeature)
                {
                    double mod = ((SkillProficiencyFeature)f).ProficiencyMultiplier;
                    if (((SkillProficiencyFeature)f).Skills.Count == 0) { if (modifier < mod) modifier = mod; }
                    else if (((SkillProficiencyFeature)f).Skills.Contains(s.Name)) if (modifier < mod) modifier = mod;
                }
                else if (f is BonusFeature && ((BonusFeature)f).SkillBonus != null && ((BonusFeature)f).SkillBonus.Trim() != "" && ((BonusFeature)f).SkillBonus.Trim() != "0" && Utils.matches((BonusFeature)f, armor, fc.classlevel, additionalKW, asa, true))
                {
                    if (((BonusFeature)f).Skills.Count == 0 || ((BonusFeature)f).Skills.Contains(s.Name)) bonus = Utils.evaluate(((BonusFeature)f).SkillBonus, asa, additionalKW, fc.classlevel, 0);
                }
            }
            int prof = getProficiency();
            Dictionary<Skill, int> res = new Dictionary<Skill, int>();
            return 10 + bonus + asa.ApplyMod(s.Base) + (int)Math.Floor(prof * modifier);
        }

        public AbilityScoreArray getSavingThrowsBoni()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            AbilityScoreArray res = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is BonusFeature);
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item weapon = getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");

            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).SavingThrowBonus != null && ((BonusFeature)fc.feature).SavingThrowBonus.Trim() != "" && ((BonusFeature)fc.feature).SavingThrowBonus.Trim() != "0" && Utils.matches(((BonusFeature)fc.feature).Condition, asa, additionalKW, fc.classlevel, 0))
                {
                    res.AddBonus(Utils.evaluate(null, ((BonusFeature)fc.feature).SavingThrowBonus, asa, additionalKW, fc.classlevel), ((BonusFeature)fc.feature).SavingThrowAbility);

                }
            return res;
        }

        public Dictionary<Ability,int> getSaves()
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is SaveProficiencyFeature || t is BonusFeature);
            AbilityScoreArray bonus = new AbilityScoreArray(0, 0, 0, 0, 0, 0);
            Ability saves = Ability.None;
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item weapon = getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).SavingThrowBonus != null && ((BonusFeature)fc.feature).SavingThrowBonus.Trim() != "" && ((BonusFeature)fc.feature).SavingThrowBonus.Trim() != "0" && Utils.matches(((BonusFeature)fc.feature).Condition, asa, additionalKW, fc.classlevel, 0))
                {
                    bonus.AddBonus(Utils.evaluate(null, ((BonusFeature)fc.feature).SavingThrowBonus, asa, additionalKW, fc.classlevel), ((BonusFeature)fc.feature).SavingThrowAbility);

                }
            Dictionary<Ability,int> res=new Dictionary<Ability,int>();
            foreach (FeatureClass f in fa)
            {
                if (f.feature is SaveProficiencyFeature) saves |= ((SaveProficiencyFeature)f.feature).Ability;
            }
            int prof = getProficiency();
            res.Add(Ability.Strength, asa.strmod + (saves.HasFlag(Ability.Strength) ? prof : 0) + bonus.Strength);
            res.Add(Ability.Dexterity, asa.dexmod + (saves.HasFlag(Ability.Dexterity) ? prof : 0) + bonus.Dexterity);
            res.Add(Ability.Constitution, asa.conmod + (saves.HasFlag(Ability.Constitution) ? prof : 0) + bonus.Constitution);
            res.Add(Ability.Intelligence, asa.intmod + (saves.HasFlag(Ability.Intelligence) ? prof : 0) + bonus.Intelligence);
            res.Add(Ability.Wisdom, asa.wismod + (saves.HasFlag(Ability.Wisdom) ? prof : 0) + bonus.Wisdom);
            res.Add(Ability.Charisma, asa.chamod + (saves.HasFlag(Ability.Charisma) ? prof : 0) + bonus.Charisma);
            return res;
        }
        public int getAC(int level = 0)
        {
            if (level == 0) level = getLevel();
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List <FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is ACFeature || t is BonusFeature);
            Item armor=getArmor();
            Item offHand=getOffHand();
            Item mainHand=getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null)
            {
                additionalKW.Add("unarmored");
                armor = new Item();
                armor.Name="No Armor";
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
                    if (Utils.matches(b, armor, fc.classlevel, additionalKW, asa)) bonus+=Utils.evaluate(b.ACBonus, asa, additionalKW, fc.classlevel, level, armor);
                }
            }
            int AC = 0;
            int shieldbonus = 0;
            if (mainHand is Shield) shieldbonus = ((Shield)offHand).ACBonus;
            if (offHand is Shield && shieldbonus < ((Shield)offHand).ACBonus) shieldbonus = ((Shield)offHand).ACBonus;
            foreach (KeyValuePair<ACFeature, int> acf in ways)
            {
                int tac = Utils.CalcAC(acf.Key,armor,shieldbonus,additionalKW, asa, bonus, acf.Value);
                if (AC < tac) AC = tac;
            }
            return AC + bonus;
        }
        public Item getArmor()
        {
            foreach (Possession p in Possessions) if (string.Equals(p.Equipped, EquipSlot.Armor, StringComparison.InvariantCultureIgnoreCase)) return p.Item;
            return null;
        }
        public Item getMainHand()
        {
            foreach (Possession p in Possessions) if (string.Equals(p.Equipped, EquipSlot.MainHand, StringComparison.InvariantCultureIgnoreCase)) return p.Item;
            return null;
        }
        public Item getOffHand()
        {
            foreach (Possession p in Possessions) if (string.Equals(p.Equipped, EquipSlot.OffHand, StringComparison.InvariantCultureIgnoreCase)) return p.Item;
            return null;
        }
        public AttackInfo getAttack(Possession p,int level=0)
        {
            if (level == 0) level = getLevel();
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is BonusFeature||t is ExtraAttackFeature || t is ToolKWProficiencyFeature || t is ToolProficiencyChoiceConditionFeature || t is ToolProficiencyFeature,level,p.CollectOnUse(level, this));
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item mainHand = getMainHand();
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
                if (f is BonusFeature)
                {
                    BonusFeature b = (BonusFeature)f;
                    if ((b.BaseItemChange != null && b.BaseItemChange.Trim() != "")) {
                        if (Utils.matches(b, Ability.Strength | Ability.Dexterity, "Weapon", "Weapon", fc.classlevel, additionalKW) || (p.Item != null && Utils.matches(b, p.Item, fc.classlevel, additionalKW, asa)))
                        {
                            try
                            {
                                Item baseitem = Item.Get(((BonusFeature)f).BaseItemChange, ((BonusFeature)f).Source);
                                if (baseitem is Weapon) weapon = baseitem as Weapon;
                                break;
                            }
                            catch (Exception) { }
                        }
                    }
                    if ((b.ProficiencyOptions != null && b.ProficiencyOptions.Count > 0))
                    {
                        if (Utils.matches(b, Ability.Strength | Ability.Dexterity, "Weapon", "Weapon", fc.classlevel, additionalKW))
                        {
                            foreach (String i in b.ProficiencyOptions) {
                                try
                                {
                                    Item it = Item.Get(i, ((BonusFeature)f).Source);
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
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k=>k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            Ability baseAbility = Ability.Strength;
            

            if (weapon.Keywords.Exists(kw => kw.Name == "finesse") && asa.ApplyMod(Ability.Dexterity) > asa.ApplyMod(baseAbility)) baseAbility = Ability.Dexterity;
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
                if (f is BonusFeature)
                {
                    BonusFeature b = (BonusFeature)f;
                    if ((b.DamageBonus != null && b.DamageBonus.Trim() != "" && b.DamageBonus.Trim() != "0") || (b.DamageBonusText != null && b.DamageBonusText != "") || b.DamageBonusModifier != Ability.None || (b.AttackBonus != null && b.AttackBonus.Trim() != "" && b.AttackBonus.Trim() != "0"))
                    {
                        if (Utils.matches(b, weapon, fc.classlevel, additionalKW, asa) || Utils.matches(b, baseAbility, "Weapon","Weapon", fc.classlevel, additionalKW))
                        {
                            attackbonus += Utils.evaluate(b.AttackBonus, asa, additionalKW, fc.classlevel, level, weapon);
                            damagebonus += Utils.evaluate(b, asa, additionalKW, fc.classlevel, level, weapon);
                            if (b.DamageBonusText != null && b.DamageBonusText != "") damage += " " + b.DamageBonusText;
                            if (b.BaseAbility != Ability.None) baseAbility = asa.Highest(baseAbility | b.BaseAbility);
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
                        if (((ToolProficiencyChoiceConditionFeature)f).getTools(this).Exists(t => t.Name.ToLowerInvariant() == w.Name.ToLowerInvariant())) profbonus = true;
                    }
                }
                else if (f is ToolKWProficiencyFeature)
                {
                    foreach (Weapon w in countsAs)
                    {
                        if (Utils.matches(((ToolKWProficiencyFeature)f), w, fc.classlevel)) profbonus = true;
                    }
                }
            }
            int ability = asa.ApplyMod(baseAbility); ;
            attackbonus += ability;
            damagebonus += ability;
            if (profbonus) attackbonus += getProficiency(level);
            if (damagebonus > 0) damage += "+" + damagebonus;
            else if (damagebonus < 0) damage += damagebonus;
            extraatk++;
            if (extraatk > 1) damage = extraatk.ToString() + " x " + damage;
            return new AttackInfo(attackbonus,damage,damagetype);
        }
        public AttackInfo getAttack(Spell s, Ability spellcastingModifier, int level = 0)
        {
            if (level == 0) level = getLevel();
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List < FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is BonusFeature, level);
            Item armor = getArmor();
            Item offHand = getOffHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (getMainHand() is Weapon) additionalKW.Add("mainhand");
            additionalKW.Add("Spell");
            int attackbonus = asa.ApplyMod(spellcastingModifier);
            attackbonus += getProficiency(level);
            int savedc = 8 + attackbonus;
            int damagebonus = 0;
            String damage = s.GetCantripDamage(level);
            string damagetype = s.GetDamageType();
            foreach (FeatureClass fc in fa)
            {
                Feature f = fc.feature;
                if (f is BonusFeature)
                {
                    BonusFeature b = (BonusFeature)f;
                    if ((b.DamageBonus != null && b.DamageBonus.Trim() != "" && b.DamageBonus.Trim() != "0") || (b.DamageBonusText != null && b.DamageBonusText != "") || b.DamageBonusModifier != Ability.None || (b.AttackBonus != null && b.AttackBonus.Trim() != "" && b.AttackBonus.Trim() != "0") || (b.SaveDCBonus != null && b.SaveDCBonus.Trim() != "" && b.SaveDCBonus.Trim() != "0"))
                    {
                        if (Utils.matches(b, s, fc.classlevel, additionalKW) || Utils.matches(b, spellcastingModifier, "Spell", "Spell", fc.classlevel, additionalKW))
                        {
                            attackbonus += Utils.evaluate(s, b.AttackBonus, asa, additionalKW, fc.classlevel, level);
                            damagebonus += Utils.evaluate(s, b, asa, additionalKW, fc.classlevel, level);
                            savedc += Utils.evaluate(s, b.SaveDCBonus, asa, additionalKW, fc.classlevel, level);
                            damagebonus += asa.ApplyMod(b.DamageBonusModifier);
                            if (b.DamageBonusText != null && b.DamageBonusText != "") damage += "+" + b.DamageBonusText;
                        }
                    }
                }
            }
            if (damagebonus > 0) damage += "+" + damagebonus;
            else if (damagebonus < 0) damage += damagebonus;
            if (Utils.matches(s, "Save", null)) return new AttackInfo(savedc.ToString(), damage, damagetype);
            else if (Utils.matches(s, "Attack", null)) return new AttackInfo(attackbonus, damage, damagetype);
            else return null;
        }
        public int getInitiative(int level = 0)
        {
            if (level == 0) level = getLevel();
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is BonusFeature);
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item weapon = getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            Ability baseAbility = Ability.Dexterity;
            int bonus = 0;
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).InitiativeBonus != null && ((BonusFeature)fc.feature).InitiativeBonus.Trim() != "" && ((BonusFeature)fc.feature).InitiativeBonus.Trim() != "0" && Utils.matches(fc.feature as BonusFeature, weapon, fc.classlevel, additionalKW, asa)) bonus += Utils.evaluate(((BonusFeature)fc.feature).InitiativeBonus, asa, null, fc.classlevel, level);
            return asa.ApplyMod(baseAbility) + bonus;
        }
        public int getLevel()
        {
            return Level.Get(getXP());
        }
        public int getProficiency(int level = 0)
        {
            if (level == 0) level = getLevel();
            int prof = Level.GetProficiency(level);
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is BonusFeature);
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item weapon = getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            int bonus = 0;
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).ProficiencyBonus != null && ((BonusFeature)fc.feature).ProficiencyBonus.Trim() != "" && ((BonusFeature)fc.feature).ProficiencyBonus.Trim() != "0" && Utils.matches(fc.feature as BonusFeature, weapon, fc.classlevel, additionalKW, asa)) bonus += Utils.evaluate(((BonusFeature)fc.feature).ProficiencyBonus, asa, null, fc.classlevel, level);
            return prof + bonus;
        }
        public int getSpellSaveDC(string SpellcastingID, Ability baseAbility) {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is BonusFeature);
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item weapon = getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            additionalKW.Add("Spell");
            int bonus = 0;
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).SaveDCBonus != null && ((BonusFeature)fc.feature).SaveDCBonus.Trim() != "" && ((BonusFeature)fc.feature).SaveDCBonus.Trim() != "0" && Utils.matches(((BonusFeature)fc.feature), baseAbility, SpellcastingID, "Spell", fc.classlevel)) bonus += Utils.evaluate(null, ((BonusFeature)fc.feature).SaveDCBonus, asa, additionalKW, fc.classlevel);
            return 8+getProficiency()+  asa.ApplyMod(baseAbility) + bonus;
        }
        public int getSpellAttack(string SpellcastingID, Ability baseAbility)
        {
            AbilityScoreArray asa;
            AbilityScoreArray max;
            List<FeatureClass> fa = getFeatureAndAbility(out asa, out max, t => t is BonusFeature);
            Item armor = getArmor();
            Item offHand = getOffHand();
            Item weapon = getMainHand();
            List<string> additionalKW = new List<string>();
            if (armor == null) additionalKW.Add("unarmored");
            if (offHand == null || (offHand.Keywords != null && offHand.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase))) || (weapon != null && weapon.Keywords != null && weapon.Keywords.Exists(k => k.Name.Equals("unarmed", StringComparison.InvariantCultureIgnoreCase)))) additionalKW.Add("freehand");
            if (offHand is Weapon) additionalKW.Add("offhand");
            if (offHand is Shield) additionalKW.Add("shield");
            if (weapon is Weapon) additionalKW.Add("mainhand");
            additionalKW.Add("Spell");
            int bonus = 0;
            foreach (FeatureClass fc in fa) if (fc.feature is BonusFeature && ((BonusFeature)fc.feature).AttackBonus != null && ((BonusFeature)fc.feature).AttackBonus.Trim() != "" && ((BonusFeature)fc.feature).AttackBonus.Trim() != "0" && Utils.matches(((BonusFeature)fc.feature), baseAbility, SpellcastingID, "Spell", fc.classlevel, additionalKW)) bonus += Utils.evaluate(null, ((BonusFeature)fc.feature).AttackBonus, asa, additionalKW, fc.classlevel);
            return getProficiency() + asa.ApplyMod(baseAbility) + bonus;
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

        public bool matches(String expression, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            return Utils.matches(expression, new AbilityScoreArray(BaseStrength, BaseDexterity, BaseConstitution, BaseIntelligence, BaseWisdom, BaseCharisma), additionalKeywords, classlevel, level);
        }

        public int getXP(bool onlyJournal = false)
        {
            int x = onlyJournal ? 0 : XP;
            foreach (JournalEntry e in ComplexJournal)
            {
                x += e.XP;
            }
            return x;
        }
        public void setXP(int xp)
        {
            foreach (JournalEntry e in ComplexJournal)
            {
                xp -= e.XP;
            }
            XP = xp;
        }

        public void removeBoon(Feature feature)
        {
            string boon = feature.Name + " " + ConfigManager.SourceSeperator + " " + feature.Source;
            int index = Boons.FindIndex(s => ConfigManager.SourceInvariantComparer.Equals(s, boon));
            if (index >= 0) Boons.RemoveAt(index);
        }
    }
}
