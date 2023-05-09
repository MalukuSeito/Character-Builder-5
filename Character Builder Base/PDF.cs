using Character_Builder;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using OGL.Items;
using OGL.Keywords;
using OGL.Monsters;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Character_Builder
{
    public class PDF
    {
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(PDF));
        public String Name { get; set; }
        public String File { get; set; }
        public String SpellFile { get; set; }
        public String LogFile { get; set; }
        public String SpellbookFile { get; set; }
        public String MonstersFile { get; set; }
        public String ActionsFile { get; set; }
        public String ActionsFile2 { get; set; }
        public List<PDFField> Fields = new List<PDFField>();
        public List<PDFField> SpellFields = new List<PDFField>();
        public List<PDFField> LogFields = new List<PDFField>();
        public List<PDFField> SpellbookFields = new List<PDFField>();
        public List<PDFField> ActionsFields = new List<PDFField>();
        public List<PDFField> MonsterFields = new List<PDFField>();
        public List<PDFField> ActionsFields2 = new List<PDFField>();

        public string SheetName { get; set; }
        public string SpellName { get; set; }
        public string LogName { get; set; }
        public string SpellbookName { get; set; }
        public string MonstersName { get; set; }
        public string ActionsName { get; set; }

        public byte[] SheetPreview { get; set; }
        public byte[] SpellPreview { get; set; }
        public byte[] LogPreview { get; set; }
        public byte[] SpellbookPreview { get; set; }
        public byte[] MonstersPreview { get; set; }
        public byte[] ActionsPreview { get; set; }


        public static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.Substring(0, 1).ToUpper() + input.Substring(1);
            }
        }

        public async Task Export(BuilderContext context, PDFBase pdf)
        {
            Dictionary<String, String> trans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> spelltrans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> logtrans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> booktrans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> actiontrans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> actiontrans2 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> monstertrans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (PDFField pf in Fields) trans.Add(pf.Name, pf.Field);
            foreach (PDFField pf in SpellFields) spelltrans.Add(pf.Name, pf.Field);
            foreach (PDFField pf in LogFields) logtrans.Add(pf.Name, pf.Field);
            foreach (PDFField pf in SpellbookFields) booktrans.Add(pf.Name, pf.Field);
            foreach (PDFField pf in ActionsFields) actiontrans.Add(pf.Name, pf.Field);
            foreach (PDFField pf in ActionsFields2) actiontrans2.Add(pf.Name, pf.Field);
            foreach (PDFField pf in MonsterFields) monstertrans.Add(pf.Name, pf.Field);
            Dictionary<string, bool> hiddenfeats = context.Player.HiddenFeatures.Distinct().ToDictionary(f => f, f => true, StringComparer.OrdinalIgnoreCase);
            Dictionary<Feature, bool> hiddenactions = pdf.IncludeActions && pdf.AutoExcludeActions ? context.Player.GetActions().Where(f=>f.Feature != null).Select(f=>f.Feature).Distinct(new ObjectIdentityEqualityComparer()).ToDictionary(f => f, f => true, new ObjectIdentityEqualityComparer()) : new Dictionary<Feature, bool>();
            hiddenfeats.Add("", true);
            List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>(from f in context.Player.GetFeatures() where f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID != "MULTICLASS" select (SpellcastingFeature)f);
            List<Spell> spellbook = new List<Spell>();
            List<HitDie> hd = context.Player.GetHitDie();
            int maxhp = context.Player.GetHitpointMax();
            using (IPDFEditor p = await pdf.CreateEditor(File))
            {

                //}
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    using (PdfReader sheet = new PdfReader(File))
                //    {
                //        if (preserveEdit) sheet.RemoveUsageRights();
                //        using (PdfStamper p = new PdfStamper(sheet, ms))
                //        {
                FillBasicFields(trans, p, context, pdf, true);
                String attacks = "";
                String resources = "";
                if (trans.ContainsKey("Actions"))
                {
                    p.SetField(trans["Actions"], String.Join("\n", context.Player.GetActions()));
                }
                if (trans.ContainsKey("Resources"))
                {
                    resources = String.Join("\n", context.Player.GetResourceInfo(pdf.IncludeResources).Values);
                }
                else
                {
                    if (attacks != "") attacks += "\n";
                    attacks += String.Join("\n", context.Player.GetResourceInfo(pdf.IncludeResources).Values);
                }
                List<ModifiedSpell> bonusspells = new List<ModifiedSpell>(context.Player.GetBonusSpells(!trans.ContainsKey("BonusSpells") && pdf.BonusSpellsAreResources == PDFBase.ExceptAtWillAndCharges));
                if (pdf.IncludeResources)
                {
                    Dictionary<string, int> bsres = context.Player.GetResources();
                    foreach (ModifiedSpell mods in bonusspells)
                    {
                        mods.includeResources = true;
                        if (bsres.ContainsKey(mods.getResourceID())) mods.used = bsres[mods.getResourceID()];
                    }
                }
                else
                {
                    foreach (ModifiedSpell mods in bonusspells)
                    {
                        mods.includeResources = false;
                    }
                }
                spellbook.AddRange(context.Player.GetBonusSpells());
                if (trans.ContainsKey("BonusSpells"))
                {
                    p.SetField(trans["BonusSpells"], String.Join("\n", bonusspells));
                }
                else if (trans.ContainsKey("Resources") && pdf.BonusSpellsAreResources > PDFBase.None)
                {
                    if (resources != "") resources += "\n";
                    resources += String.Join("\n", bonusspells);
                }
                else if (pdf.BonusSpellsAreResources > PDFBase.None)
                {
                    if (attacks != "") attacks += "\n";
                    attacks += String.Join("\n", bonusspells);
                }

                if (trans.ContainsKey("Resources"))
                {
                    p.SetField(trans["Resources"], resources);
                }
                if (trans.ContainsKey("Attacks")) p.SetField(trans["Attacks"], attacks);
                if (trans.ContainsKey("HitDieTotal")) p.SetField(trans["HitDieTotal"], String.Join(", ", from h in hd select h.Total()));
                if (trans.ContainsKey("MaxHP")) p.SetField(trans["MaxHP"], maxhp.ToString());
                if (trans.ContainsKey("Resistances")) p.SetField(trans["Resistances"], FirstCharToUpper(String.Join(", ", context.Player.GetResistances()).ToLowerInvariant()));
                if (trans.ContainsKey("Vulnerabilities")) p.SetField(trans["Vulnerabilities"], FirstCharToUpper(String.Join(", ", context.Player.GetVulnerabilities()).ToLowerInvariant()));
                if (trans.ContainsKey("Immunities")) p.SetField(trans["Immunities"], FirstCharToUpper(String.Join(", ", context.Player.GetImmunities()).ToLowerInvariant()));
                if (trans.ContainsKey("SavingThrowAdvantages")) p.SetField(trans["SavingThrowAdvantages"], FirstCharToUpper(String.Join(", ", context.Player.GetSavingThrowAdvantages()).ToLowerInvariant()));
                if (trans.ContainsKey("ResistancesImmunities"))
                {
                    StringBuilder sb = new StringBuilder();
                    List<String> res = context.Player.GetResistances().ToList();
                    if (res.Count > 0) sb.Append("Resistances: ").Append(String.Join(", ", res));
                    List<String> imm = context.Player.GetResistances().ToList();
                    if (imm.Count > 0) sb.Append("Immunities: ").Append(String.Join(", ", imm));
                    List<String> vuln = context.Player.GetResistances().ToList();
                    if (vuln.Count > 0) sb.Append("Vulnerabilities: ").Append(String.Join(", ", vuln));
                    List<String> sadv = context.Player.GetSavingThrowAdvantages().ToList();
                    if (vuln.Count > 0) sb.Append("Advantages on Saving Throws vs: ").Append(String.Join(", ", sadv));
                    if (sb.Length > 0)
                    {
                        p.SetField(trans["ResistancesImmunities"], sb.ToString());
                    }
                }
                if (pdf.IncludeResources)
                {
                    if (trans.ContainsKey("CurrentHP")) p.SetField(trans["CurrentHP"], (maxhp + context.Player.CurrentHPLoss).ToString());
                    if (trans.ContainsKey("TempHP")) p.SetField(trans["TempHP"], context.Player.TempHP.ToString());
                    for (int d = 1; d <= context.Player.FailedDeathSaves; d++) if (trans.ContainsKey("DeathSaveFail" + d)) p.SetField(trans["DeathSaveFail" + d], "Yes");
                        else break;
                    for (int d = 1; d <= context.Player.SuccessDeathSaves; d++) if (trans.ContainsKey("DeathSaveSuccess" + d)) p.SetField(trans["DeathSaveSuccess" + d], "Yes");
                        else break;
                    if (trans.ContainsKey("HitDie")) p.SetField(trans["HitDie"], String.Join(", ", hd));
                    if (trans.ContainsKey("Inspiration")) if (context.Player.Inspiration) p.SetField(trans["Inspiration"], "Yes");
                    foreach (var e in actiontrans.AsEnumerable())
                    {
                        if (e.Key.StartsWith("Condition"))
                        {
                            if (context.Player.Conditions.Exists(s => StringComparer.OrdinalIgnoreCase.Equals("Condition" + s, e.Key)))
                            {
                                p.SetField(actiontrans[e.Value], "Yes");
                            }
                        }
                    }
                }
                if (context.Player.Portrait != null) p.SetImage(trans["Portrait"], context.Player.Portrait);
                if (context.Player.FactionImage != null) p.SetImage(trans["FactionPortrait"], context.Player.FactionImage);
                foreach (Skill s in context.Skills.Values) if (trans.ContainsKey("Passive" + s.Name)) p.SetField(trans["Passive" + s.Name], context.Player.GetPassiveSkill(s).ToString());
                foreach (Skill s in context.Player.GetSkillProficiencies()) p.SetField(trans[s.Name + "Proficiency"], "Yes");
                foreach (SkillInfo s in context.Player.GetSkills()) p.SetField(trans[s.Skill.Name], PlusMinus(s.Roll));
                Ability saveprof = context.Player.GetSaveProficiencies();
                if (saveprof.HasFlag(Ability.Strength)) p.SetField(trans["StrengthSaveProficiency"], "Yes");
                if (saveprof.HasFlag(Ability.Dexterity)) p.SetField(trans["DexteritySaveProficiency"], "Yes");
                if (saveprof.HasFlag(Ability.Constitution)) p.SetField(trans["ConstitutionSaveProficiency"], "Yes");
                if (saveprof.HasFlag(Ability.Intelligence)) p.SetField(trans["IntelligenceSaveProficiency"], "Yes");
                if (saveprof.HasFlag(Ability.Wisdom)) p.SetField(trans["WisdomSaveProficiency"], "Yes");
                if (saveprof.HasFlag(Ability.Charisma)) p.SetField(trans["CharismaSaveProficiency"], "Yes");
                foreach (KeyValuePair<Ability, int> v in context.Player.GetSaves()) p.SetField(trans[v.Key.ToString("F") + "Save"], PlusMinus(v.Value));
                List<String> profs = new List<string>();
                profs.Add(String.Join(", ", context.Player.GetLanguages()));
                profs.Add(String.Join(", ", context.Player.GetToolProficiencies()));
                profs.Add(String.Join("; ", context.Player.GetToolKWProficiencies()));
                //                        foreach (List<Keyword> kws in context.Player.getToolKWProficiencies()) profs.Add("Any "+String.Join(", ", kws));
                profs.Add(String.Join(", ", context.Player.GetOtherProficiencies()));
                profs.RemoveAll(t => t == "");
                p.SetField(trans["Proficiencies"], String.Join("\n", profs));
                Price money = context.Player.GetMoney();
                int level = context.Player.GetLevel();
                // TODO Proper Items:
                List<Possession> equip = new List<Possession>();
                List<String> equipDetailed = new List<String>();
                List<IInfoText> equipDetailed2 = new List<IInfoText>();
                List<Possession> treasure = new List<Possession>();
                List<IInfoText> treasureDetailed = new List<IInfoText>();
                List<Feature> onUse = new List<Feature>();

                foreach (Possession pos in context.Player.GetItemsAndPossessions())
                {
                    if (pos.Count > 0)
                    {
                        if (pos.BaseItem != null && pos.BaseItem != "") equip.Add(pos);
                        if ((trans.ContainsKey("Equipment") || trans.ContainsKey("EquipmentShort") || trans.ContainsKey("EquipmentDetailed")) && pos.BaseItem != null && pos.BaseItem != "")
                        {
                            Item i = context.GetItem(pos.BaseItem, null);
                            if (pos.Equipped != EquipSlot.None || i is Weapon || i is Armor || i is Shield)
                            {
                                equipDetailed.Add(pos.ToInfo(pdf.EquipmentKeywords, pdf.EquipmentStats));
                                equipDetailed2.Add(pos);
                            }
                            else
                            {
                                treasure.Add(pos);
                                treasureDetailed.Add(pos);
                            }
                        }
                        else
                        {
                            treasure.Add(pos);
                            treasureDetailed.Add(pos);
                        }
                        onUse.AddRange(pos.CollectOnUse(level, context.Player, context));
                    }
                }
                equip.Sort(delegate (Possession t1, Possession t2)
                {
                    if (t1.Hightlight && !t2.Hightlight) return -1;
                    else if (t2.Hightlight && !t1.Hightlight) return 1;
                    else
                    {
                        if (!string.Equals(t1.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase) && string.Equals(t2.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) return -1;
                        else if (!string.Equals(t2.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase) && string.Equals(t1.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) return 1;
                        else return (t1.ToString().CompareTo(t2.ToString()));
                    }

                });
                int chigh = 1;
                List<KeyValuePair<string, AttackInfo>> attackinfos = new List<KeyValuePair<string, AttackInfo>>();
                List<KeyValuePair<string, AttackInfo>> addattackinfos = new List<KeyValuePair<string, AttackInfo>>();
                foreach (SpellcastingFeature scf in spellcasts)
                {
                    Spellcasting sc = context.Player.GetSpellcasting(scf.SpellcastingID);
                    foreach (Spell s in sc.GetLearned(context.Player, context))
                    {
                        if (sc.Highlight != null && sc.Highlight != "" && s.Name.ToLowerInvariant() == sc.Highlight.ToLowerInvariant())
                        {
                            AttackInfo ai = context.Player.GetAttack(s, scf.SpellcastingAbility);
                            if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                        }
                        else
                        {
                            AttackInfo ai = context.Player.GetAttack(s, scf.SpellcastingAbility);
                            if (ai != null && ai.Damage != null && ai.Damage != "") addattackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                        }
                    }
                    //foreach (Spell s in sc.GetPrepared(context.Player, context))
                    //{
                    //    if (sc.Highlight != null && sc.Highlight != "" && s.Name.ToLowerInvariant() == sc.Highlight.ToLowerInvariant())
                    //    {
                    //        AttackInfo ai = context.Player.GetAttack(s, scf.SpellcastingAbility);
                    //        if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                    //    }
                    //    else
                    //    {
                    //        AttackInfo ai = context.Player.GetAttack(s, scf.SpellcastingAbility);
                    //        if (ai != null && ai.Damage != null && ai.Damage != "") addattackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                    //    }
                    //}
                    //foreach (Spell s in sc.GetAdditionalClassSpells(context.Player, context))
                    //{
                    //    if (sc.Highlight != null && sc.Highlight != "" && s.Name.ToLowerInvariant() == sc.Highlight.ToLowerInvariant())
                    //    {
                    //        AttackInfo ai = context.Player.GetAttack(s, scf.SpellcastingAbility);
                    //        if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                    //    }
                    //    else
                    //    {
                    //        AttackInfo ai = context.Player.GetAttack(s, scf.SpellcastingAbility);
                    //        if (ai != null && ai.Damage != null && ai.Damage != "") addattackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                    //    }
                    //}
                }
                foreach (Possession pos in equip)
                {
                    IEnumerable<AttackInfo> ais = context.Player.GetAttack(pos);
                    if (ais != null) attackinfos.AddRange(ais.Select(ai => new KeyValuePair<string, AttackInfo>(pos.ToString() + (ai.AttackOptions.Count > 0 ? " (" + string.Join(", ",ai.AttackOptions) + " )" : ""), ai)));
                }
                foreach (ModifiedSpell s in context.Player.GetBonusSpells(false))
                {
                    if (Utils.Matches(context, s, "Attack or Save", null))
                    {
                        AttackInfo ai = context.Player.GetAttack(s, s.differentAbility);
                        if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                        //p.SetField(trans["Attack" + chigh], s.Name);
                        //if (ai.SaveDC != "") p.SetField(trans["Attack" + chigh + "Attack"], "DC " + ai.SaveDC);
                        //else p.SetField(trans["Attack" + chigh + "Attack"], PlusMinus(ai.AttackBonus));
                        //if (trans.ContainsKey("Attack" + chigh + "DamageType"))
                        //{
                        //    p.SetField(trans["Attack" + chigh + "Damage"], ai.Damage);
                        //    p.SetField(trans["Attack" + chigh + "DamageType"], ai.DamageType);
                        //}
                        //else
                        //{
                        //    p.SetField(trans["Attack" + chigh + "Damage"], ai.Damage + " " + ai.DamageType);
                        //}
                        //chigh++;
                    }
                }
                attackinfos.AddRange(addattackinfos);
                //attackinfos.Sort((a, b) =>
                //{
                //    int oa = context.Player.AttackOrder.FindIndex(s => StringComparer.OrdinalIgnoreCase.Equals(a.Key, s));
                //    int ob = context.Player.AttackOrder.FindIndex(s => StringComparer.OrdinalIgnoreCase.Equals(b.Key, s));
                //    if (oa < 0) oa = int.MaxValue;
                //    if (ob < 0) ob = int.MaxValue;
                //    return oa.CompareTo(ob);
                //});

                foreach (KeyValuePair<string, AttackInfo> aip in attackinfos.OrderBy(a => { int i = context.Player.AttackOrder.FindIndex(s => StringComparer.OrdinalIgnoreCase.Equals(a.Key, s)); return i < 0 ? int.MaxValue : i; }))
                {
                    if (trans.ContainsKey("Attack" + chigh))
                    {
                        p.SetField(trans["Attack" + chigh], aip.Key);
                        if (aip.Value.SaveDC != "") p.SetField(trans["Attack" + chigh + "Attack"], "DC " + aip.Value.SaveDC);
                        else p.SetField(trans["Attack" + chigh + "Attack"], PlusMinus(aip.Value.AttackBonus));
                        if (trans.ContainsKey("Attack" + chigh + "DamageType"))
                        {
                            p.SetField(trans["Attack" + chigh + "Damage"], aip.Value.Damage);
                            p.SetField(trans["Attack" + chigh + "DamageType"], aip.Value.DamageType);
                        }
                        else
                        {
                            p.SetField(trans["Attack" + chigh + "Damage"], aip.Value.Damage + " " + aip.Value.DamageType);
                        }
                        chigh++;
                    }
                }
                List<IInfoText> usable = new List<IInfoText>();
                //if (pdf.PreserveEdit || true)
                //{
                if (trans.ContainsKey("RaceBackgroundFeatures"))
                {
                    List<Feature> feats = new List<Feature>();
                    foreach (Feature f in onUse) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f))
                        {
                            if (trans.ContainsKey("Treasure") || trans.ContainsKey("Usable")) usable.Add(f);
                            else if (!pdf.IncludeSpellbook || pdf.ForceAttunedAndOnUseItemsOnSheet) feats.Add(f);
                        }
                    foreach (Feature f in context.Player.GetOnlyBackgroundFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    foreach (Feature f in context.Player.GetBoons()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    if (!pdf.IncludeSpellbook || pdf.ForceAttunedAndOnUseItemsOnSheet) foreach (Feature f in context.Player.GetPossessionFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    foreach (Feature f in context.Player.GetRaceFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    p.SetTextAndDescriptions(trans["RaceBackgroundFeatures"], pdf.OnlyFeatureTitles, null, feats);
                    List<Feature> feats2 = new List<Feature>();
                    foreach (Feature f in context.Player.GetClassFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats2.Add(f);
                    foreach (Feature f in context.Player.GetCommonFeaturesAndFeats()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats2.Add(f);
                    if (trans.ContainsKey("Features")) p.SetTextAndDescriptions(trans["Features"], pdf.OnlyFeatureTitles, null, feats2);
                }
                else if (trans.ContainsKey("Features"))
                {
                    List<Feature> feats = new List<Feature>();
                    foreach (Feature f in onUse) if (!f.Hidden)
                        {
                            if (trans.ContainsKey("Treasure") || trans.ContainsKey("Usable")) usable.Add(f);
                            else if (!pdf.IncludeSpellbook || pdf.ForceAttunedAndOnUseItemsOnSheet) feats.Add(f);
                        }
                    foreach (Feature f in context.Player.GetOnlyBackgroundFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    foreach (Feature f in context.Player.GetBoons()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    if (!pdf.IncludeSpellbook || pdf.ForceAttunedAndOnUseItemsOnSheet) foreach (Feature f in context.Player.GetPossessionFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    foreach (Feature f in context.Player.GetRaceFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    foreach (Feature f in context.Player.GetClassFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    foreach (Feature f in context.Player.GetCommonFeaturesAndFeats()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name) && !hiddenfeats.ContainsKey(f.Name + "/" + f.Level) && !hiddenactions.ContainsKey(f)) feats.Add(f);
                    p.SetTextAndDescriptions(trans["Features"], pdf.OnlyFeatureTitles, null, feats);
                }
                bool addUsableToTreasure = false;
                if (trans.ContainsKey("Usable")) p.SetTextAndDescriptions(trans["Usable"], pdf.OnlyFeatureTitles, null, usable);
                else addUsableToTreasure = usable.Count > 0;
                var banked = context.Player.GetItemsAndPossessions(true, true).Where(pp => pp is JournalPossession jpp && jpp.Banked).ToList();
                if (trans.ContainsKey("Banked"))
                {
                    p.SetField(trans["Banked"], string.Join(",", banked.Select(ppos => ppos.ToInfo(pdf.EquipmentKeywords, pdf.EquipmentStats))));
                }
                if (trans.ContainsKey("EquipmentDetailed")) p.SetTextAndDescriptions(trans["EquipmentDetailed"], false, null, banked);
                if (banked.Count > 0 && !trans.ContainsKey("Banked") && !trans.ContainsKey("BankedDetailed"))
                {
                    equip.AddRange(treasure);
                    equipDetailed2.AddRange(treasureDetailed);
                    equipDetailed.AddRange(treasure.Select(pos => pos.ToInfo(pdf.EquipmentKeywords, pdf.EquipmentStats)));
                    treasure.Clear();
                    treasureDetailed.Clear();
                    treasure.AddRange(banked);
                    treasureDetailed.AddRange(banked);
                    addUsableToTreasure = false;
                }
                bool addMoney = false;
                
                if (trans.ContainsKey("CP") && trans.ContainsKey("GP") && trans.ContainsKey("SP") && trans.ContainsKey("EP") && trans.ContainsKey("PP"))
                {
                    if (trans.ContainsKey("Equipment")) p.SetField(trans["Equipment"], String.Join(pdf.EquipmentKeywords ? ";" : "; ", equipDetailed));
                    if (trans.ContainsKey("EquipmentShort")) p.SetField(trans["EquipmentShort"], String.Join(", ", equip));
                    if (trans.ContainsKey("EquipmentDetailed")) p.SetTextAndDescriptions(trans["EquipmentDetailed"], false, null, equipDetailed2);
                    if (trans.ContainsKey("Treasure")) p.SetField(trans["Treasure"], String.Join(", ", treasure) + (addUsableToTreasure ? "\n" + String.Join(",", usable) : ""));
                    p.SetField(trans["CP"], money.cp.ToString());
                    if (trans.ContainsKey("SP")) p.SetField(trans["SP"], money.sp.ToString());
                    if (trans.ContainsKey("EP")) p.SetField(trans["EP"], money.ep.ToString());
                    if (trans.ContainsKey("GP")) p.SetField(trans["GP"], money.gp.ToString());
                    if (trans.ContainsKey("PP")) p.SetField(trans["PP"], money.pp.ToString());
                }
                else if (trans.ContainsKey("GP"))
                {
                    if (trans.ContainsKey("Equipment")) p.SetField(trans["Equipment"], String.Join(pdf.EquipmentKeywords ? "\n" : "; ", equipDetailed));
                    if (trans.ContainsKey("EquipmentShort")) p.SetField(trans["EquipmentShort"], String.Join(", ", equip));
                    if (trans.ContainsKey("EquipmentDetailed")) p.SetTextAndDescriptions(trans["EquipmentDetailed"], false, null, equipDetailed2);
                    if (trans.ContainsKey("Treasure"))
                    {
                        if (addUsableToTreasure) p.SetTextAndDescriptions(trans["Treasure"], false, String.Join(", ", treasure), usable);
                        else p.SetField(trans["Treasure"], String.Join(", ", treasure) + (addUsableToTreasure ? "\n" + String.Join(",", usable) : ""));
                    }
                    p.SetField(trans["GP"], money.ToGold());
                }
                else if (trans.ContainsKey("EquipmentShort"))
                {
                    p.SetField(trans["EquipmentShort"], String.Join(", ", equip) + "\n" + money.ToString());
                    if (trans.ContainsKey("EquipmentDetailed")) p.SetTextAndDescriptions(trans["EquipmentDetailed"], false, null, equipDetailed2);
                    if (trans.ContainsKey("Equipment")) p.SetField(trans["Equipment"], String.Join(pdf.EquipmentKeywords ? "\n" : "; ", equipDetailed));
                    if (trans.ContainsKey("Treasure"))
                    {
                        if (addUsableToTreasure) p.SetTextAndDescriptions(trans["Treasure"], false, String.Join(", ", treasure), usable);
                        else p.SetField(trans["Treasure"], String.Join(", ", treasure));
                    }
                }
                else if (trans.ContainsKey("Equipment"))
                {
                    p.SetField(trans["Equipment"], String.Join(pdf.EquipmentKeywords ? "\n" : "; ", equipDetailed) + "\n" + money.ToString());
                    if (trans.ContainsKey("EquipmentDetailed")) p.SetTextAndDescriptions(trans["EquipmentDetailed"], false, null, equipDetailed2);
                    if (trans.ContainsKey("Treasure"))
                    {
                        if (addUsableToTreasure) p.SetTextAndDescriptions(trans["Treasure"], false, String.Join(", ", treasure), usable);
                        else p.SetField(trans["Treasure"], String.Join(", ", treasure));
                    }
                }
                else if (trans.ContainsKey("EquipmentDetailed"))
                {
                    p.SetTextAndDescriptions(trans["EquipmentDetailed"], false, null, equipDetailed2, money.ToString());
                    if (trans.ContainsKey("Treasure"))
                    {
                        if (addUsableToTreasure) p.SetTextAndDescriptions(trans["Treasure"], false, String.Join(", ", treasure), usable);
                        else p.SetField(trans["Treasure"], String.Join(", ", treasure));
                    }
                }
                else if (trans.ContainsKey("Treasure"))
                {
                    if (addUsableToTreasure) p.SetTextAndDescriptions(trans["Treasure"], false, String.Join(", ", treasure), usable, money.ToString());
                    else p.SetField(trans["Treasure"], String.Join(", ", treasure) + "\n" + money.ToString());
                }
                else
                {
                    addMoney = true;
                }
                using (IPDFSheet sheet = pdf.CreateSheet())
                {
                    sheet.Add(p);
                    await sheet.AddBlankPages(null);
                    int sheetc = 0;
                    if (SpellFile != null && SpellFile != "" && spelltrans.ContainsKey("Spell1-1"))
                    {
                        foreach (SpellcastingFeature scf in spellcasts)
                        {
                            if (scf.SpellcastingID != "MULTICLASS")
                            {
                                Spellcasting sc = context.Player.GetSpellcasting(scf.SpellcastingID);
                                int classlevel = context.Player.GetClassLevel(scf.SpellcastingID);
                                List<int> SpellSlots = context.Player.GetSpellSlots(scf.SpellcastingID);
                                List<int> UsedSpellSlots = context.Player.GetUsedSpellSlots(scf.SpellcastingID);
                                List<Spell> Available = new List<Spell>();
                                List<Spell> Prepared = new List<Spell>();
                                if (scf.Preparation == PreparationMode.ClassList)
                                {
                                    Available.AddRange(sc.GetAdditionalClassSpells(context.Player, context));
                                    Available.AddRange(Utils.FilterSpell(context, scf.PrepareableSpells, scf.SpellcastingID, classlevel));
                                    Prepared.AddRange(sc.GetPrepared(context.Player, context));
                                    Available.RemoveAll(s => Prepared.Exists(ss => StringComparer.OrdinalIgnoreCase.Equals(s.Name, ss.Name)));
                                }
                                else if (scf.Preparation == PreparationMode.Spellbook)
                                {
                                    Available.AddRange(sc.GetSpellbook(context.Player, context));
                                    Prepared.AddRange(sc.GetPrepared(context.Player, context));
                                    Available.RemoveAll(s => Prepared.Exists(ss => StringComparer.OrdinalIgnoreCase.Equals(s.Name, ss.Name)));
                                }
                                else
                                {
                                    Prepared.AddRange(sc.GetPrepared(context.Player, context));
                                }
                                Prepared.AddRange(sc.GetLearned(context.Player, context));
                                Available.AddRange(Prepared);
                                spellbook.AddRange(Available);
                                List<Spell> Shown = new List<Spell>(Available.Distinct());
                                Shown.Sort(delegate (Spell t1, Spell t2)
                                {
                                    bool t1p = Prepared.Contains(t1);
                                    bool t2p = Prepared.Contains(t2);
                                    if (t1p && t2p) return (t1.Name.CompareTo(t2.Name));
                                    else if (t1p) return -1;
                                    else if (t2p) return 1;
                                    else return (t1.Name.CompareTo(t2.Name));

                                });
                                List<LinkedList<Spell>> SpellLevels = new List<LinkedList<Spell>>();
                                foreach (Spell s in Shown)
                                {
                                    while (SpellLevels.Count <= s.Level) SpellLevels.Add(new LinkedList<Spell>());
                                    SpellLevels[s.Level].AddLast(s);
                                }
                                while (SpellLevels.Count <= SpellSlots.Count) SpellLevels.Add(new LinkedList<Spell>());


                                int sheetmaxlevel = 0;
                                for (sheetmaxlevel = 1; sheetmaxlevel < SpellLevels.Count; sheetmaxlevel++) if (!spelltrans.ContainsKey("Spell" + sheetmaxlevel + "-1")) break;
                                sheetmaxlevel--;
                                int offset = 0;

                                while (SpellLevels.Count > 0 && sheetmaxlevel > 0)
                                {
                                    using (IPDFEditor sp = await pdf.CreateEditor(SpellFile))
                                    {
                                        FillBasicFields(spelltrans, sp, context, pdf);
                                        if (spelltrans.ContainsKey("SpellcastingClass")) sp.SetField(spelltrans["SpellcastingClass"], scf.DisplayName);
                                        if (spelltrans.ContainsKey("SpellcastingAbility")) sp.SetField(spelltrans["SpellcastingAbility"], Enum.GetName(typeof(Ability), scf.SpellcastingAbility));
                                        if (spelltrans.ContainsKey("SpellSaveDC")) sp.SetField(spelltrans["SpellSaveDC"], context.Player.GetSpellSaveDC(scf.SpellcastingID, scf.SpellcastingAbility).ToString());
                                        if (spelltrans.ContainsKey("SpellAttackBonus")) sp.SetField(spelltrans["SpellAttackBonus"], PlusMinus(context.Player.GetSpellAttack(scf.SpellcastingID, scf.SpellcastingAbility)));
                                        for (int i = 0; i <= sheetmaxlevel && i < SpellLevels.Count; i++)
                                        {
                                            if (SpellSlots.Count >= i && i > 0 && SpellSlots[i - 1] > 0)
                                            {
                                                if (spelltrans.ContainsKey("SpellSlots" + i))
                                                {
                                                    sp.SetField(spelltrans["SpellSlots" + i], (offset > 0 ? "(" + (offset + i) + ") " : "") + SpellSlots[i - 1].ToString());
                                                }
                                            }
                                            if (pdf.IncludeResources && UsedSpellSlots.Count >= i && i > 0)
                                            {
                                                if (spelltrans.ContainsKey("SpellSlotsExpended" + i))
                                                {
                                                    sp.SetField(spelltrans["SpellSlotsExpended" + i], UsedSpellSlots[i - 1].ToString());
                                                }
                                            }
                                            int field = 1;
                                            if (!spelltrans.ContainsKey("Spell" + i + "-1")) SpellLevels[i].Clear();
                                            while (SpellLevels[i].Count > 0)
                                            {
                                                if (!spelltrans.ContainsKey("Spell" + i + "-" + field)) break;
                                                sp.SetField(spelltrans["Spell" + i + "-" + field], SpellLevels[i].First.Value.Name);
                                                if (Prepared.Contains(SpellLevels[i].First.Value) && spelltrans.ContainsKey("Prepared" + i + "-" + field))
                                                    sp.SetField(spelltrans["Prepared" + i + "-" + field], "Yes");
                                                SpellLevels[i].RemoveFirst();
                                                field++;
                                            }
                                        }
                                        sheetc++;
                                        sheet.Add(sp);
                                    }
                                    bool empty = true;
                                    for (int i = 0; i <= sheetmaxlevel && i < SpellLevels.Count; i++) if (SpellLevels[i].Count > 0) empty = false;
                                    if (empty)
                                    {
                                        SpellLevels.RemoveRange(1, sheetmaxlevel);
                                        if (SpellLevels.Count == 1) SpellLevels.Clear(); //Cantrips
                                        offset += sheetmaxlevel;
                                    }
                                }
                            }

                        }
                        if (pdf.Duplex && !pdf.DuplexWhite && sheetc % 2 != 0)
                        {
                            using (IPDFEditor sp = await pdf.CreateEditor(SpellFile))
                            {
                                FillBasicFields(spelltrans, sp, context, pdf);
                                sheet.Add(sp);
                            }
                        }
                        await sheet.AddBlankPages(SpellFile);
                    }
                    
                    if (pdf.IncludeLog && LogFile != null && LogFile != "" && (logtrans.ContainsKey("Title1") || logtrans.ContainsKey("XP1")))
                    {
                        Queue<JournalEntry> entries = new Queue<JournalEntry>(context.Player.ComplexJournal);



                        Price gold = context.Player.GetMoney(false);
                        int xp = context.Player.XP;
                        int ap = context.Player.AP;
                        int renown = 0;
                        int downtime = 0;
                        int magic = 0;
                        int t1tp = 0;
                        int t2tp = 0;
                        int t3tp = 0;
                        int t4tp = 0;
                        int sheetCount = 0;
                        bool advancement = context.Player.Advancement;
                        bool hasPossesionsInJournal = context.Player.GetItemsAndPossessions(true, true).Count > 0;
                        int common = 0;
                        int uncommon = 0;
                        int consumables = 0;
                        Dictionary<Guid, JournalPossession> journalPossesions = new Dictionary<Guid, JournalPossession>();
                        Dictionary<Guid, JournalBoon> journalBoons = new Dictionary<Guid, JournalBoon>();
                        while (entries.Count > 0 || (pdf.Duplex && !pdf.DuplexWhite && sheetCount % 2 != 0))
                        {
                            int counter = 1;
                            using (IPDFEditor lp = await pdf.CreateEditor(LogFile))
                            {
                                sheetCount++;
                                FillBasicFields(logtrans, lp, context, pdf);
                                if (logtrans.ContainsKey("Sheet")) lp.SetField(logtrans["Sheet"], sheetCount.ToString());
                                while (entries.Count > 0 && (logtrans.ContainsKey("Title" + counter) || logtrans.ContainsKey("XP" + counter) || logtrans.ContainsKey("AP" + counter) || logtrans.ContainsKey("APXP" + counter)))
                                {
                                    JournalEntry entry = entries.Dequeue();
                                    //if (advancement && xp > 0)
                                    //{
                                    //    ap = context.Levels.ToAP(context.Levels.ToXP(ap) + xp);
                                    //    xp = 0;
                                    //} else if (!advancement && ap > 0)
                                    //{
                                    //    xp = context.Levels.ToXP(context.Levels.ToAP(xp) + ap);
                                    //    ap = 0;
                                    //}
                                    if (advancement && xp > 0 && entry.AP != 0)
                                    {
                                        ap = context.Levels.ToAP(context.Levels.ToXP(ap) + xp);
                                        xp = 0;
                                    }
                                    else if (!advancement && ap > 0 && entry.XP != 0)
                                    {
                                        xp = context.Levels.ToXP(context.Levels.ToAP(xp) + ap);
                                        ap = 0;
                                    }
                                    List<string> tp = new List<string>();
                                    List<string> tpm = new List<string>();
                                    List<string> tpt = new List<string>();
                                    List<string> tpv = new List<string>();
                                    List<string> tpmv = new List<string>();
                                    if (entry.InSheet)
                                    {
                                        if (logtrans.ContainsKey("Title" + counter)) lp.SetField(logtrans["Title" + counter], entry.Title);
                                        if (logtrans.ContainsKey("Session" + counter)) lp.SetField(logtrans["Session" + counter], entry.Session);
                                        if (logtrans.ContainsKey("Date" + counter)) lp.SetField(logtrans["Date" + counter], entry.Added.ToString());
                                        if (logtrans.ContainsKey("DM" + counter)) lp.SetField(logtrans["DM" + counter], entry.DM);
                                        if (logtrans.ContainsKey("LevelStart" + counter)) lp.SetField(logtrans["LevelStart" + counter], context.Levels.Get(advancement ? (xp > 0 ? context.Levels.ToAP(context.Levels.ToXP(ap) + xp): ap) : (ap > 0 ? context.Levels.ToXP(context.Levels.ToAP(xp) + ap) : xp), advancement).ToString());
                                        if (logtrans.ContainsKey("XPStart" + counter)) lp.SetField(logtrans["XPStart" + counter], advancement ? (context.Levels.ToXP(ap) + xp).ToString() : xp.ToString());
                                        if (logtrans.ContainsKey("APStart" + counter)) lp.SetField(logtrans["APStart" + counter], advancement ? APFormat(context, pdf.APFormat, (xp > 0 ? context.Levels.ToAP(context.Levels.ToXP(ap) + xp) : ap)) : APFormat(context, pdf.APFormat, context.Levels.ToAP(xp) + ap));
                                        if (logtrans.ContainsKey("APXPStart" + counter)) lp.SetField(logtrans["APXPStart" + counter], (entry.AP != 0 ? (advancement ? APFormat(context, pdf.APFormat, ap) : APFormat(context, pdf.APFormat, context.Levels.ToAP(xp) + ap)) : "") + (entry.AP != 0 && entry.XP != 0 ? ", " : "") + (entry.XP != 0 ? (advancement ? (context.Levels.ToXP(ap) + xp).ToString() : xp.ToString()) : ""));
                                        if (logtrans.ContainsKey("APXPStartText" + counter)) lp.SetField(logtrans["APXPStartText" + counter], "Starting " + (entry.AP != 0 ? (entry.XP != 0 ? "ACP/XP" : "ACP") : (entry.XP != 0 ? "XP" : (advancement ? "ACP" : "XP"))));
                                        if (logtrans.ContainsKey("GoldStart" + counter)) lp.SetField(logtrans["GoldStart" + counter], gold.ToGold());
                                        if (logtrans.ContainsKey("DowntimeStart" + counter)) lp.SetField(logtrans["DowntimeStart" + counter], downtime.ToString());
                                        if (logtrans.ContainsKey("RenownStart" + counter)) lp.SetField(logtrans["RenownStart" + counter], renown.ToString());
                                        if (logtrans.ContainsKey("MagicItemsStart" + counter)) lp.SetField(logtrans["MagicItemsStart" + counter], hasPossesionsInJournal ? ToString(uncommon, common, consumables) : magic.ToString());
                                        List<string> tps = new List<string>();
                                        if (t1tp != 0) tps.Add(t1tp.ToString() + " T1");
                                        if (t2tp != 0) tps.Add(t2tp.ToString() + " T2");
                                        if (t3tp != 0) tps.Add(t3tp.ToString() + " T3");
                                        if (t4tp != 0) tps.Add(t4tp.ToString() + " T4");
                                        List<string> tpst = new List<string>();
                                        List<string> tpsv = new List<string>();
                                        if (entry.T1TP != 0)
                                        {
                                            tpst.Add("Tier 1");
                                            tpsv.Add(t1tp.ToString());
                                        }
                                        if (entry.T2TP != 0)
                                        {
                                            tpst.Add("Tier 2");
                                            tpsv.Add(t2tp.ToString());
                                        }
                                        if (entry.T3TP != 0)
                                        {
                                            tpst.Add("Tier 3");
                                            tpsv.Add(t3tp.ToString());
                                        }
                                        if (entry.T4TP != 0)
                                        {
                                            tpst.Add("Tier 4");
                                            tpsv.Add(t4tp.ToString());
                                        }
                                        List<string> tpsm = new List<string>();
                                        if (magic != 0) tpsm.Add(magic.ToString() + (tps.Count > 0 ? " Magic Items" : ""));
                                        if (t1tp != 0) tpsm.Add(t1tp.ToString() + " T1");
                                        if (t2tp != 0) tpsm.Add(t2tp.ToString() + " T2");
                                        if (t3tp != 0) tpsm.Add(t3tp.ToString() + " T3");
                                        if (t4tp != 0) tpsm.Add(t4tp.ToString() + " T4");
                                        if (uncommon > 0 || common > 0 || consumables > 0) tpsm.Add(ToString(uncommon, common, consumables));
                                        if (logtrans.ContainsKey("MagicItemTreasurePointsStart" + counter)) lp.SetField(logtrans["MagicItemTreasurePointsStart" + counter], String.Join(", ", tpsm));
                                        if (logtrans.ContainsKey("MagicItemTreasurePointsStartText" + counter)) lp.SetField(logtrans["MagicItemTreasurePointsStartText" + counter], (magic > 0 ? (tps.Count > 0 ? "Starting Magic Items / TP" : "Starting # of Magic Items") : (tps.Count > 0 || advancement ? "Starting Treasure Points" : "Starting # of Magic Items")));

                                        if (logtrans.ContainsKey("TreasurePointsStart" + counter)) lp.SetField(logtrans["TreasurePointsStart" + counter], String.Join(", ", tps));
                                        if (logtrans.ContainsKey("TreasurePointsStartText" + counter)) lp.SetField(logtrans["TreasurePointsStartText" + counter], String.Join(", ", tpst));
                                        if (logtrans.ContainsKey("TreasurePointsStartValue" + counter)) lp.SetField(logtrans["TreasurePointsStartValue" + counter], String.Join(", ", tpsv));
                                        if (logtrans.ContainsKey("TreasurePointsStartTextLong" + counter)) lp.SetField(logtrans["TreasurePointsStartTextLong" + counter], "Treasure Points: " + String.Join(", ", tpst));
                                        if (logtrans.ContainsKey("Tier1TreasurePointsStart" + counter)) lp.SetField(logtrans["Tier1TreasurePointsStart" + counter], t1tp.ToString());
                                        if (logtrans.ContainsKey("Tier2TreasurePointsStart" + counter)) lp.SetField(logtrans["Tier2TreasurePointsStart" + counter], t2tp.ToString());
                                        if (logtrans.ContainsKey("Tier3TreasurePointsStart" + counter)) lp.SetField(logtrans["Tier3TreasurePointsStart" + counter], t3tp.ToString());
                                        if (logtrans.ContainsKey("Tier4TreasurePointsStart" + counter)) lp.SetField(logtrans["Tier4TreasurePointsStart" + counter], t4tp.ToString());



                                        if (entry.T1TP != 0)
                                        {
                                            tp.Add(PlusMinus(entry.T1TP, "--") + " T1");
                                            tpt.Add("Tier 1");
                                            tpv.Add(PlusMinus(entry.T1TP, "--"));
                                        }
                                        if (entry.T2TP != 0)
                                        {
                                            tp.Add(PlusMinus(entry.T2TP, "--") + " T2");
                                            tpt.Add("Tier 2");
                                            tpv.Add(PlusMinus(entry.T2TP, "--"));
                                        }
                                        if (entry.T3TP != 0)
                                        {
                                            tp.Add(PlusMinus(entry.T3TP, "--") + " T3");
                                            tpt.Add("Tier 3");
                                            tpv.Add(PlusMinus(entry.T3TP, "--"));
                                        }
                                        if (entry.T4TP != 0)
                                        {
                                            tp.Add(PlusMinus(entry.T4TP, "--") + " T4");
                                            tpt.Add("Tier 4");
                                            tpv.Add(PlusMinus(entry.T4TP, "--"));
                                        }
                                        if (entry.MagicItems != 0)
                                        {
                                            tpmv.Add(PlusMinus(entry.MagicItems, "--"));
                                            tpm.Add(PlusMinus(entry.MagicItems, "--") + " Magic Items");

                                        }
                                        tpmv.AddRange(tpv);
                                        tpm.AddRange(tp);

                                        if (logtrans.ContainsKey("XP" + counter)) lp.SetField(logtrans["XP" + counter], entry.XP.ToString());
                                        if (logtrans.ContainsKey("AP" + counter)) lp.SetField(logtrans["AP" + counter], entry.AP.ToString());
                                        if (logtrans.ContainsKey("APXP" + counter)) lp.SetField(logtrans["APXP" + counter], (entry.AP != 0 ? entry.AP.ToString() : "") + (entry.AP != 0 && entry.XP != 0 ? ", " : "") + (entry.XP != 0 ? entry.XP.ToString() : "") + ((entry.AP != 0 || entry.XP != 0) && entry.Milestone ? ", " : "") + (entry.Milestone ? "Milestone" : ""));
                                        if (logtrans.ContainsKey("APXPText" + counter)) lp.SetField(logtrans["APXPText" + counter], (entry.AP != 0 ? (entry.XP != 0 ? "ACP/XP" : "ACP") : (entry.XP != 0 ? "XP" : (entry.Milestone ? "Milestone" : (advancement ? "ACP" : "XP")))) + " +/-");
                                        if (logtrans.ContainsKey("Gold" + counter)) lp.SetField(logtrans["Gold" + counter], entry.GetMoney());
                                        if (logtrans.ContainsKey("Downtime" + counter)) lp.SetField(logtrans["Downtime" + counter], PlusMinus(entry.Downtime, "--"));
                                        if (logtrans.ContainsKey("Renown" + counter)) lp.SetField(logtrans["Renown" + counter], PlusMinus(entry.Renown, "--"));
                                        if (logtrans.ContainsKey("MagicItems" + counter)) lp.SetField(logtrans["MagicItems" + counter], PlusMinus(entry.MagicItems, "--"));
                                        if (logtrans.ContainsKey("Tier1TreasurePoints" + counter)) lp.SetField(logtrans["Tier1TreasurePoints" + counter], PlusMinus(entry.T1TP, "--"));
                                        if (logtrans.ContainsKey("Tier2TreasurePoints" + counter)) lp.SetField(logtrans["Tier2TreasurePoints" + counter], PlusMinus(entry.T2TP, "--"));
                                        if (logtrans.ContainsKey("Tier3TreasurePoints" + counter)) lp.SetField(logtrans["Tier3TreasurePoints" + counter], PlusMinus(entry.T3TP, "--"));
                                        if (logtrans.ContainsKey("Tier4TreasurePoints" + counter)) lp.SetField(logtrans["Tier4TreasurePoints" + counter], PlusMinus(entry.T4TP, "--"));
                                        if (logtrans.ContainsKey("TreasurePoints" + counter)) lp.SetField(logtrans["TreasurePoints" + counter], String.Join(", ", tp));
                                        if (logtrans.ContainsKey("TreasurePointsText" + counter)) lp.SetField(logtrans["TreasurePointsText" + counter], String.Join(", ", tpt));
                                        if (logtrans.ContainsKey("TreasurePointsTextLong" + counter)) lp.SetField(logtrans["TreasurePointsTextLong" + counter], String.Join(", ", tpt) + " Treasure Points +/-");
                                        if (logtrans.ContainsKey("TreasurePointsValue" + counter)) lp.SetField(logtrans["TreasurePointsValue" + counter], String.Join(", ", tpv));
                                        if (entry.Milestone && logtrans.ContainsKey("Milestone" + counter)) lp.SetField(logtrans["Milestone" + counter], "Yes");
                                    }
                                    xp += entry.XP;
                                    ap += entry.AP;
                                    if (entry.Milestone)
                                    {
                                        if (advancement)
                                        {
                                            if (xp > 0)
                                            {
                                                ap = context.Levels.ToAP(context.Levels.ToXP(ap) + xp);
                                                xp = 0;
                                            }
                                            ap += context.Levels.XpToLevelUp(ap, true);
                                        } else
                                        {
                                            if (ap > 0)
                                            {
                                                xp = context.Levels.ToAP(context.Levels.ToAP(xp) + ap);
                                                ap = 0;
                                            }
                                            xp += context.Levels.XpToLevelUp(xp, false);
                                        }
                                    }
                                    gold.pp += entry.PP;
                                    gold.gp += entry.GP;
                                    gold.sp += entry.SP;
                                    gold.ep += entry.EP;
                                    gold.cp += entry.CP;
                                    renown += entry.Renown;
                                    if (!pdf.IgnoreMagicItems)
                                    {
                                        magic += entry.MagicItems;
                                    }
                                    downtime += entry.Downtime;
                                    t1tp += entry.T1TP;
                                    t2tp += entry.T2TP;
                                    t3tp += entry.T3TP;
                                    t4tp += entry.T4TP;
                                    List<string> addedItems = new List<string>();
                                    List<string> removedItems = new List<string>();
                                    List<string> touchedItems = new List<string>();
                                    foreach (JournalPossession jp in entry.Possessions)
                                    {
                                        jp.Context = context;
                                        if (jp.Deleted)
                                        {
                                            journalPossesions.Remove(jp.Guid);
                                            removedItems.Add(jp.FullName + (jp.Banked ? " [banked]" : ""));
                                        }
                                        else if (journalPossesions.ContainsKey(jp.Guid))
                                        {
                                            journalPossesions[jp.Guid] = jp;
                                            touchedItems.Add(jp.FullName + (jp.Banked ? " [banked]" : ""));
                                        }
                                        else
                                        {
                                            journalPossesions.Add(jp.Guid, jp);
                                            addedItems.Add(jp.FullName + (jp.Banked ? " [banked]" : ""));
                                        }
                                    }
                                    foreach (JournalBoon jb in entry.Boons)
                                    {
                                        if (jb.Deleted)
                                        {
                                            journalBoons.Remove(jb.Guid);
                                            removedItems.Add(jb.ToString() + (jb.Banked ? " [banked]" : ""));
                                        }
                                        else if (journalBoons.ContainsKey(jb.Guid))
                                        {
                                            journalBoons[jb.Guid] = jb;
                                            addedItems.Add(jb.ToString() + (jb.Banked ? " [banked]" : ""));
                                        }
                                        else
                                        {
                                            journalBoons.Add(jb.Guid, jb);
                                            touchedItems.Add(jb.ToString() + (jb.Banked ? " [banked]" : ""));
                                        }
                                    }
                                    int newcommon = journalPossesions.Values.Where(x => !x.Banked && !x.Consumable && x.Rarity == Rarity.Common).Count();
                                    int newconsumable = journalPossesions.Values.Where(x => !x.Banked && x.Consumable).Count();
                                    int newuncommon = journalPossesions.Values.Where(x => !x.Banked && !x.Consumable && x.Rarity >= Rarity.Uncommon).Count();
                                    if (common != newcommon || consumables != newconsumable || uncommon != newuncommon)
                                    {
                                        tpmv.Add(ToString(newuncommon - uncommon, newcommon - common, newconsumable - consumables, true));
                                        tpm.Add(ToString(newuncommon - uncommon, newcommon - common, newconsumable - consumables, true));
                                    }
                                    string text = entry.Text;
                                    if (entry.Notes.Count > 0)
                                    {
                                        text = string.Join(", ", entry.Notes.Select(s=>s.IndexOf('\n') > 0 ? s.Substring(0, s.IndexOf('\n')) : s)) + (text != null ? "\n" + text : "");
                                    }
                                    if (removedItems.Count > 0 || addedItems.Count > 0 || touchedItems.Count > 0)
                                    {
                                        text = string.Join(", ", addedItems.Select(s => "+" + s).Union(removedItems.Select(s => "-" + s)).Union(touchedItems)) + (text != null ? "\n" + text : "");
                                    }

                                    if (entry.InSheet)
                                    {
                                        if (text != null)
                                        {
                                            if (logtrans.ContainsKey("Notes" + counter)) lp.SetField(logtrans["Notes" + counter], text);
                                            else if (logtrans.ContainsKey("Notes" + counter + "Line1"))
                                            {
                                                int line = 1;
                                                Queue<string> lines = new Queue<string>(text.Split('\n'));
                                                while (lines.Count > 0 && logtrans.ContainsKey("Notes" + counter + "Line" + (line + 1)))
                                                {
                                                    lp.SetField(logtrans["Notes" + counter + "Line" + line], lines.Dequeue());
                                                    line++;
                                                }
                                                lp.SetField(logtrans["Notes" + counter + "Line" + line], string.Join(" ", lines));
                                            }
                                        }
                                        if (logtrans.ContainsKey("MagicItemTreasurePoints" + counter)) lp.SetField(logtrans["MagicItemTreasurePoints" + counter], String.Join(", ", tpm));
                                        if (logtrans.ContainsKey("MagicItemTreasurePointsText" + counter)) lp.SetField(logtrans["MagicItemTreasurePointsText" + counter], (magic > 0 ? (tp.Count > 0 ? "Magic Items, TP +/-" : "Magic Items +/-") : (tp.Count > 0 || advancement ? "Treasure Points +/-" : "Magic Items +/-")));
                                        if (logtrans.ContainsKey("MagicItemTreasurePointsValue" + counter)) lp.SetField(logtrans["MagicItemTreasurePointsValue" + counter], String.Join(", ", tpmv));
                                        if (logtrans.ContainsKey("MagicItemTreasurePoints" + counter)) lp.SetField(logtrans["MagicItemTreasurePoints" + counter], String.Join(", ", tpm));
                                        if (logtrans.ContainsKey("LevelEnd" + counter)) lp.SetField(logtrans["LevelEnd" + counter], context.Levels.Get(advancement ? (xp > 0 ? context.Levels.ToAP(context.Levels.ToXP(ap) + xp) : ap) : (ap > 0 ? context.Levels.ToXP(context.Levels.ToAP(xp) + ap) : xp), advancement).ToString());
                                        if (logtrans.ContainsKey("XPEnd" + counter)) lp.SetField(logtrans["XPEnd" + counter], advancement ? (context.Levels.ToXP(ap) + xp).ToString() : xp.ToString());
                                        if (logtrans.ContainsKey("APEnd" + counter)) lp.SetField(logtrans["APEnd" + counter], advancement ? APFormat(context, pdf.APFormat, (xp > 0 ? context.Levels.ToAP(context.Levels.ToXP(ap) + xp) : ap)) : APFormat(context, pdf.APFormat, context.Levels.ToAP(xp) + ap));
                                        if (logtrans.ContainsKey("APXPEnd" + counter)) lp.SetField(logtrans["APXPEnd" + counter], (entry.AP != 0 ? (advancement ? APFormat(context, pdf.APFormat, ap) : APFormat(context, pdf.APFormat, context.Levels.ToAP(xp) + ap)) : "") + (entry.AP != 0 && entry.XP != 0 ? ", " : "") + (entry.XP != 0 ? (advancement ? (context.Levels.ToXP(ap) + xp).ToString() : xp.ToString()) : ""));
                                        if (logtrans.ContainsKey("APXPEndText" + counter)) lp.SetField(logtrans["APXPEndText" + counter], (entry.AP != 0 ? (entry.XP != 0 ? "ACP/XP" : "ACP") : (entry.XP != 0 ? "XP" : (advancement ? "ACP" : "XP"))) + " Total");
                                        if (logtrans.ContainsKey("GoldEnd" + counter)) lp.SetField(logtrans["GoldEnd" + counter], gold.ToGold());
                                        if (logtrans.ContainsKey("DowntimeEnd" + counter)) lp.SetField(logtrans["DowntimeEnd" + counter], downtime.ToString());
                                        if (logtrans.ContainsKey("RenownEnd" + counter)) lp.SetField(logtrans["RenownEnd" + counter], renown.ToString());
                                        if (logtrans.ContainsKey("MagicItemsEnd" + counter)) lp.SetField(logtrans["MagicItemsEnd" + counter], hasPossesionsInJournal ? ToString(uncommon, common, consumables) : magic.ToString());
                                        if (logtrans.ContainsKey("Tier1TreasurePointsEnd" + counter)) lp.SetField(logtrans["Tier1TreasurePointsEnd" + counter], t1tp.ToString());
                                        if (logtrans.ContainsKey("Tier2TreasurePointsEnd" + counter)) lp.SetField(logtrans["Tier2TreasurePointsEnd" + counter], t2tp.ToString());
                                        if (logtrans.ContainsKey("Tier3TreasurePointsEnd" + counter)) lp.SetField(logtrans["Tier3TreasurePointsEnd" + counter], t3tp.ToString());
                                        if (logtrans.ContainsKey("Tier4TreasurePointsEnd" + counter)) lp.SetField(logtrans["Tier4TreasurePointsEnd" + counter], t4tp.ToString());
                                        List<string> tps = new List<string>();
                                        if (t1tp != 0) tps.Add(t1tp.ToString() + " T1");
                                        if (t2tp != 0) tps.Add(t2tp.ToString() + " T2");
                                        if (t3tp != 0) tps.Add(t3tp.ToString() + " T3");
                                        if (t4tp != 0) tps.Add(t4tp.ToString() + " T4");
                                        List<string> tpst = new List<string>();
                                        List<string> tpsv = new List<string>();
                                        if (entry.T1TP != 0)
                                        {
                                            tpst.Add("Tier 1");
                                            tpsv.Add(t1tp.ToString());
                                        }
                                        if (entry.T2TP != 0)
                                        {
                                            tpst.Add("Tier 2");
                                            tpsv.Add(t2tp.ToString());
                                        }
                                        if (entry.T3TP != 0)
                                        {
                                            tpst.Add("Tier 3");
                                            tpsv.Add(t3tp.ToString());
                                        }
                                        if (entry.T4TP != 0)
                                        {
                                            tpst.Add("Tier 4");
                                            tpsv.Add(t4tp.ToString());
                                        }
                                        if (logtrans.ContainsKey("TreasurePointsEnd" + counter)) lp.SetField(logtrans["TreasurePointsEnd" + counter], String.Join(", ", tps));
                                        if (logtrans.ContainsKey("TreasurePointsEndText" + counter)) lp.SetField(logtrans["TreasurePointsEndText" + counter], String.Join(", ", tpst) + "Total");
                                        if (logtrans.ContainsKey("TreasurePointsEndValue" + counter)) lp.SetField(logtrans["TreasurePointsEndValue" + counter], String.Join(", ", tpsv));
                                        if (logtrans.ContainsKey("TreasurePointsEndTextLong" + counter)) lp.SetField(logtrans["TreasurePointsEndTextLong" + counter], "Treasure Points: " + String.Join(", ", tpst) + " Total");
                                        List<string> tpsm = new List<string>();
                                        if (magic != 0) tpsm.Add(magic.ToString() + (tps.Count > 0 ? " Magic Items" : ""));
                                        if (t1tp != 0) tpsm.Add(t1tp.ToString() + " T1");
                                        if (t2tp != 0) tpsm.Add(t2tp.ToString() + " T2");
                                        if (t3tp != 0) tpsm.Add(t3tp.ToString() + " T3");
                                        if (t4tp != 0) tpsm.Add(t4tp.ToString() + " T4");
                                        if (newuncommon > 0 || newcommon > 0 || newconsumable > 0) tpsm.Add(ToString(newuncommon, newcommon, newconsumable));
                                        if (logtrans.ContainsKey("MagicItemTreasurePointsEnd" + counter)) lp.SetField(logtrans["MagicItemTreasurePointsEnd" + counter], String.Join(", ", tpsm));
                                        if (logtrans.ContainsKey("MagicItemTreasurePointsEndText" + counter)) lp.SetField(logtrans["MagicItemTreasurePointsEndText" + counter], (magic > 0 ? (tps.Count > 0 ? "Magic Items / TP Total" : "Magic Items Total") : (tps.Count > 0 || advancement ? "Treasure Points Total" : "Magic Items Total")));

                                        counter++;
                                    }
                                    common = newcommon;
                                    consumables = newconsumable;
                                    uncommon = newuncommon;


                                }
                                if (counter > 1 || (pdf.Duplex && !pdf.DuplexWhite && sheetCount % 2 != 1)) sheet.Add(lp);
                            }
                        }
                        await sheet.AddBlankPages(LogFile);
                    }
                    
                    if (pdf.IncludeSpellbook && SpellbookFile != null && SpellbookFile != "" && (booktrans.ContainsKey("Name1") || booktrans.ContainsKey("Description1")))
                    {
                        List<SpellModifyFeature> mods = (from f in context.Player.GetFeatures() where f is SpellModifyFeature select f as SpellModifyFeature).ToList();
                        spellbook.AddRange(context.Player.GetSpellscrolls());
                        Queue<object> entries = new Queue<object>(spellbook.OrderBy(s => s.Name).Distinct(new SpellEqualityComparer()));
                        foreach (Possession pos in context.Player.GetItemsAndPossessions())
                        {
                            if (((pos.Description != null && pos.Description != "") && (pdf.ForceAttunedItemsInSpellbook || !pdf.ForceAttunedAndOnUseItemsOnSheet || pdf.MundaneEquipmentInSpellbook))
                                || (pos.MagicProperties.Count > 0 && (pdf.ForceAttunedItemsInSpellbook || !pdf.ForceAttunedAndOnUseItemsOnSheet))
                                || (pos.MagicProperties.Count == 0 && pdf.MundaneEquipmentInSpellbook && (pos.Item is Item i && !i.autogenerated && ((i is Weapon w && w.Damage != null) || (i is Armor a && a.BaseAC > 0) || (i.Description != "" && i.Description != null)))))
                                    entries.Enqueue(pos);
                        }
                        int sheetCount = 0;
                        while (entries.Count > 0 || (pdf.Duplex && !pdf.DuplexWhite && sheetCount % 2 != 0))
                        {
                            int counter = 1;
                            using (IPDFEditor sbp = await pdf.CreateEditor(SpellbookFile))
                            {
                                sheetCount++;
                                FillBasicFields(booktrans, sbp, context, pdf);
                                if (booktrans.ContainsKey("Sheet")) sbp.SetField(booktrans["Sheet"], sheetCount.ToString());
                                while (entries.Count > 0 && (booktrans.ContainsKey("Name" + counter) || booktrans.ContainsKey("Description" + counter)))
                                {
                                    object xml = entries.Dequeue();
                                    if (xml is Spell entry)
                                    {
                                        StringBuilder description = new StringBuilder();
                                        String name = entry.Name;
                                        String spellLevel = "";
                                        List<Keyword> original = entry.GetKeywords();
                                        List<Keyword> keywords = new List<Keyword>(original);
                                        if (booktrans.ContainsKey("Level" + counter)) sbp.SetField(booktrans["Level" + counter], entry.Level.ToString());
                                        else spellLevel = (entry.Level == 0 ? "" : " " + AddOrdinal(entry.Level) + " Level");
                                        keywords.RemoveAll(k => k.Name.Equals("cantrip", StringComparison.OrdinalIgnoreCase));
                                        if (booktrans.ContainsKey("School" + counter)) sbp.SetField(booktrans["School" + counter], GetAndRemoveSchool(keywords));
                                        else if (!booktrans.ContainsKey("Keywords" + counter)) spellLevel += " " + GetAndRemoveSchool(keywords);
                                        if (entry.Level == 0) spellLevel += " Cantrip";
                                        if (booktrans.ContainsKey("SchoolLevel" + counter)) sbp.SetField(booktrans["SchoolLevel" + counter], spellLevel);
                                        else name += ", " + spellLevel;
                                        if (booktrans.ContainsKey("Name" + counter)) sbp.SetField(booktrans["Name" + counter], name);
                                        else description.Append(name).AppendLine();
                                        if (booktrans.ContainsKey("Classes" + counter)) sbp.SetField(booktrans["Classes" + counter], GetAndRemoveClasses(keywords, context));
                                        if (booktrans.ContainsKey("Time" + counter)) sbp.SetField(booktrans["Time" + counter], entry.CastingTime);
                                        else description.Append("Casting Time: ").Append(entry.CastingTime).AppendLine();
                                        if (booktrans.ContainsKey("Range" + counter)) sbp.SetField(booktrans["Range" + counter], entry.Range);
                                        else description.Append("Range: ").Append(entry.Range).AppendLine();
                                        if (booktrans.ContainsKey("Components" + counter)) sbp.SetField(booktrans["Components" + counter], GetAndRemoveComponents(keywords));
                                        else if (!booktrans.ContainsKey("Keywords" + counter)) description.Append("Components: ").Append(GetAndRemoveComponents(keywords)).AppendLine();
                                        if (booktrans.ContainsKey("Duration" + counter)) sbp.SetField(booktrans["Duration" + counter], entry.Duration);
                                        else description.Append("Duration: ").Append(entry.Duration).AppendLine();

                                        if (booktrans.ContainsKey("Keywords" + counter)) sbp.SetField(booktrans["Keywords" + counter], String.Join(", ", keywords));

                                        description.Append(entry.Description);
                                        List<IInfoText> add = new List<IInfoText>();
                                        if (booktrans.ContainsKey("AdditionDescription" + counter)) sbp.SetTextAndDescriptions(booktrans["AdditionDescription" + counter], false, null, entry.Descriptions);
                                        else add.AddRange(entry.Descriptions);
                                        //foreach (Description d in entry.Descriptions)
                                        //{
                                        //    add.Append(d.Name.ToUpperInvariant()).Append(": ").Append(d.Text.Trim(new char[] { ' ', '\r', '\n', '\t' })).AppendLine();
                                        //    if (d is ListDescription) foreach (Names n in (d as ListDescription).Names) add.Append(n.Title).Append(": ").Append(String.Join(", ", n.ListOfNames)).AppendLine();
                                        //    if (d is TableDescription) foreach (TableEntry tr in (d as TableDescription).Entries) add.Append(tr.ToFullString()).AppendLine();
                                        //}
                                        //if (booktrans.ContainsKey("AdditionDescription" + counter)) sbp.SetField(booktrans["AdditionDescription" + counter], add.ToString());
                                        //else description.AppendLine().AppendLine().Append(add.ToString());
                                        List<IInfoText> modifiers = new List<IInfoText>();
                                        foreach (SpellModifyFeature m in mods.Where(f => Utils.Matches(context, entry, ((SpellModifyFeature)f).Spells, null))) modifiers.Add(m);
                                        string posttext = null;
                                        if (booktrans.ContainsKey("Modifiers" + counter)) sbp.SetTextAndDescriptions(booktrans["Modifiers" + counter], pdf.OnlyFeatureTitles, null, modifiers);
                                        else if (pdf.OnlyFeatureTitles) posttext = String.Join(",", modifiers.Select(m => m.ToInfo(false)));
                                        else add.AddRange(modifiers);

                                        if (booktrans.ContainsKey("Source" + counter)) {
                                            sbp.SetField(booktrans["Source" + counter], entry.Source);
                                            if (booktrans.ContainsKey("Description" + counter)) sbp.SetTextAndDescriptions(booktrans["Description" + counter], false, description.ToString(), add, posttext);
                                        }
                                        else if (booktrans.ContainsKey("Description" + counter)) sbp.SetTextAndDescriptions(booktrans["Description" + counter], false, description.ToString(), add, (posttext != null ? posttext + "\n" : "") + "Source: " + entry.Source);
                                        counter++;
                                    }
                                    else if (xml is Possession pos)
                                    {
                                        StringBuilder description = new StringBuilder();
                                        HashSet<string> source = new HashSet<string>();
                                        HashSet<string> keywords = new HashSet<string>();
                                        if (booktrans.ContainsKey("Name" + counter)) sbp.SetField(booktrans["Name" + counter], pos.ToString());
                                        else description.Append(pos.ToString()).AppendLine();
                                        if (pos.Description != null && pos.Description != "") description.AppendLine(pos.Description);
                                        foreach (MagicProperty mp in pos.Magic)
                                        {
                                            string d = mp.DisplayRequirement;
                                            if (d != null && d != "") keywords.Add(d);
                                            if (mp.Source != null && mp.Source != "") source.Add(mp.Source);
                                            string t = mp.Text;
                                            if (t != null && t.Trim(new char[] { ' ', '\r', '\n', '\t' }) != "")
                                            {
                                                description.AppendLine().AppendLine(t.Trim(new char[] { ' ', '\r', '\n', '\t' }));
                                            }
                                        }
                                        if (booktrans.ContainsKey("Classes" + counter)) sbp.SetField(booktrans["Classes" + counter], string.Join("; ", keywords));
                                        if (pos.Item is Item item)
                                        {
                                            if (pos.Magic.Count > 0) description.AppendLine();
                                            if (!pdf.EquipmentKeywords && item.Keywords != null)
                                            {
                                                description.AppendLine(String.Join(", ", item.Keywords.Select(s => s.ToString())));
                                            }
                                            if (!pdf.EquipmentStats) {
                                                if (item is Weapon w && w.Damage != null) description.Append(w.Damage).Append(" ").Append(w.DamageType ?? "").AppendLine();
                                                if (item is Armor a && a.BaseAC > 0) description.Append(a.BaseAC).Append(" AC").AppendLine();
                                                //if (item is Pack pack) description.Append(String.Join(", ", pack.Contents)).AppendLine();
                                                if (item.Description != null && item.Description != "") description.AppendLine(item.Description);
                                            }
                                            source.Add(item.Source);
                                        }
                                        if (booktrans.ContainsKey("Source" + counter)) sbp.SetField(booktrans["Source" + counter], string.Join(", ", source));
                                        else description.AppendLine().Append("Source: ").Append(string.Join(", ", source)).AppendLine();
                                        if (booktrans.ContainsKey("Description" + counter)) sbp.SetField(booktrans["Description" + counter], description.ToString().Trim(new char[] { ' ', '\r', '\n', '\t' }));
                                        counter++;
                                    }
                                    
                                }
                                if (counter > 1 || (pdf.Duplex && !pdf.DuplexWhite && sheetCount % 2 != 1)) sheet.Add(sbp);
                            }
                        }
                        await sheet.AddBlankPages(SpellbookFile);
                    }
                    
                    if (pdf.IncludeActions && ActionsFile != null && ActionsFile != "" && (actiontrans.ContainsKey("Action1Name") || actiontrans.ContainsKey("Action1Text")))
                    {
                        Queue<ActionInfo> entries = new Queue<ActionInfo>(context.Player.GetActions());
                        int sheetCount = 0;
                        String file = ActionsFile;
                        while (entries.Count > 0 || (pdf.Duplex && !pdf.DuplexWhite && sheetCount % 2 != 0))
                        {
                            if (sheetCount > 0 && ActionsFile2 != null && ActionsFile2 != "" && (actiontrans2.ContainsKey("Action1Name") || actiontrans2.ContainsKey("Action1Text")))
                            {
                                file = ActionsFile2;
                                actiontrans = actiontrans2;
                            }
                            int counter = 1;

                            using (IPDFEditor abp = await pdf.CreateEditor(file))
                            {
                                sheetCount++;
                                FillBasicFields(actiontrans, abp, context, pdf);
                                if (actiontrans.ContainsKey("Sheet")) abp.SetField(actiontrans["Sheet"], sheetCount.ToString());
                                if (pdf.IncludeResources)
                                {
                                    if (trans.ContainsKey("CurrentHP")) abp.SetField(trans["CurrentHP"], (maxhp + context.Player.CurrentHPLoss).ToString());
                                    if (trans.ContainsKey("TempHP")) abp.SetField(trans["TempHP"], context.Player.TempHP.ToString());
                                    for (int d = 1; d <= context.Player.FailedDeathSaves; d++) if (trans.ContainsKey("DeathSaveFail" + d)) abp.SetField(trans["DeathSaveFail" + d], "Yes");
                                        else break;
                                    for (int d = 1; d <= context.Player.SuccessDeathSaves; d++) if (trans.ContainsKey("DeathSaveSuccess" + d)) abp.SetField(trans["DeathSaveSuccess" + d], "Yes");
                                        else break;
                                    if (trans.ContainsKey("HitDie")) abp.SetField(trans["HitDie"], String.Join(", ", hd));
                                    if (trans.ContainsKey("Inspiration")) if (context.Player.Inspiration) abp.SetField(trans["Inspiration"], "Yes");
                                    foreach (var e in actiontrans.AsEnumerable())
                                    {
                                        if (e.Key.StartsWith("Condition"))
                                        {
                                            if (context.Player.Conditions.Exists(s => StringComparer.OrdinalIgnoreCase.Equals("Condition" + s, e.Key)))
                                            {
                                                abp.SetField(actiontrans[e.Value], "Yes");
                                            }
                                        }
                                    }
                                }
                                while (entries.Count > 0 && (actiontrans.ContainsKey("Action" + counter + "Name") || actiontrans.ContainsKey("Action" + counter + "Text")))
                                {
                                    ActionInfo entry = entries.Dequeue();
                                    StringBuilder name = new StringBuilder();
                                    name.Append(entry.Name);

                                    if (actiontrans.ContainsKey("Action" + counter + "Type")) abp.SetField(actiontrans["Action" + counter + "Type"], Type(entry.Type));
                                    else name.Append(" - ").Append(entry.Type);
                                    if (actiontrans.ContainsKey("Action" + counter + "Source")) abp.SetField(actiontrans["Action" + counter + "Source"], entry.Source);
                                    else name.Append(" (").Append(entry.Source).Append(")");
                                    if (actiontrans.ContainsKey("Action" + counter + "Name"))
                                    {
                                        abp.SetField(actiontrans["Action" + counter + "Name"], name.ToString());
                                        name = new StringBuilder();
                                    }
                                    else name.Append(" ").Append(entry.Name);
                                    if (actiontrans.ContainsKey("Action" + counter + "Text"))
                                    {
                                        if (name.Length > 0) abp.SetField(actiontrans["Action" + counter + "Text"], (name.ToString() + " " + entry.Text).Replace("\n", " "));
                                        else abp.SetField(actiontrans["Action" + counter + "Text"], entry.Text.Replace("\n", " "));
                                    }
                                    counter++;
                                }
                                if (counter > 1 || (pdf.Duplex && !pdf.DuplexWhite && sheetCount % 2 != 1)) sheet.Add(abp);
                            }
                        }
                        if (sheetCount > 0 && ActionsFile2 != null && ActionsFile2 != "" && (actiontrans2.ContainsKey("Action1Name") || actiontrans2.ContainsKey("Action1Text")))
                        {
                            file = ActionsFile2;
                        }
                        await sheet.AddBlankPages(file);
                    }
                    
                    if (pdf.IncludeMonsters && MonstersFile != null && MonstersFile != "" && (monstertrans.ContainsKey("Name1") || monstertrans.ContainsKey("Traits1")))
                    {
                        Dictionary<Monster, MonsterInfo> monsters = new Dictionary<Monster, MonsterInfo>(new MonsterInfo());
                        foreach (FormsCompanionInfo fc in context.Player.GetFormsCompanionChoices())
                        {
                            foreach (Monster m in fc.AppliedChoices(context, context.Player.GetFinalAbilityScores()))
                            {
                                if (monsters.ContainsKey(m))
                                {
                                    monsters[m].Sources.Add(fc.DisplayName);
                                }
                                else
                                {
                                    monsters.Add(m, new MonsterInfo()
                                    {
                                        Monster = m,
                                        Sources = new List<string>() { fc.DisplayName }
                                    });
                                }
                            }
                        }
                        Queue<MonsterInfo> entries = new Queue<MonsterInfo>(monsters.Values.OrderBy(s => s.Monster.Name));
                        int sheetCount = 0;
                        while (entries.Count > 0)
                        {
                            int counter = 1;

                            using (IPDFEditor mp = await pdf.CreateEditor(MonstersFile))
                            {
                                sheetCount++;
                                FillBasicFields(monstertrans, mp, context, pdf);
                                if (monstertrans.ContainsKey("Sheet")) mp.SetField(monstertrans["Sheet"], sheetCount.ToString());
                                while (entries.Count > 0 && (monstertrans.ContainsKey("Name" + counter) || monstertrans.ContainsKey("Traits" + counter)))
                                {
                                    MonsterInfo entry = entries.Dequeue();
                                    StringBuilder traits = new StringBuilder();
                                    if (monstertrans.ContainsKey("Name" + counter)) mp.SetField(monstertrans["Name" + counter], entry.Monster.Name);
                                    else traits.Append(entry.Monster.Name);
                                    if (monstertrans.ContainsKey("Feature" + counter)) mp.SetField(monstertrans["Feature" + counter], string.Join(", ", entry.Sources));
                                    else traits.Append("From: ").Append(string.Join(", ", entry.Sources)).Append("\n");

                                    if (monstertrans.ContainsKey("Flavour" + counter)) mp.SetField(monstertrans["Flavour" + counter], entry.Monster.Flavour);
                                    if (monstertrans.ContainsKey("Descriptions" + counter))
                                    {
                                        mp.SetField(monstertrans["Description" + counter], string.Join("\n", entry.Monster.Descriptions.Select(s => s.Name + ": " + s.Text)));
                                        if (monstertrans.ContainsKey("Description" + counter)) mp.SetField(monstertrans["Description" + counter], entry.Monster.Description);
                                    }
                                    else if (monstertrans.ContainsKey("Description" + counter)) mp.SetField(monstertrans["Description" + counter], entry.Monster.Description + "\n" + string.Join("\n", entry.Monster.Descriptions.Select(s => s.Name + ": " + s.Text)));

                                    StringBuilder type = new StringBuilder();
                                    if (monstertrans.ContainsKey("Size" + counter)) mp.SetField(monstertrans["Size" + counter], entry.Monster.Size.ToString());
                                    else type.Append(entry.Monster.Size.ToString());
                                    List<Keyword> t = new List<Keyword>(entry.Monster.Keywords);
                                    if (t.Count >= 1)
                                    {
                                        AppendIfContent(type, " ").Append(t[0].ToString().ToLowerInvariant());
                                        t.RemoveAt(0);
                                        if (t.Count >= 1) type.Append(" (").Append(string.Join(", ", t.Select(s => s.ToString())).ToLowerInvariant()).Append(")");
                                    }
                                    if (monstertrans.ContainsKey("Alignment" + counter)) mp.SetField(monstertrans["Alignment" + counter], entry.Monster.Alignment);
                                    else if (entry.Monster.Alignment != null && entry.Monster.Alignment != "") type.Append(", ").Append(entry.Monster.Alignment);
                                    if (monstertrans.ContainsKey("Type" + counter)) mp.SetField(monstertrans["Type" + counter], type.ToString());
                                    else if (type.Length > 0) traits.Append("Type: ").Append(type.ToString()).Append("\n");

                                    if (monstertrans.ContainsKey("Image" + counter)) mp.SetImage(monstertrans["Image" + counter], entry.Monster.ImageData);

                                    string AC = entry.Monster.AC.ToString();
                                    if (monstertrans.ContainsKey("ACText" + counter)) mp.SetField(monstertrans["ACText" + counter], entry.Monster.ACText);
                                    else if (entry.Monster.ACText != null && entry.Monster.ACText != "") AC = AC + " (" + entry.Monster.ACText + ")";
                                    if (monstertrans.ContainsKey("AC" + counter)) mp.SetField(monstertrans["AC" + counter], AC);
                                    else traits.Append("Armor Class ").Append(AC).Append("\n");

                                    string HP = entry.Monster.HP.ToString();
                                    if (monstertrans.ContainsKey("HPRoll" + counter)) mp.SetField(monstertrans["HPRoll" + counter], entry.Monster.HPRoll);
                                    else if (entry.Monster.HPRoll != null && entry.Monster.HPRoll != "") HP = HP + " (" + entry.Monster.HPRoll + ")";
                                    if (monstertrans.ContainsKey("HP" + counter)) mp.SetField(monstertrans["HP" + counter], HP);
                                    else traits.Append("Hit Points ").Append(HP).Append("\n");

                                    if (monstertrans.ContainsKey("Speed" + counter)) mp.SetField(monstertrans["Speed" + counter], string.Join(", ", entry.Monster.Speeds));
                                    else traits.Append("Speeds ").Append(string.Join(", ", entry.Monster.Speeds)).Append("\n");

                                    StringBuilder stats = new StringBuilder();
                                    if (monstertrans.ContainsKey("StrMod" + counter))
                                    {
                                        mp.SetField(monstertrans["StrMod" + counter], (entry.Monster.Strength / 2 - 5).ToString());
                                        if (monstertrans.ContainsKey("Str" + counter)) mp.SetField(monstertrans["Str" + counter], entry.Monster.Strength.ToString());
                                    }
                                    else if (monstertrans.ContainsKey("Str" + counter)) mp.SetField(monstertrans["Str" + counter], entry.Monster.Strength.ToString() + " (" + (entry.Monster.Strength / 2 - 5).ToString("+#;-#;+0") + ")");
                                    else stats.Append("Str: ").Append(entry.Monster.Strength.ToString() + " (" + (entry.Monster.Strength / 2 - 5).ToString("+#;-#;+0") + ")").Append("\n");
                                    if (monstertrans.ContainsKey("DexMod" + counter))
                                    {
                                        mp.SetField(monstertrans["DexMod" + counter], (entry.Monster.Dexterity / 2 - 5).ToString());
                                        if (monstertrans.ContainsKey("Dex" + counter)) mp.SetField(monstertrans["Dex" + counter], entry.Monster.Dexterity.ToString());
                                    }
                                    else if (monstertrans.ContainsKey("Dex" + counter)) mp.SetField(monstertrans["Dex" + counter], entry.Monster.Dexterity.ToString() + " (" + (entry.Monster.Dexterity / 2 - 5).ToString("+#;-#;+0") + ")");
                                    else AppendIfContent(stats, " ").Append("Dex: ").Append(entry.Monster.Dexterity.ToString() + " (" + (entry.Monster.Dexterity / 2 - 5).ToString("+#;-#;+0") + ")").Append("\n");
                                    if (monstertrans.ContainsKey("ConMod" + counter))
                                    {
                                        mp.SetField(monstertrans["ConMod" + counter], (entry.Monster.Constitution / 2 - 5).ToString());
                                        if (monstertrans.ContainsKey("Con" + counter)) mp.SetField(monstertrans["Con" + counter], entry.Monster.Constitution.ToString());
                                    }
                                    else if (monstertrans.ContainsKey("Con" + counter)) mp.SetField(monstertrans["Con" + counter], entry.Monster.Constitution.ToString() + " (" + (entry.Monster.Constitution / 2 - 5).ToString("+#;-#;+0") + ")");
                                    else AppendIfContent(stats, " ").Append("Con: ").Append(entry.Monster.Constitution.ToString() + " (" + (entry.Monster.Constitution / 2 - 5).ToString("+#;-#;+0") + ")").Append("\n");
                                    if (monstertrans.ContainsKey("IntMod" + counter))
                                    {
                                        mp.SetField(monstertrans["IntMod" + counter], (entry.Monster.Intelligence / 2 - 5).ToString());
                                        if (monstertrans.ContainsKey("Int" + counter)) mp.SetField(monstertrans["Int" + counter], entry.Monster.Intelligence.ToString());
                                    }
                                    else if (monstertrans.ContainsKey("Int" + counter)) mp.SetField(monstertrans["Int" + counter], entry.Monster.Intelligence.ToString() + " (" + (entry.Monster.Intelligence / 2 - 5).ToString("+#;-#;+0") + ")");
                                    else AppendIfContent(stats, " ").Append("Int: ").Append(entry.Monster.Intelligence.ToString() + " (" + (entry.Monster.Intelligence / 2 - 5).ToString("+#;-#;+0") + ")").Append("\n");
                                    if (monstertrans.ContainsKey("WisMod" + counter))
                                    {
                                        mp.SetField(monstertrans["WisMod" + counter], (entry.Monster.Wisdom / 2 - 5).ToString());
                                        if (monstertrans.ContainsKey("Wis" + counter)) mp.SetField(monstertrans["Wis" + counter], entry.Monster.Wisdom.ToString());
                                    }
                                    else if (monstertrans.ContainsKey("Wis" + counter)) mp.SetField(monstertrans["Wis" + counter], entry.Monster.Wisdom.ToString() + " (" + (entry.Monster.Wisdom / 2 - 5).ToString("+#;-#;+0") + ")");
                                    else AppendIfContent(stats, " ").Append("Wis: ").Append(entry.Monster.Wisdom.ToString() + " (" + (entry.Monster.Wisdom / 2 - 5).ToString("+#;-#;+0") + ")").Append("\n");
                                    if (monstertrans.ContainsKey("ChaMod" + counter))
                                    {
                                        mp.SetField(monstertrans["ChaMod" + counter], (entry.Monster.Charisma / 2 - 5).ToString());
                                        if (monstertrans.ContainsKey("Cha" + counter)) mp.SetField(monstertrans["Cha" + counter], entry.Monster.Charisma.ToString());
                                    }
                                    else if (monstertrans.ContainsKey("Cha" + counter)) mp.SetField(monstertrans["Cha" + counter], entry.Monster.Charisma.ToString() + " (" + (entry.Monster.Charisma / 2 - 5).ToString("+#;-#;+0") + ")");
                                    else AppendIfContent(stats, " ").Append("Cha: ").Append(entry.Monster.Charisma.ToString() + " (" + (entry.Monster.Charisma / 2 - 5).ToString("+#;-#;+0") + ")").Append("\n");

                                    if (monstertrans.ContainsKey("Stats" + counter)) mp.SetField(monstertrans["Stats" + counter], stats.ToString());
                                    else if (stats.Length > 0) traits.Append(stats.ToString()).Append("\n");

                                    if (monstertrans.ContainsKey("Saves" + counter)) mp.SetField(monstertrans["Saves" + counter], string.Join(", ", entry.Monster.SaveBonus.Select(s => s.ToString(entry.Monster))));
                                    else if (entry.Monster.SaveBonus.Count > 0) traits.Append("Saving Throws ").Append(string.Join(", ", entry.Monster.SaveBonus.Select(s => s.ToString(entry.Monster)))).Append("\n");

                                    if (monstertrans.ContainsKey("Skills" + counter)) mp.SetField(monstertrans["Skills" + counter], string.Join(", ", entry.Monster.SkillBonus.Select(s => s.ToString(entry.Monster, context))));
                                    else if (entry.Monster.SkillBonus.Count > 0) traits.Append("Skills ").Append(string.Join(", ", entry.Monster.SkillBonus.Select(s => s.ToString(entry.Monster, context)))).Append("\n");

                                    if (monstertrans.ContainsKey("Vulnerabilities" + counter)) mp.SetField(monstertrans["Vulnerabilities" + counter], string.Join(", ", entry.Monster.Vulnerablities));
                                    else if (entry.Monster.Vulnerablities.Count > 0) traits.Append("Damage Vulnerabilities ").Append(string.Join(", ", entry.Monster.Vulnerablities)).Append("\n");
                                    if (monstertrans.ContainsKey("Resistances" + counter)) mp.SetField(monstertrans["Resistances" + counter], string.Join(", ", entry.Monster.Resistances));
                                    else if (entry.Monster.Resistances.Count > 0) traits.Append("Damage Resistances ").Append(string.Join(", ", entry.Monster.Resistances)).Append("\n");
                                    if (monstertrans.ContainsKey("Immunities" + counter)) mp.SetField(monstertrans["Immunities" + counter], string.Join(", ", entry.Monster.Immunities));
                                    else if (entry.Monster.Immunities.Count > 0) traits.Append("Damage Immunities ").Append(string.Join(", ", entry.Monster.Immunities)).Append("\n");
                                    if (monstertrans.ContainsKey("ConditionImmunities" + counter)) mp.SetField(monstertrans["ConditionImmunities" + counter], string.Join(", ", entry.Monster.ConditionImmunities));
                                    else if (entry.Monster.ConditionImmunities.Count > 0) traits.Append("Condition Immunities ").Append(string.Join(", ", entry.Monster.ConditionImmunities)).Append("\n");
                                    List<string> senses = new List<string>(entry.Monster.Senses);
                                    int percept = 0;
                                    foreach (var ms in entry.Monster.SkillBonus) if (StringComparer.OrdinalIgnoreCase.Equals(ms.Skill, "perception")) percept = ms.Bonus;
                                    senses.Add("passive Perception " + (entry.Monster.Wisdom / 2 + 5 + percept).ToString());
                                    if (monstertrans.ContainsKey("Senses" + counter)) mp.SetField(monstertrans["Senses" + counter], string.Join(", ", senses));
                                    else traits.Append("Senses ").Append(string.Join(", ", senses)).Append("\n");
                                    if (monstertrans.ContainsKey("Languages" + counter)) mp.SetField(monstertrans["Languages" + counter], entry.Monster.Languages.Count > 0 ? string.Join(", ", entry.Monster.Languages) : "--");
                                    else traits.Append("Languages ").Append(entry.Monster.Languages.Count > 0 ? string.Join(", ", entry.Monster.Languages) : "--").Append("\n");

                                    string CR = entry.Monster.CR.ToString("#");
                                    if (entry.Monster.CR == 0m) CR = "0";
                                    if (entry.Monster.CR == 0.125m) CR = "1/8";
                                    if (entry.Monster.CR == 0.25m) CR = "\u00BC";
                                    if (entry.Monster.CR == 0.5m) CR = "\u00BD";
                                    if (monstertrans.ContainsKey("XP" + counter)) mp.SetField(monstertrans["XP" + counter], entry.Monster.XP.ToString());
                                    else CR = CR + " (" + entry.Monster.XP.ToString() + " XP)";
                                    if (monstertrans.ContainsKey("CR" + counter)) mp.SetField(monstertrans["CR" + counter], CR);
                                    else traits.Append("Challenge ").Append(CR).Append("\n");
                                    if (traits.Length > 0) traits.Append("\n");
                                    foreach (MonsterTrait mt in entry.Monster.Traits)
                                    {
                                        if (traits.Length > 0) traits.Append("\n");
                                        traits.Append(mt.Name).Append(". ").Append(mt.Text).Append("\n");
                                        if (mt is MonsterAction ma) traits.Append("  Attack: ").Append(PlusMinus(ma.AttackBonus)).Append(", ").Append(ma.Damage).Append(" damage,").Append("\n");
                                    }
                                    StringBuilder t2;
                                    if (monstertrans.ContainsKey("Actions" + counter)) t2 = new StringBuilder();
                                    else
                                    {
                                        t2 = traits;
                                        if (entry.Monster.Actions.Count > 0) traits.Append("\nActions:\n");
                                    }
                                    foreach (MonsterTrait mt in entry.Monster.Actions)
                                    {
                                        AppendIfContent(t2, "\n").Append(mt.Name).Append(". ").Append(mt.Text).Append("\n");
                                        if (mt is MonsterAction ma) t2.Append("  Attack: ").Append(PlusMinus(ma.AttackBonus)).Append(", ").Append(ma.Damage).Append(" damage,").Append("\n");
                                    }
                                    if (monstertrans.ContainsKey("Actions" + counter)) mp.SetField(monstertrans["Actions" + counter], t2.ToString());

                                    if (monstertrans.ContainsKey("Reactions" + counter)) t2 = new StringBuilder();
                                    else
                                    {
                                        t2 = traits;
                                        if (entry.Monster.Reactions.Count > 0) traits.Append("\nReactions:\n");
                                    }
                                    foreach (MonsterTrait mt in entry.Monster.Reactions)
                                    {
                                        AppendIfContent(t2, "\n").Append(mt.Name).Append(". ").Append(mt.Text).Append("\n");
                                        if (mt is MonsterAction ma) t2.Append("  Attack: ").Append(PlusMinus(ma.AttackBonus)).Append(", ").Append(ma.Damage).Append(" damage,").Append("\n");
                                    }
                                    if (monstertrans.ContainsKey("Reactions" + counter)) mp.SetField(monstertrans["Reactions" + counter], t2.ToString());

                                    if (monstertrans.ContainsKey("LegendaryActions" + counter)) t2 = new StringBuilder();
                                    else
                                    {
                                        t2 = traits;
                                        if (entry.Monster.Reactions.Count > 0) traits.Append("\nLegendary Actions:\n");
                                    }
                                    foreach (MonsterTrait mt in entry.Monster.LegendaryActions)
                                    {
                                        AppendIfContent(t2, "\n").Append(mt.Name).Append(". ").Append(mt.Text).Append("\n");
                                        if (mt is MonsterAction ma) t2.Append("  Attack: ").Append(PlusMinus(ma.AttackBonus)).Append(", ").Append(ma.Damage).Append(" damage,").Append("\n");
                                    }
                                    if (monstertrans.ContainsKey("LegendaryActions" + counter)) mp.SetField(monstertrans["LegendaryActions" + counter], t2.ToString());

                                    if (monstertrans.ContainsKey("Source" + counter)) mp.SetField(monstertrans["Source" + counter], entry.Monster.Source);
                                    else AppendIfContent(traits, "\n").Append("Source: ").Append(entry.Monster.Source);

                                    if (monstertrans.ContainsKey("Trait" + counter)) mp.SetField(monstertrans["Trait" + counter], traits.ToString());
                                    counter++;
                                }
                                if (counter > 1) sheet.Add(mp);
                            }
                        }
                    }

                }
            }
        }

        private string ToString(int uncommon, int common, int consumables, bool relative = false)
        {
            string s = "";
            if (uncommon != 0) s += (relative && uncommon > 0 ? "+" : "") + uncommon + " Uncommon+";
            if (common != 0) s += (s.Length>0 ? ", " : "") + (relative && common > 0? "+" : "") + common + " Common";
            if (consumables != 0) s += (s.Length > 0 ? ", " : "") + (relative && consumables > 0 ? "+" : "") + consumables + " Consumable";
            return s;
        }

        public static string APFormat(BuilderContext context, string format, int ap)
        {
            int level = context.Levels.Get(ap, true);
            int next = 8;
            if (level < context.Levels.Advancement.Count)
            {
                next = context.Levels.Advancement[level] - (level > 0 ? context.Levels.Advancement[level - 1] : 0);
            }
            int over = ap - (level > 0 ? context.Levels.Advancement[level - 1] : 0);
            return String.Format(format, ap, over, level, next);
        }

        public static StringBuilder AppendIfContent(StringBuilder s, string v)
        {
            if (s.Length > 0) s.Append(v);
            return s;
        }

        private static string Type(ActionType type)
        {
            switch (type)
            {
                case ActionType.Action: return "A";
                case ActionType.BonusAction: return "B";
                case ActionType.Reaction: return "R";
                case ActionType.MoveAction: return "M";
                default: return "";
            }
        }

        private static List<Keyword> Schools = new List<Keyword>()
        {
            new Keyword("Abjuration"), new Keyword("Conjuration"), new Keyword("Divination"), new Keyword("Enchantment"), new Keyword("Evocation"), new Keyword("Illusion"), new Keyword("Necromancy"), new Keyword("Transmutation")
        };

        private string GetAndRemoveSchool(List<Keyword> kw)
        {
            List<string> s = new List<string>();
            foreach (Keyword k in Schools) if (kw.Remove(k)) s.Add(k.Name.ToLowerInvariant());
            string res = string.Join(", ", s);
            if (res == null || res == "") return "";
            return char.ToUpper(res[0]) + res.Substring(1);
        }

        private string GetAndRemoveComponents(List<Keyword> kw)
        {
            List<string> r = new List<string>();
            bool v = false, s = false, m = false, roy = false;
            string mat = "";
            string price = "";
            for (int i = kw.Count - 1; i >= 0; i--)
            {
                if (kw[i].Name.Equals("verbal", StringComparison.OrdinalIgnoreCase))
                {
                    v = true;
                    kw.RemoveAt(i);
                }
                else if (kw[i].Name.Equals("somatic", StringComparison.OrdinalIgnoreCase))
                {
                    s = true;
                    kw.RemoveAt(i);
                }
                else if (kw[i] is Material)
                {
                    m = true;
                    mat = mat == "" ? (kw[i] as Material).Components : mat + "; " + (kw[i] as Material).Components;
                    kw.RemoveAt(i);
                }
                else if (kw[i] is Royalty)
                {
                    roy = true;
                    price = price == "" ? (kw[i] as Royalty).Price : price + "; " + (kw[i] as Royalty).Price;
                    kw.RemoveAt(i);
                }
            }
            if (v) r.Add("V");
            if (s) r.Add("S");
            if (m) r.Add("M(" + mat + ")");
            if (roy) r.Add("R(" + price + ")");
            string res = string.Join(", ", r);
            if (res == null || res == "") return "";
            return char.ToUpper(res[0]) + res.Substring(1);
        }

        private static List<Keyword> Classes = new List<Keyword>();
        private string GetAndRemoveClasses(List<Keyword> kw, BuilderContext context)
        {
            if (Classes.Count == 0) foreach (string cl in context.ClassesSimple.Keys) Classes.Add(new Keyword(cl));
            List<string> s = new List<string>();
            foreach (Keyword k in Classes) if (kw.Remove(k)) s.Add(char.ToUpper(k.Name[0]) + k.Name.Substring(1).ToLowerInvariant());
            return string.Join(", ", s);
        }

        private static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }

        private void FillBasicFields(Dictionary<string, string> trans, IPDFEditor p, BuilderContext context, PDFBase pdf, bool swap = false)
        {
            swap = swap && pdf.SwapScoreAndMod;
            int down = 0;
            int renown = 0;
            int magic = 0;
            int t1tp = 0;
            int t2tp = 0;
            int t3tp = 0;
            int t4tp = 0;
            List<IInfoText> journal = new List<IInfoText>(context.Player.Journal.Select(s => ToFeature(s)));
            if (trans.ContainsKey("Renown") || trans.ContainsKey("MagicItems") || trans.ContainsKey("Downtime") || trans.ContainsKey("TreasurePoints") || trans.ContainsKey("Tier1TreasurePoints") || trans.ContainsKey("Tier2TreasurePoints") || trans.ContainsKey("Tier3TreasurePoints") || trans.ContainsKey("Tier4TreasurePoints") || trans.ContainsKey("Journal") || trans.ContainsKey("Backstory"))
            {
                foreach (JournalEntry je in context.Player.ComplexJournal)
                {
                    down += je.Downtime;
                    renown += je.Renown;
                    magic += je.MagicItems;
                    t1tp += je.T1TP;
                    t2tp += je.T2TP;
                    t3tp += je.T3TP;
                    t4tp += je.T4TP;
                    journal.AddRange(je.Notes.Select(s => ToFeature(s)));
                }
            }
            bool advancement = context.Player.Advancement;
            int ap = advancement ? context.Player.GetXP() : context.Levels.ToAP(context.Player.GetXP());
            for (int i = 1; i<=ap; i++)
            {
                if (trans.ContainsKey("APCheck" + i)) p.SetField(trans["APCheck" + i], "Yes");
                else break;
            }
            if (trans.ContainsKey("Background")) p.SetField(trans["Background"], SourceInvariantComparer.NoSource(context.Player.BackgroundName));
            if (trans.ContainsKey("Race")) p.SetField(trans["Race"], context.Player.GetRaceSubName());
            if (trans.ContainsKey("PersonalityTrait")) p.SetField(trans["PersonalityTrait"], context.Player.PersonalityTrait + (context.Player.PersonalityTrait2 == null || context.Player.PersonalityTrait2 == "" || context.Player.PersonalityTrait2 == "- None -" ? "" : "\n" + context.Player.PersonalityTrait2));
            if (trans.ContainsKey("Ideal")) p.SetField(trans["Ideal"], context.Player.Ideal);
            if (trans.ContainsKey("Bond")) p.SetField(trans["Bond"], context.Player.Bond);
            if (trans.ContainsKey("Flaw")) p.SetField(trans["Flaw"], context.Player.Flaw);
            if (trans.ContainsKey("PlayerName")) p.SetField(trans["PlayerName"], context.Player.PlayerName);
            if (trans.ContainsKey("Alignment")) p.SetField(trans["Alignment"], context.Player.Alignment);
            if (trans.ContainsKey("XP")) p.SetField(trans["XP"], advancement ? APFormat(context, pdf.APFormat, context.Player.GetXP()) + " ACP" : context.Player.GetXP().ToString());
            if (trans.ContainsKey("XPTotal")) p.SetField(trans["XPTotal"], advancement ? context.Levels.ToXP(context.Player.GetXP()).ToString() : context.Player.GetXP().ToString());
            if (trans.ContainsKey("AP")) p.SetField(trans["AP"], advancement ? APFormat(context, pdf.APFormat, context.Player.GetXP()) : context.Player.GetXP().ToString() + " XP");
            if (trans.ContainsKey("APTotal")) p.SetField(trans["APTotal"], advancement ? context.Player.GetXP().ToString() : APFormat(context, pdf.APFormat, context.Levels.ToXP(context.Player.GetXP())));
            if (trans.ContainsKey("Age")) p.SetField(trans["Age"], context.Player.Age.ToString());
            if (trans.ContainsKey("Height")) p.SetField(trans["Height"], context.Player.Height.ToString());
            if (trans.ContainsKey("Weight")) p.SetField(trans["Weight"], context.Player.Weight.ToString() + " lb");
            if (trans.ContainsKey("Eyes")) p.SetField(trans["Eyes"], context.Player.Eyes.ToString());
            if (trans.ContainsKey("Skin")) p.SetField(trans["Skin"], context.Player.Skin.ToString());
            if (trans.ContainsKey("Hair")) p.SetField(trans["Hair"], context.Player.Hair.ToString());
            if (trans.ContainsKey("Speed")) p.SetField(trans["Speed"], context.Player.GetSpeed().ToString());
            if (trans.ContainsKey("FactionName")) p.SetField(trans["FactionName"], context.Player.FactionName);
            if (trans.ContainsKey("Journal"))
            {
                p.SetTextAndDescriptions(trans["Journal"], false, null, journal);
                if (trans.ContainsKey("Backstory")) p.SetField(trans["Backstory"], context.Player.Backstory);
            } 
            else if (trans.ContainsKey("Backstory")) p.SetTextAndDescriptions(trans["Backstory"], false, context.Player.Backstory, journal);
            if (trans.ContainsKey("Allies")) p.SetField(trans["Allies"], context.Player.Allies);
            if (trans.ContainsKey("Strength")) p.SetField(trans["Strength"], !swap ? context.Player.GetStrength().ToString() : PlusMinus(context.Player.GetStrengthMod()));
            if (trans.ContainsKey("Dexterity")) p.SetField(trans["Dexterity"], !swap ? context.Player.GetDexterity().ToString() : PlusMinus(context.Player.GetDexterityMod()));
            if (trans.ContainsKey("Constitution")) p.SetField(trans["Constitution"], !swap ? context.Player.GetConstitution().ToString() : PlusMinus(context.Player.GetConstitutionMod()));
            if (trans.ContainsKey("Intelligence")) p.SetField(trans["Intelligence"], !swap ? context.Player.GetIntelligence().ToString() : PlusMinus(context.Player.GetIntelligenceMod()));
            if (trans.ContainsKey("Wisdom")) p.SetField(trans["Wisdom"], !swap ? context.Player.GetWisdom().ToString() : PlusMinus(context.Player.GetWisdomMod()));
            if (trans.ContainsKey("Charisma")) p.SetField(trans["Charisma"], !swap ? context.Player.GetCharisma().ToString() : PlusMinus(context.Player.GetCharismaMod()));
            if (trans.ContainsKey("StrengthModifier")) p.SetField(trans["StrengthModifier"], !swap ? PlusMinus(context.Player.GetStrengthMod()) : context.Player.GetStrength().ToString());
            if (trans.ContainsKey("DexterityModifier")) p.SetField(trans["DexterityModifier"], !swap ? PlusMinus(context.Player.GetDexterityMod()) : context.Player.GetDexterity().ToString());
            if (trans.ContainsKey("ConstitutionModifier")) p.SetField(trans["ConstitutionModifier"], !swap ? PlusMinus(context.Player.GetConstitutionMod()) : context.Player.GetConstitution().ToString());
            if (trans.ContainsKey("IntelligenceModifier")) p.SetField(trans["IntelligenceModifier"], !swap ? PlusMinus(context.Player.GetIntelligenceMod()) : context.Player.GetIntelligence().ToString());
            if (trans.ContainsKey("WisdomModifier")) p.SetField(trans["WisdomModifier"], !swap ? PlusMinus(context.Player.GetWisdomMod()) : context.Player.GetWisdom().ToString());
            if (trans.ContainsKey("CharismaModifier")) p.SetField(trans["CharismaModifier"], !swap ? PlusMinus(context.Player.GetCharismaMod()) : context.Player.GetCharisma().ToString());
            if (trans.ContainsKey("ForceStrength")) p.SetField(trans["ForceStrength"], context.Player.GetStrength().ToString());
            if (trans.ContainsKey("ForceDexterity")) p.SetField(trans["ForceDexterity"], context.Player.GetDexterity().ToString());
            if (trans.ContainsKey("ForceConstitution")) p.SetField(trans["ForceConstitution"], context.Player.GetConstitution().ToString());
            if (trans.ContainsKey("ForceIntelligence")) p.SetField(trans["ForceIntelligence"], context.Player.GetIntelligence().ToString());
            if (trans.ContainsKey("ForceWisdom")) p.SetField(trans["ForceWisdom"], context.Player.GetWisdom().ToString());
            if (trans.ContainsKey("ForceCharisma")) p.SetField(trans["ForceCharisma"], context.Player.GetCharisma().ToString());
            if (trans.ContainsKey("ForceStrengthModifier")) p.SetField(trans["ForceStrengthModifier"], PlusMinus(context.Player.GetStrengthMod()));
            if (trans.ContainsKey("ForceDexterityModifier")) p.SetField(trans["ForceDexterityModifier"], PlusMinus(context.Player.GetDexterityMod()));
            if (trans.ContainsKey("ForceConstitutionModifier")) p.SetField(trans["ForceConstitutionModifier"], PlusMinus(context.Player.GetConstitutionMod()));
            if (trans.ContainsKey("ForceIntelligenceModifier")) p.SetField(trans["ForceIntelligenceModifier"], PlusMinus(context.Player.GetIntelligenceMod()));
            if (trans.ContainsKey("ForceWisdomModifier")) p.SetField(trans["ForceWisdomModifier"], PlusMinus(context.Player.GetWisdomMod()));
            if (trans.ContainsKey("AC")) p.SetField(trans["AC"], context.Player.GetAC().ToString());
            if (trans.ContainsKey("ProficiencyBonus")) p.SetField(trans["ProficiencyBonus"], PlusMinus(context.Player.GetProficiency()));
            if (trans.ContainsKey("Initiative")) p.SetField(trans["Initiative"], PlusMinus(context.Player.GetInitiative()));
            if (trans.ContainsKey("ForceCharismaModifier")) p.SetField(trans["ForceCharismaModifier"], PlusMinus(context.Player.GetCharismaMod()));
            if (trans.ContainsKey("CharacterName")) p.SetField(trans["CharacterName"], context.Player.Name);
            if (trans.ContainsKey("CharacterName2")) p.SetField(trans["CharacterName2"], context.Player.Name);
            if (trans.ContainsKey("ClassLevel")) p.SetField(trans["ClassLevel"], String.Join(" | ", context.Player.GetClassesStrings()));
            if (trans.ContainsKey("DCI")) p.SetField(trans["DCI"], context.Player.DCI);
            if (trans.ContainsKey("Renown")) p.SetField(trans["Renown"], renown.ToString());
            if (trans.ContainsKey("MagicItems")) p.SetField(trans["MagicItems"], magic.ToString());
            if (trans.ContainsKey("Downtime")) p.SetField(trans["Downtime"], down.ToString());
            if (trans.ContainsKey("FactionRank")) p.SetField(trans["FactionRank"], context.Player.FactionRank);
            List<string> tp = new List<string>();
            if (t1tp != 0) tp.Add(t1tp.ToString() + " T1");
            if (t2tp != 0) tp.Add(t2tp.ToString() + " T2");
            if (t3tp != 0) tp.Add(t3tp.ToString() + " T3");
            if (t4tp != 0) tp.Add(t4tp.ToString() + " T4");
            if (trans.ContainsKey("TreasurePoints")) p.SetField(trans["TreasurePoints"], String.Join(", ",tp));
            if (trans.ContainsKey("Tier1TreasurePoints")) p.SetField(trans["Tier1TreasurePoints"], t1tp.ToString());
            if (trans.ContainsKey("Tier2TreasurePoints")) p.SetField(trans["Tier2TreasurePoints"], t2tp.ToString());
            if (trans.ContainsKey("Tier3TreasurePoints")) p.SetField(trans["Tier3TreasurePoints"], t3tp.ToString());
            if (trans.ContainsKey("Tier4TreasurePoints")) p.SetField(trans["Tier4TreasurePoints"], t4tp.ToString());
        }

        private IInfoText ToFeature(string s)
        {
            var split = s.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(ss => ss.Trim(" \n\r\t".ToCharArray())).ToList();
            return new Feature(split[0], string.Join(" ", split.Skip(1)));
            
        }

        private string PlusMinus(int value, string zero = "+0")
        {
            if (value > 0) return "+" + value;
            if (value == 0) return zero;
            return value.ToString();
        }
    }
}
