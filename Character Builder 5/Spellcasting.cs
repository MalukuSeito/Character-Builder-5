using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class Spellcasting
    {
        public string SpellcastingID;
        public List<SpellPerLevel> PreparedPerLevel = new List<SpellPerLevel>();
        public List<string> Prepared = new List<string>();
        public List<SpellChoicePerLevel> SpellChoicesPerLevel = new List<SpellChoicePerLevel>();
        public List<SpellChoice> Spellchoices = new List<SpellChoice>();
        public List<SpellPerLevel> SpellbookAdditionalPerLevel = new List<SpellPerLevel>();
        public List<string> SpellbookAdditional = new List<string>();
        public List<int> UsedSlots = new List<int>();
        public string Highlight { get; set; }
        //[XmlIgnore]
        // public string Displayname { get; set; }
        public IEnumerable<ModifiedSpell> getPrepared(int level = 0)
        {
            combinePrepared(level);
            List<ModifiedSpell> res = new List<ModifiedSpell>(from s in Prepared select new ModifiedSpell(Spell.Get(s, null), null, false));
            
            foreach (Feature f in Player.current.getFeatures(level))
            {
                if (f is BonusSpellPrepareFeature && ((BonusSpellPrepareFeature)f).SpellcastingID == SpellcastingID)
                {
                    BonusSpellPrepareFeature bspf = (BonusSpellPrepareFeature)f;
                    foreach (string s in bspf.Spells)
                    {
                        res.Add(new ModifiedSpell(Spell.Get(s, bspf.Source), bspf.KeywordsToAdd, true));
                    }
                }
            }
            res.Sort();
            return res;
        }
        public IEnumerable<ModifiedSpell> getLearned(int level = 0)
        {
            List<ModifiedSpell> res = new List<ModifiedSpell>();
            combineSpellChoices(level);
            foreach (Feature f in Player.current.getFeatures(level))
                if (f is SpellChoiceFeature && ((SpellChoiceFeature)f).SpellcastingID == SpellcastingID)
                {
                    foreach (SpellChoice s in Spellchoices)
                    {
                        SpellChoiceFeature scf = (SpellChoiceFeature)f;
                        if (s.UniqueID == scf.UniqueID && scf.AddTo == PreparationMode.LearnSpells)
                        {
                            res.AddRange(from spell in s.Choices select new ModifiedSpell(Spell.Get(spell, f.Source), scf.KeywordsToAdd));
                        }
                    }
                }
            res.Sort();
            return res;
        }
        public IEnumerable<Spell> getSpellbook(int level = 0)
        {
            List<Spell> res = new List<Spell>();
            combineSpellChoices(level);
            combineSpellbookAdditional(level);
            string sourcehint = null;
            foreach (Feature f in Player.current.getFeatures(level))
                if (f is SpellChoiceFeature && ((SpellChoiceFeature)f).SpellcastingID == SpellcastingID)
                {
                    sourcehint = f.Source;
                    foreach (SpellChoice s in Spellchoices)
                    {
                        SpellChoiceFeature scf = (SpellChoiceFeature)f;
                        if (s.UniqueID == scf.UniqueID && scf.AddTo == PreparationMode.Spellbook)
                        {
                            res.AddRange(from spell in s.Choices select new ModifiedSpell(Spell.Get(spell, scf.Source), scf.KeywordsToAdd, false, false));
                        }
                    }
                }
            res.AddRange(from s in SpellbookAdditional select Spell.Get(s, sourcehint));
            res.Sort();
            return res;

        }
        public IEnumerable<Spell> getAdditionalClassSpells(int level = 0)
        {
            List<Spell> res = new List<Spell>();
            combineSpellbookAdditional(level);
            combineSpellChoices(level);
            string sourcehint = null;
            foreach (Feature f in Player.current.getFeatures(level))
                if (f is SpellChoiceFeature && ((SpellChoiceFeature)f).SpellcastingID == SpellcastingID)
                {
                    sourcehint = f.Source;
                    foreach (SpellChoice s in Spellchoices)
                    {
                        SpellChoiceFeature scf = (SpellChoiceFeature)f;
                        if (s.UniqueID == scf.UniqueID && scf.AddTo == PreparationMode.ClassList)
                        {
                            res.AddRange(from spell in s.Choices select new ModifiedSpell(Spell.Get(spell, scf.Source), scf.KeywordsToAdd));
                        }
                    }
                }
            res.AddRange(from s in SpellbookAdditional select Spell.Get(s, sourcehint));
            res.Sort();
            return res;
        }

        public List<string> getPreparedList(int level = 0)
        {
            if (level ==0) {
                level = Player.current.getLevel();
            }
            SpellPerLevel result = (from prep in PreparedPerLevel where prep.Level <= level orderby prep.Level descending select prep).FirstOrDefault();
            if (result != null)
            {
                if (result.Level != level)
                {
                    result = new SpellPerLevel(level, new List<string>(result.Spells));
                    PreparedPerLevel.Add(result);
                    return result.Spells;
                }
                return result.Spells;
            }
            result = new SpellPerLevel(level, new List<string>());
            PreparedPerLevel.Add(result);
            return result.Spells;
        }

        public List<string> getAdditionalList(int level = 0)
        {
            if (level == 0)
            {
                level = Player.current.getLevel();
            }
            SpellPerLevel result = (from prep in SpellbookAdditionalPerLevel where prep.Level <= level orderby prep.Level descending select prep).FirstOrDefault();
            if (result != null)
            {
                if (result.Level != level)
                {
                    result = new SpellPerLevel(level, new List<string>(result.Spells));
                    SpellbookAdditionalPerLevel.Add(result);
                    return result.Spells;
                }
                return result.Spells;
            }
            result = new SpellPerLevel(level, new List<string>());
            SpellbookAdditionalPerLevel.Add(result);
            return result.Spells;
        }

        private void combinePrepared(int level = 0)
        {
            if (level ==0) {
                level = Player.current.getLevel();
            }
            SpellPerLevel result = (from prep in PreparedPerLevel where prep.Level <= level orderby prep.Level descending select prep).FirstOrDefault();
            if (result != null) Prepared = new List<string>(result.Spells);
            else Prepared = new List<string>();
        }

        private void combineSpellbookAdditional(int level = 0)
        {
            if (level == 0)
            {
                level = Player.current.getLevel();
            }
            SpellPerLevel result = (from prep in SpellbookAdditionalPerLevel where prep.Level <= level orderby prep.Level descending select prep).FirstOrDefault();
            if (result != null) SpellbookAdditional = new List<string>(result.Spells);
            else SpellbookAdditional = new List<string>();
        }

        private void combineSpellChoices(int level = 0)
        {
            if (level == 0)
            {
                level = Player.current.getLevel();
            }
            Spellchoices.Clear();
            foreach (SpellChoicePerLevel scpl in from prep in SpellChoicesPerLevel where prep.Level <= level orderby prep.Level descending select prep) {
                 foreach (SpellChoice s in scpl.Choices) {
                    foreach (SpellChoice sc in Spellchoices) {
                        if (s.UniqueID == sc.UniqueID) {
                            goto NEXT;
                        }
                    }
                    Spellchoices.Add(new SpellChoice(s));
                NEXT: ;
                 }            
            }

            /*foreach (SpellChoicePerLevel s in SpellChoicesPerLevel)
            {
                if (s.Level <= level)
                {
                    foreach (SpellChoice sc in s.Choices)
                    {
                        SpellChoice target = null;
                        foreach (SpellChoice cur in Spellchoices)
                        {
                            if (sc.UniqueID == cur.UniqueID)
                            {
                                target = cur;
                                break;
                            }
                        }
                        if (target == null)
                        {
                            target = new SpellChoice();
                            target.UniqueID = sc.UniqueID;
                            Spellchoices.Add(target);
                        }
                        for (int i = 0; i < sc.Choices.Count; i++)
                        {
                            while (i >= target.Choices.Count) target.Choices.Add(sc.Choices[i]);
                            if (sc.Choices[i] != null) target.Choices[i] = sc.Choices[i];
                        }
                    }
                }
            }*/
        }

        internal void postLoad(int level)
        {
            if (Spellchoices.Count > 0)
            {
                if (SpellChoicesPerLevel.Count == 0)
                {
                    SpellChoicesPerLevel.Add(new SpellChoicePerLevel(level, new List<SpellChoice>(Spellchoices)));
                }
                Spellchoices.Clear();
            }
            if (SpellbookAdditional.Count > 0)
            {
                if (SpellbookAdditionalPerLevel.Count == 0)
                {
                    SpellbookAdditionalPerLevel.Add(new SpellPerLevel(level, new List<string>(SpellbookAdditional)));
                }
                SpellbookAdditional.Clear();
            }
            if (Prepared.Count > 0)
            {
                if (PreparedPerLevel.Count == 0)
                {
                    PreparedPerLevel.Add(new SpellPerLevel(level, new List<string>(Prepared)));
                }
                Prepared.Clear();
            }
        }
    }
}
