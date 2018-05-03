using Character_Builder;
using OGL;
using OGL.Base;
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
            Dictionary<string, bool> hiddenfeats = context.Player.HiddenFeatures.ToDictionary(f => f, f => true, StringComparer.OrdinalIgnoreCase);
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
                FillBasicFields(trans, p, context);
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
                List<ModifiedSpell> bonusspells=new List<ModifiedSpell>(context.Player.GetBonusSpells());
                if (pdf.IncludeResources)
                {
                    Dictionary<string, int> bsres = context.Player.GetResources();
                    foreach (ModifiedSpell mods in bonusspells)
                    {
                        mods.includeResources = true;
                        if (bsres.ContainsKey(mods.getResourceID())) mods.used = bsres[mods.getResourceID()];
                    }
                } else
                {
                    foreach (ModifiedSpell mods in bonusspells)
                    {
                        mods.includeResources = false;
                    }
                }
                spellbook.AddRange(bonusspells);
                if (trans.ContainsKey("BonusSpells"))
                {
                    p.SetField(trans["BonusSpells"], String.Join("\n", bonusspells));
                }
                else if (trans.ContainsKey("Resources"))
                {
                    if (resources != "") resources += "\n";
                    resources += String.Join("\n", bonusspells);
                }
                else
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
                    if (trans.ContainsKey("CurrentHP")) p.SetField(trans["CurrentHP"], (maxhp+context.Player.CurrentHPLoss).ToString());
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
                int level=context.Player.GetLevel();
                // TODO Proper Items:
                List<Possession> equip=new List<Possession>();
                List<String> equipDetailed = new List<String>();
                List<String> equipDetailed2 = new List<String>();
                List<Possession> treasure=new List<Possession>();
                List<String> treasureDetailed = new List<String>();
                List<Feature> onUse=new List<Feature>();
                foreach (Possession pos in context.Player.GetItemsAndPossessions()) {
                    if ((trans.ContainsKey("Equipment") || trans.ContainsKey("EquipmentShort") || trans.ContainsKey("EquipmentDetailed")) && pos.BaseItem != null && pos.BaseItem != "")
                    {
                        Item i = context.GetItem(pos.BaseItem, null);
                        if (pos.Equipped != EquipSlot.None || i is Weapon || i is Armor || i is Shield)
                        {
                            equip.Add(pos);
                            equipDetailed.Add(pos.ToInfo());
                            equipDetailed2.Add(pos.ToInfo(true));
                        }
                        else
                        {
                            treasure.Add(pos);
                            treasureDetailed.Add(pos.ToInfo(true));
                        }
                    }
                    else
                    {
                        treasure.Add(pos);
                        treasureDetailed.Add(pos.ToInfo(true));
                    }
                    onUse.AddRange(pos.CollectOnUse(level, context.Player, context));
                }
                equip.Sort(delegate(Possession t1, Possession t2)
                {
                    if (t1.Hightlight && !t2.Hightlight) return -1;
                    else if (t2.Hightlight && !t1.Hightlight) return 1;
                    else
                    {
                        if (!string.Equals(t1.Equipped,EquipSlot.None,StringComparison.OrdinalIgnoreCase) && string.Equals(t2.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) return -1;
                        else if (!string.Equals(t2.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase) && string.Equals(t1.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) return 1;
                        else return (t1.ToString().CompareTo(t2.ToString()));
                    }

                });
                int chigh = 1;
                foreach (SpellcastingFeature scf in spellcasts)
                {
                    Spellcasting sc = context.Player.GetSpellcasting(scf.SpellcastingID);
                    if (sc.Highlight != null && sc.Highlight != "")
                    {
                        foreach (Spell s in sc.GetLearned(context.Player, context))
                        {
                            if (s.Name.ToLowerInvariant() == sc.Highlight.ToLowerInvariant())
                            {
                                if (trans.ContainsKey("Attack" + chigh))
                                {
                                    AttackInfo ai = context.Player.GetAttack(s,scf.SpellcastingAbility);
                                    p.SetField(trans["Attack" + chigh], s.Name);
                                    if (ai.SaveDC != "") p.SetField(trans["Attack" + chigh + "Attack"], "DC " + ai.SaveDC);
                                    else p.SetField(trans["Attack" + chigh + "Attack"], PlusMinus(ai.AttackBonus));
                                    if (trans.ContainsKey("Attack" + chigh + "DamageType"))
                                    {
                                        p.SetField(trans["Attack" + chigh + "Damage"], ai.Damage);
                                        p.SetField(trans["Attack" + chigh + "DamageType"], ai.DamageType);
                                    }
                                    else
                                    {
                                        p.SetField(trans["Attack" + chigh + "Damage"], ai.Damage + " " + ai.DamageType);
                                    }
                                    chigh++;
                                }
                            }
                        }
                    }
                }
                foreach (ModifiedSpell s in bonusspells)
                {
                    if (Utils.Matches(context, s, "Attack or Save", null))
                    {
                        if (trans.ContainsKey("Attack" + chigh))
                        {
                            AttackInfo ai = context.Player.GetAttack(s, s.differentAbility);
                            p.SetField(trans["Attack" + chigh], s.Name);
                            if (ai.SaveDC != "") p.SetField(trans["Attack" + chigh + "Attack"], "DC " + ai.SaveDC);
                            else p.SetField(trans["Attack" + chigh + "Attack"], PlusMinus(ai.AttackBonus));
                            if (trans.ContainsKey("Attack" + chigh + "DamageType"))
                            {
                                p.SetField(trans["Attack" + chigh + "Damage"], ai.Damage);
                                p.SetField(trans["Attack" + chigh + "DamageType"], ai.DamageType);
                            }
                            else
                            {
                                p.SetField(trans["Attack" + chigh + "Damage"], ai.Damage + " " + ai.DamageType);
                            }
                            chigh++;
                        }
                    }
                }
                foreach (Possession pos in equip)
                {
                    AttackInfo ai = context.Player.GetAttack(pos);
                    if (ai != null)
                    {
                        if (trans.ContainsKey("Attack" + chigh))
                        {
                            p.SetField(trans["Attack" + chigh], pos.ToString());
                            if (ai.SaveDC != "") p.SetField(trans["Attack" + chigh + "Attack"], "DC "+ai.SaveDC);
                            else p.SetField(trans["Attack" + chigh + "Attack"], PlusMinus(ai.AttackBonus));
                            if (trans.ContainsKey("Attack" + chigh + "DamageType"))
                            {
                                p.SetField(trans["Attack" + chigh + "Damage"], ai.Damage);
                                p.SetField(trans["Attack" + chigh + "DamageType"], ai.DamageType);
                            }
                            else
                            {
                                p.SetField(trans["Attack" + chigh + "Damage"], ai.Damage + " " + ai.DamageType);
                            }
                            chigh++;
                        }
                    }
                }
                List<string> usable = new List<string>();
                //if (pdf.PreserveEdit || true)
                //{
                if (trans.ContainsKey("RaceBackgroundFeatures"))
                {
                    List<string> feats = new List<string>();
                    foreach (Feature f in onUse) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name))
                        {
                            if (trans.ContainsKey("Treasure") || trans.ContainsKey("Usable")) usable.Add(f.ShortDesc());
                            else feats.Add(f.ShortDesc());
                        }
                    foreach (Feature f in context.Player.GetBackgroundFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                    foreach (Feature f in context.Player.GetRaceFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                    p.SetField(trans["RaceBackgroundFeatures"], String.Join("\n", feats));
                    List<string> feats2 = new List<string>();
                    foreach (Feature f in context.Player.GetClassFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name)) feats2.Add(f.ShortDesc());
                    foreach (Feature f in context.Player.GetCommonFeaturesAndFeats()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name)) feats2.Add(f.ShortDesc());
                    if (trans.ContainsKey("Features")) p.SetField(trans["Features"], String.Join("\n", feats2));
                } else if (trans.ContainsKey("Features"))
                {
                    List<string> feats = new List<string>();
                    foreach (Feature f in onUse) if (!f.Hidden)
                        {
                            if (trans.ContainsKey("Treasure") || trans.ContainsKey("Usable")) usable.Add(f.ShortDesc());
                            else feats.Add(f.ShortDesc());
                        }
                    foreach (Feature f in context.Player.GetBackgroundFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                    foreach (Feature f in context.Player.GetRaceFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                    foreach (Feature f in context.Player.GetClassFeatures()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                    foreach (Feature f in context.Player.GetCommonFeaturesAndFeats()) if (!f.Hidden && f.Name != null && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                    p.SetField(trans["Features"], String.Join("\n", feats));
                }
                //}
                //else
                //{
                //    var helveticaBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6);
                //    var helveticaRegular = FontFactory.GetFont(FontFactory.HELVETICA, 6);
                //    Phrase feats = new Phrase();
                //    foreach (Feature f in context.Player.GetBackgroundFeatures()) if (!f.Hidden)
                //        {
                //            feats.Add(new Chunk(f.Name + ": ", helveticaBold));
                //            feats.Add(new Chunk(f.Text + "\n", helveticaRegular));
                //        }
                //    foreach (Feature f in context.Player.GetRaceFeatures()) if (!f.Hidden)
                //        {
                //            feats.Add(new Chunk(f.Name + ": ", helveticaBold));
                //            feats.Add(new Chunk(f.Text + "\n", helveticaRegular));
                //        }
                //    foreach (AcroFields.FieldPosition pos in p.GetFieldPositions(trans["RaceBackgroundFeatures"]))
                //    {
                //        ColumnText currentColumnText = new ColumnText(p.GetOverContent(pos.page));
                //        currentColumnText.SetSimpleColumn(feats, pos.position.Left, pos.position.Bottom, pos.position.Right, pos.position.Top, 13, Element.ALIGN_LEFT);
                //        currentColumnText.Go();
                //    } 
                //    Phrase feats2 = new Phrase();
                //    foreach (Feature f in context.Player.GetClassFeatures()) if (!f.Hidden)
                //        {
                //            feats.Add(new Chunk(f.Name + ": ", helveticaBold));
                //            feats.Add(new Chunk(f.Text + "\n", helveticaRegular));
                //        }
                //    foreach (Feature f in context.Player.GetCommonFeaturesAndFeats()) if (!f.Hidden)
                //        {
                //            feats.Add(new Chunk(f.Name + ": ", helveticaBold));
                //            feats.Add(new Chunk(f.Text + "\n", helveticaRegular));
                //        }
                //    foreach (AcroFields.FieldPosition pos in p.GetFieldPositions(trans["Features"]))
                //    {
                //        ColumnText currentColumnText = new ColumnText(p.GetOverContent(pos.page));
                //        currentColumnText.SetSimpleColumn(feats2, pos.position.Left, pos.position.Bottom, pos.position.Right, pos.position.Top, 13, Element.ALIGN_LEFT);
                //        currentColumnText.Go();
                //    }
                //}
                bool addUsableToTreasure = false;
                if (trans.ContainsKey("Usable")) p.SetField(trans["Usable"], String.Join("\n", usable));
                else addUsableToTreasure = usable.Count > 0;
                if (trans.ContainsKey("CP") && trans.ContainsKey("GP") && trans.ContainsKey("SP") && trans.ContainsKey("EP") && trans.ContainsKey("PP"))
                {
                    if (trans.ContainsKey("Equipment")) p.SetField(trans["Equipment"], String.Join("\n", equipDetailed));
                    if (trans.ContainsKey("EquipmentShort")) p.SetField(trans["EquipmentShort"], String.Join(", ", equip));
                    if (trans.ContainsKey("EquipmentDetailed")) p.SetField(trans["EquipmentDetailed"], String.Join("\n", equipDetailed2));
                    if (trans.ContainsKey("Treasure")) p.SetField(trans["Treasure"], String.Join(", ", treasure) + (addUsableToTreasure ? "\n" + String.Join("\n", usable) : ""));
                    p.SetField(trans["CP"], money.cp.ToString());
                    if (trans.ContainsKey("SP")) p.SetField(trans["SP"], money.sp.ToString());
                    if (trans.ContainsKey("EP")) p.SetField(trans["EP"], money.ep.ToString());
                    if (trans.ContainsKey("GP")) p.SetField(trans["GP"], money.gp.ToString());
                    if (trans.ContainsKey("PP")) p.SetField(trans["PP"], money.pp.ToString());
                }
                else if (trans.ContainsKey("GP"))
                {
                    if (trans.ContainsKey("Equipment")) p.SetField(trans["Equipment"], String.Join("\n", equipDetailed));
                    if (trans.ContainsKey("EquipmentShort")) p.SetField(trans["EquipmentShort"], String.Join(", ", equip));
                    if (trans.ContainsKey("EquipmentDetailed")) p.SetField(trans["EquipmentDetailed"], String.Join("\n", equipDetailed2));
                    if (trans.ContainsKey("Treasure")) p.SetField(trans["Treasure"], String.Join(", ", treasure) + (addUsableToTreasure ? "\n" + String.Join("\n", usable) : ""));
                    p.SetField(trans["GP"], money.ToGold());
                }
                else if (trans.ContainsKey("EquipmentShort"))
                {
                    p.SetField(trans["EquipmentShort"], String.Join(", ", equip) + "\n" + money.ToString());
                    if (trans.ContainsKey("EquipmentDetailed")) p.SetField(trans["EquipmentDetailed"], String.Join("\n", equipDetailed2));
                    if (trans.ContainsKey("Equipment")) p.SetField(trans["Equipment"], String.Join("\n", equipDetailed));
                    if (trans.ContainsKey("Treasure")) p.SetField(trans["Treasure"], String.Join(", ", treasure) + (addUsableToTreasure ? "\n" + String.Join("\n", usable) : ""));
                }
                else if (trans.ContainsKey("Equipment"))
                {
                    p.SetField(trans["Equipment"], String.Join("\n", equipDetailed) + "\n" + money.ToString());
                    if (trans.ContainsKey("EquipmentDetailed")) p.SetField(trans["EquipmentDetailed"], String.Join("\n", equipDetailed2));
                    if (trans.ContainsKey("Treasure")) p.SetField(trans["Treasure"], String.Join(", ", treasure) + (addUsableToTreasure ? "\n" + String.Join("\n", usable) : ""));
                } else if (trans.ContainsKey("EquipmentDetailed"))
                {
                    p.SetField(trans["EquipmentDetailed"], String.Join("\n", equipDetailed2) + "\n" + money.ToString());
                    if (trans.ContainsKey("Treasure")) p.SetField(trans["Treasure"], String.Join(", ", treasure) + (addUsableToTreasure ? "\n" + String.Join("\n", usable) : ""));
                }
                else if (trans.ContainsKey("Treasure"))
                {
                    p.SetField(trans["Treasure"], String.Join(", ", treasure) + (addUsableToTreasure ? "\n" + String.Join("\n", usable) : "") + "\n" + money.ToString());
                }
                else
                {
                    treasureDetailed.Add(money.ToString());
                }
                if (trans.ContainsKey("TreasureDetailed")) p.SetField(trans["TreasureDetailed"], String.Join("\n", treasureDetailed));
                using (IPDFSheet sheet = pdf.CreateSheet())
                {
                    sheet.Add(p);
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
                            }
                            else if (scf.Preparation == PreparationMode.Spellbook)
                            {
                                Available.AddRange(sc.GetSpellbook(context.Player, context));
                                Prepared.AddRange(sc.GetPrepared(context.Player, context));
                            }
                            else
                            {
                                Prepared.AddRange(sc.GetPrepared(context.Player, context));
                            }
                            Prepared.AddRange(sc.GetLearned(context.Player, context));
                            Available.AddRange(Prepared);
                            spellbook.AddRange(Available);
                            List<Spell> Shown = new List<Spell>(Available.Distinct());
                            Shown.Sort(delegate(Spell t1, Spell t2)
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
                                    FillBasicFields(spelltrans, sp, context);
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

                    if (pdf.IncludeLog && LogFile != null && LogFile != "" && (logtrans.ContainsKey("Title1") || logtrans.ContainsKey("XP1")))
                    {
                        Queue<JournalEntry> entries = new Queue<JournalEntry>(context.Player.ComplexJournal);
                        


                        Price gold = context.Player.GetMoney(false);
                        int xp = context.Player.XP;
                        int renown = 0;
                        int downtime = 0;
                        int magic = 0;
                        int sheetCount = 0;
                        while (entries.Count > 0)
                        {
                            int counter = 1;
                            using (IPDFEditor lp = await pdf.CreateEditor(LogFile)) 
                            {
                                sheetCount++;
                                FillBasicFields(logtrans, lp, context);
                                if (logtrans.ContainsKey("Sheet")) lp.SetField(logtrans["Sheet"], sheetCount.ToString());
                                while (entries.Count > 0 && (logtrans.ContainsKey("Title" + counter) || logtrans.ContainsKey("XP" + counter)))
                                {
                                    JournalEntry entry = entries.Dequeue();
                                    if (entry.InSheet)
                                    {
                                        if (logtrans.ContainsKey("Title" + counter)) lp.SetField(logtrans["Title" + counter], entry.Title);
                                        if (logtrans.ContainsKey("Session" + counter)) lp.SetField(logtrans["Session" + counter], entry.Session);
                                        if (logtrans.ContainsKey("Date" + counter)) lp.SetField(logtrans["Date" + counter], entry.Added.ToString());
                                        if (logtrans.ContainsKey("DM" + counter)) lp.SetField(logtrans["DM" + counter], entry.DM);
                                        if (entry.Text != null)
                                        {
                                            if (logtrans.ContainsKey("Notes" + counter)) lp.SetField(logtrans["Notes" + counter], entry.Text);
                                            else if (logtrans.ContainsKey("Notes" + counter + "Line1"))
                                            {
                                                int line = 1;
                                                Queue<string> lines = new Queue<string>(entry.Text.Split('\n'));
                                                while (lines.Count > 0 && logtrans.ContainsKey("Notes" + counter + "Line" + (line + 1)))
                                                {
                                                    lp.SetField(logtrans["Notes" + counter + "Line" + line], lines.Dequeue());
                                                    line++;
                                                }
                                                lp.SetField(logtrans["Notes" + counter + "Line" + line], string.Join(" ", lines));
                                            }
                                        }
                                        if (logtrans.ContainsKey("XPStart" + counter)) lp.SetField(logtrans["XPStart" + counter], xp.ToString());
                                        if (logtrans.ContainsKey("GoldStart" + counter)) lp.SetField(logtrans["GoldStart" + counter], gold.ToGold());
                                        if (logtrans.ContainsKey("DowntimeStart" + counter)) lp.SetField(logtrans["DowntimeStart" + counter], downtime.ToString());
                                        if (logtrans.ContainsKey("RenownStart" + counter)) lp.SetField(logtrans["RenownStart" + counter], renown.ToString());
                                        if (logtrans.ContainsKey("MagicItemsStart" + counter)) lp.SetField(logtrans["MagicItemsStart" + counter], magic.ToString());

                                        if (logtrans.ContainsKey("XP" + counter)) lp.SetField(logtrans["XP" + counter], entry.XP.ToString());
                                        if (logtrans.ContainsKey("Gold" + counter)) lp.SetField(logtrans["Gold" + counter], entry.GetMoney());
                                        if (logtrans.ContainsKey("Downtime" + counter)) lp.SetField(logtrans["Downtime" + counter], PlusMinus(entry.Downtime));
                                        if (logtrans.ContainsKey("Renown" + counter)) lp.SetField(logtrans["Renown" + counter], PlusMinus(entry.Renown));
                                        if (logtrans.ContainsKey("MagicItems" + counter)) lp.SetField(logtrans["MagicItems" + counter], PlusMinus(entry.MagicItems));
                                    }
                                    xp += entry.XP;
                                    gold.pp += entry.PP;
                                    gold.gp += entry.GP;
                                    gold.sp += entry.SP;
                                    gold.ep += entry.EP;
                                    gold.cp += entry.CP;
                                    renown += entry.Renown;
                                    magic += entry.MagicItems;
                                    downtime += entry.Downtime;
                                    if (entry.InSheet)
                                    {
                                        if (logtrans.ContainsKey("XPEnd" + counter)) lp.SetField(logtrans["XPEnd" + counter], xp.ToString());
                                        if (logtrans.ContainsKey("GoldEnd" + counter)) lp.SetField(logtrans["GoldEnd" + counter], gold.ToGold());
                                        if (logtrans.ContainsKey("DowntimeEnd" + counter)) lp.SetField(logtrans["DowntimeEnd" + counter], downtime.ToString());
                                        if (logtrans.ContainsKey("RenownEnd" + counter)) lp.SetField(logtrans["RenownEnd" + counter], renown.ToString());
                                        if (logtrans.ContainsKey("MagicItemsEnd" + counter)) lp.SetField(logtrans["MagicItemsEnd" + counter], magic.ToString());
                                        counter++;
                                    }

                                }
                                if (counter > 1) sheet.Add(lp);
                            }
                        }
                    }

                    if (pdf.IncludeSpellbook && SpellbookFile != null && SpellbookFile != "" && (booktrans.ContainsKey("Name1") || booktrans.ContainsKey("Description1")))
                    {
                        List<SpellModifyFeature> mods = (from f in context.Player.GetFeatures() where f is SpellModifyFeature select f as SpellModifyFeature).ToList();
                        Queue<Spell> entries = new Queue<Spell>(spellbook.OrderBy(s => s.Name).Distinct(new SpellEqualityComparer()));
                        int sheetCount = 0;
                        while (entries.Count > 0)
                        {
                            int counter = 1;
                            using (IPDFEditor sbp = await pdf.CreateEditor(SpellbookFile))
                            {
                                sheetCount++;
                                FillBasicFields(booktrans, sbp, context);
                                if (booktrans.ContainsKey("Sheet")) sbp.SetField(booktrans["Sheet"], sheetCount.ToString());
                                while (entries.Count > 0 && (booktrans.ContainsKey("Name" + counter) || booktrans.ContainsKey("Description" + counter)))
                                {
                                    Spell entry = entries.Dequeue();
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
                                    StringBuilder add = new StringBuilder();
                                    foreach (Description d in entry.Descriptions)
                                    {
                                        add.Append(d.Name.ToUpperInvariant()).Append(": ").Append(d.Text).AppendLine();
                                        if (d is ListDescription) foreach (Names n in (d as ListDescription).Names) add.Append(n.Title).Append(": ").Append(String.Join(", ", n.ListOfNames)).AppendLine();
                                        if (d is TableDescription) foreach (TableEntry tr in (d as TableDescription).Entries) add.Append(tr.ToFullString()).AppendLine();
                                    }
                                    if (booktrans.ContainsKey("AdditionDescription" + counter)) sbp.SetField(booktrans["AdditionDescription" + counter], add.ToString());
                                    else description.AppendLine().AppendLine().Append(add.ToString());
                                    StringBuilder Modifiers = new StringBuilder();
                                    foreach (SpellModifyFeature m in mods.Where(f => Utils.Matches(context, entry, ((SpellModifyFeature)f).Spells, null)))
                                    {
                                        Modifiers.Append(m.Name.ToUpperInvariant()).Append(": ").Append(m.Text).AppendLine();
                                    }
                                    if (booktrans.ContainsKey("Modifiers" + counter)) sbp.SetField(booktrans["Modifiers" + counter], Modifiers.ToString());
                                    else description.AppendLine().Append(Modifiers.ToString());
                                    if (booktrans.ContainsKey("Source" + counter)) sbp.SetField(booktrans["Source" + counter], entry.Source);
                                    else description.AppendLine().Append("Source: ").Append(entry.Source).AppendLine();
                                    if (booktrans.ContainsKey("Description" + counter)) sbp.SetField(booktrans["Description" + counter], description.ToString());
                                    counter++;
                                }
                                if (counter > 1) sheet.Add(sbp);
                            }
                        }
                    }
                    if (pdf.IncludeActions && ActionsFile != null && ActionsFile != "" && (actiontrans.ContainsKey("Action1Name") || actiontrans.ContainsKey("Action1Text")))
                    {
                        Queue<ActionInfo> entries = new Queue<ActionInfo>(context.Player.GetActions());
                        int sheetCount = 0;
                        String file = ActionsFile;
                        while (entries.Count > 0)
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
                                FillBasicFields(actiontrans, abp, context);
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
                                if (counter > 1) sheet.Add(abp);
                            }
                        }
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
                                } else
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
                                FillBasicFields(monstertrans, mp, context);
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
                                    if (t.Count >= 1) {
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
                                        mp.SetField(monstertrans["StrMod" + counter], (entry.Monster.Strength / 2 - 5).ToString() );
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

                                    if (monstertrans.ContainsKey("Saves" + counter)) mp.SetField(monstertrans["Saves" + counter], string.Join(", ", entry.Monster.SaveBonus.Select(s=>s.ToString(entry.Monster))));
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

        public static StringBuilder AppendIfContent(StringBuilder s, string v)
        {
            if (s.Length > 0) s.Append(v);
            return s;
        }

        private static string Type(ActionType type)
        {
            switch(type)
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
            bool v = false, s = false, m = false;
            string mat = "";
            for (int i = kw.Count - 1; i >= 0; i--)
            {
                if (kw[i].Name.Equals("verbal", StringComparison.OrdinalIgnoreCase))
                {
                    v = true;
                    kw.RemoveAt(i);
                } else if (kw[i].Name.Equals("somatic", StringComparison.OrdinalIgnoreCase))
                {
                    s = true;
                    kw.RemoveAt(i);
                } else  if (kw[i] is Material)
                {
                    m = true;
                    mat = mat == "" ? (kw[i] as Material).Components : mat + "; " + (kw[i] as Material).Components;
                    kw.RemoveAt(i);
                }
            }
            if (v) r.Add("V");
            if (s) r.Add("S");
            if (m) r.Add("M(" + mat + ")");
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

        private void FillBasicFields(Dictionary<string, string> trans, IPDFEditor p, BuilderContext context)
        {
            int down = 0;
            int renown = 0;
            int magic = 0;
            if (trans.ContainsKey("Renown") || trans.ContainsKey("MagicItems") || trans.ContainsKey("Downtime"))
            {
                foreach (JournalEntry je in context.Player.ComplexJournal)
                {
                    down += je.Downtime;
                    renown += je.Renown;
                    magic += je.MagicItems;
                }
            }
            if (trans.ContainsKey("Background")) p.SetField(trans["Background"], SourceInvariantComparer.NoSource(context.Player.BackgroundName));
            if (trans.ContainsKey("Race")) p.SetField(trans["Race"], context.Player.GetRaceSubName());
            if (trans.ContainsKey("PersonalityTrait")) p.SetField(trans["PersonalityTrait"], context.Player.PersonalityTrait);
            if (trans.ContainsKey("Ideal")) p.SetField(trans["Ideal"], context.Player.Ideal);
            if (trans.ContainsKey("Bond")) p.SetField(trans["Bond"], context.Player.Bond);
            if (trans.ContainsKey("Flaw")) p.SetField(trans["Flaw"], context.Player.Flaw);
            if (trans.ContainsKey("PlayerName")) p.SetField(trans["PlayerName"], context.Player.PlayerName);
            if (trans.ContainsKey("Alignment")) p.SetField(trans["Alignment"], context.Player.Alignment);
            if (trans.ContainsKey("XP")) p.SetField(trans["XP"], context.Player.GetXP().ToString());
            if (trans.ContainsKey("Age")) p.SetField(trans["Age"], context.Player.Age.ToString());
            if (trans.ContainsKey("Height")) p.SetField(trans["Height"], context.Player.Height.ToString());
            if (trans.ContainsKey("Weight")) p.SetField(trans["Weight"], context.Player.Weight.ToString() + " lb");
            if (trans.ContainsKey("Eyes")) p.SetField(trans["Eyes"], context.Player.Eyes.ToString());
            if (trans.ContainsKey("Skin")) p.SetField(trans["Skin"], context.Player.Skin.ToString());
            if (trans.ContainsKey("Hair")) p.SetField(trans["Hair"], context.Player.Hair.ToString());
            if (trans.ContainsKey("Speed")) p.SetField(trans["Speed"], context.Player.GetSpeed().ToString());
            if (trans.ContainsKey("FactionName")) p.SetField(trans["FactionName"], context.Player.FactionName);
            if (trans.ContainsKey("Backstory")) p.SetField(trans["Backstory"], context.Player.Backstory);
            if (trans.ContainsKey("Allies")) p.SetField(trans["Allies"], context.Player.Allies);
            if (trans.ContainsKey("Strength")) p.SetField(trans["Strength"], context.Player.GetStrength().ToString());
            if (trans.ContainsKey("Dexterity")) p.SetField(trans["Dexterity"], context.Player.GetDexterity().ToString());
            if (trans.ContainsKey("Constitution")) p.SetField(trans["Constitution"], context.Player.GetConstitution().ToString());
            if (trans.ContainsKey("Intelligence")) p.SetField(trans["Intelligence"], context.Player.GetIntelligence().ToString());
            if (trans.ContainsKey("Wisdom")) p.SetField(trans["Wisdom"], context.Player.GetWisdom().ToString());
            if (trans.ContainsKey("Charisma")) p.SetField(trans["Charisma"], context.Player.GetCharisma().ToString());
            if (trans.ContainsKey("StrengthModifier")) p.SetField(trans["StrengthModifier"], PlusMinus(context.Player.GetStrengthMod()));
            if (trans.ContainsKey("DexterityModifier")) p.SetField(trans["DexterityModifier"], PlusMinus(context.Player.GetDexterityMod()));
            if (trans.ContainsKey("ConstitutionModifier")) p.SetField(trans["ConstitutionModifier"], PlusMinus(context.Player.GetConstitutionMod()));
            if (trans.ContainsKey("IntelligenceModifier")) p.SetField(trans["IntelligenceModifier"], PlusMinus(context.Player.GetIntelligenceMod()));
            if (trans.ContainsKey("WisdomModifier")) p.SetField(trans["WisdomModifier"], PlusMinus(context.Player.GetWisdomMod()));
            if (trans.ContainsKey("AC")) p.SetField(trans["AC"], context.Player.GetAC().ToString());
            if (trans.ContainsKey("ProficiencyBonus")) p.SetField(trans["ProficiencyBonus"], PlusMinus(context.Player.GetProficiency()));
            if (trans.ContainsKey("Initiative")) p.SetField(trans["Initiative"], PlusMinus(context.Player.GetInitiative()));
            if (trans.ContainsKey("CharismaModifier")) p.SetField(trans["CharismaModifier"], PlusMinus(context.Player.GetCharismaMod()));
            if (trans.ContainsKey("CharacterName")) p.SetField(trans["CharacterName"], context.Player.Name);
            if (trans.ContainsKey("CharacterName2")) p.SetField(trans["CharacterName2"], context.Player.Name);
            if (trans.ContainsKey("ClassLevel")) p.SetField(trans["ClassLevel"], String.Join(" | ", context.Player.GetClassesStrings()));
            if (trans.ContainsKey("DCI")) p.SetField(trans["DCI"], context.Player.DCI);
            if (trans.ContainsKey("Renown")) p.SetField(trans["Renown"], renown.ToString());
            if (trans.ContainsKey("MagicItems")) p.SetField(trans["MagicItems"], magic.ToString());
            if (trans.ContainsKey("Downtime")) p.SetField(trans["Downtime"], down.ToString());
            if (trans.ContainsKey("FactionRank")) p.SetField(trans["FactionRank"], context.Player.FactionRank);
        }

        private string PlusMinus(int value)
        {
            if (value > 0) return "+" + value;
            if (value == 0) return "--";
            return value.ToString();
        }
    }
}
