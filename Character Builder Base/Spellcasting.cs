using OGL;
using OGL.Base;
using OGL.Features;
using OGL.Keywords;
using OGL.Spells;
using System.Collections.Generic;
using System.Linq;

namespace Character_Builder
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
        public List<ModifiedSpell> GetPrepared(Player player, OGLContext context, int level = 0)
        {
            CombinePrepared(player, context, level);
            List<ModifiedSpell> res = new List<ModifiedSpell>(from s in Prepared select new ModifiedSpell(context.GetSpell(s, null), null, false, false));
            
            foreach (Feature f in player.GetFeatures(level))
            {
                if (f is BonusSpellPrepareFeature bspf && bspf.SpellcastingID == SpellcastingID && bspf.AddTo == PreparationMode.LearnSpells)
                {
                    foreach (string s in bspf.Spells)
                    {
                        res.Add(new ModifiedSpell(context.GetSpell(s, bspf.Source), bspf.KeywordsToAdd, true, false));
                    }
                    res.AddRange(Utils.FilterSpell(player.Context, bspf.Condition, bspf.SpellcastingID).Select(s=>new ModifiedSpell(s, bspf.KeywordsToAdd, true, false)));
                }
            }
            res.Sort();
            return res;
        }
        public IEnumerable<ModifiedSpell> GetLearned(Player player, OGLContext context, int level = 0)
        {
            List<ModifiedSpell> res = new List<ModifiedSpell>();
            CombineSpellChoices(player, context, level);
            foreach (Feature f in player.GetFeatures(level))
                if (f is SpellChoiceFeature && ((SpellChoiceFeature)f).SpellcastingID == SpellcastingID)
                {
                    foreach (SpellChoice s in Spellchoices)
                    {
                        SpellChoiceFeature scf = (SpellChoiceFeature)f;
                        if (s.UniqueID == scf.UniqueID && scf.AddTo == PreparationMode.LearnSpells)
                        {
                            res.AddRange(from spell in s.Choices select new ModifiedSpell(context.GetSpell(spell, f.Source), scf.KeywordsToAdd, false, false));
                        }
                    }
                }
            res.Sort();
            return res;
        }
        public IEnumerable<Spell> GetSpellbook(Player player, OGLContext context, int level = 0)
        {
            List<Spell> res = new List<Spell>();
            CombineSpellChoices(player, context, level);
            CombineSpellbookAdditional(player, context, level);
            string sourcehint = null;
            foreach (Feature f in player.GetFeatures(level))
            {
                if (f is SpellChoiceFeature && ((SpellChoiceFeature)f).SpellcastingID == SpellcastingID)
                {
                    sourcehint = f.Source;
                    foreach (SpellChoice s in Spellchoices)
                    {
                        SpellChoiceFeature scf = (SpellChoiceFeature)f;
                        if (s.UniqueID == scf.UniqueID && scf.AddTo == PreparationMode.Spellbook)
                        {
                            res.AddRange(from spell in s.Choices select new ModifiedSpell(context.GetSpell(spell, scf.Source), scf.KeywordsToAdd, false, false));
                        }
                    }
                }
                else if (f is BonusSpellPrepareFeature bspf && bspf.SpellcastingID == SpellcastingID && bspf.AddTo == PreparationMode.Spellbook)
                {
                    foreach (string s in bspf.Spells)
                    {
                        res.Add(new ModifiedSpell(context.GetSpell(s, bspf.Source), bspf.KeywordsToAdd, true, false));
                    }
                    res.AddRange(Utils.FilterSpell(player.Context, bspf.Condition, bspf.SpellcastingID).Select(s => new ModifiedSpell(s, bspf.KeywordsToAdd, false, false)));
                }
            }
            res.AddRange(from s in SpellbookAdditional select context.GetSpell(s, sourcehint));
            res.Sort();
            return res;

        }
        public IEnumerable<Spell> GetAdditionalClassSpells(Player player, OGLContext context, int level = 0)
        {
            List<Spell> res = new List<Spell>();
            CombineSpellbookAdditional(player, context, level);
            CombineSpellChoices(player, context, level);
            string sourcehint = null;
            foreach (Feature f in player.GetFeatures(level))
            {
                if (f is SpellChoiceFeature && ((SpellChoiceFeature)f).SpellcastingID == SpellcastingID)
                {
                    sourcehint = f.Source;
                    foreach (SpellChoice s in Spellchoices)
                    {
                        SpellChoiceFeature scf = (SpellChoiceFeature)f;
                        if (s.UniqueID == scf.UniqueID && scf.AddTo == PreparationMode.ClassList)
                        {
                            res.AddRange(from spell in s.Choices select new ModifiedSpell(context.GetSpell(spell, scf.Source), scf.KeywordsToAdd));
                        }
                    }
                }
                else if (f is BonusSpellPrepareFeature bspf && bspf.SpellcastingID == SpellcastingID && bspf.AddTo == PreparationMode.ClassList)
                {
                    foreach (string s in bspf.Spells)
                    {
                        res.Add(new ModifiedSpell(context.GetSpell(s, bspf.Source), bspf.KeywordsToAdd, true, false));
                    }
                    res.AddRange(Utils.FilterSpell(player.Context, bspf.Condition, bspf.SpellcastingID).Select(s => new ModifiedSpell(s, bspf.KeywordsToAdd, false, false)));
                }
            }
            res.AddRange(from s in SpellbookAdditional select context.GetSpell(s, sourcehint));
            res.Sort();
            return res;
        }

        public IEnumerable<ModifiedSpell> GetSpellbookRituals(Player player, OGLContext context, int level = 0)
        {
            List<ModifiedSpell> res = new List<ModifiedSpell>();
            List<ModifiedSpell> excluded = GetPrepared(player, context, level);
            foreach (Spell s in GetSpellbook(player, context, level)) {
                if (s.GetKeywords().Contains(new Keyword("Ritual")) && !excluded.Exists(e => e.Name == s.Name && e.Source == s.Source)) res.Add(new ModifiedSpell(s, true));
            }
            res.Sort();
            return res;
        }

        public IEnumerable<ModifiedSpell> GetCLassListRituals(string classlist, Player player, BuilderContext context, int level = 0)
        {
            List<ModifiedSpell> res = new List<ModifiedSpell>();
            List<ModifiedSpell> excluded = GetPrepared(player, context, level);
            foreach (Spell s in Utils.FilterSpell(context, classlist, SpellcastingID, player.GetClassLevel(SpellcastingID, level)))
            {
                if (s.GetKeywords().Contains(new Keyword("Ritual")) && !excluded.Exists(e => e.Name == s.Name && e.Source == s.Source)) res.Add(new ModifiedSpell(s, true));
            }
            foreach (Spell s in GetAdditionalClassSpells(player, context, level))
            {
                if (s.GetKeywords().Contains(new Keyword("Ritual")) && !excluded.Exists(e => e.Name == s.Name && e.Source == s.Source)) res.Add(new ModifiedSpell(s, true));
            }
            res.Sort();
            return res;
        }

        public List<string> GetPreparedList(Player player, OGLContext context, int level = 0)
        {
            if (level ==0) {
                level = player.GetLevel();
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

        public List<string> GetAdditionalList(Player player, OGLContext context, int level = 0)
        {
            if (level == 0)
            {
                level = player.GetLevel();
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

        private void CombinePrepared(Player player, OGLContext context, int level = 0)
        {
            if (level ==0) {
                level = player.GetLevel();
            }
            SpellPerLevel result = (from prep in PreparedPerLevel where prep.Level <= level orderby prep.Level descending select prep).FirstOrDefault();
            if (result != null) Prepared = new List<string>(result.Spells);
            else Prepared = new List<string>();
        }

        private void CombineSpellbookAdditional(Player player, OGLContext context, int level = 0)
        {
            if (level == 0)
            {
                level = player.GetLevel();
            }
            SpellPerLevel result = (from prep in SpellbookAdditionalPerLevel where prep.Level <= level orderby prep.Level descending select prep).FirstOrDefault();
            if (result != null) SpellbookAdditional = new List<string>(result.Spells);
            else SpellbookAdditional = new List<string>();
        }

        private void CombineSpellChoices(Player player, OGLContext context, int level = 0)
        {
            if (level == 0)
            {
                level = player.GetLevel();
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

        public void PostLoad(int level)
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
