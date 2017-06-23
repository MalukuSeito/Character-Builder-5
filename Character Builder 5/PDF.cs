using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Character_Builder;
using OGL.Features;
using OGL;
using OGL.Spells;
using OGL.Base;
using OGL.Items;
using OGL.Keywords;
using OGL.Descriptions;

namespace Character_Builder_5
{
    public class PDF
    {
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(PDF));
        public String Name { get; set; }
        public String File { get; set; }
        public String SpellFile { get; set; }
        public String LogFile { get; set; }
        public String SpellbookFile { get; set; }
        public List<PDFField> Fields = new List<PDFField>();
        public List<PDFField> SpellFields = new List<PDFField>();
        public List<PDFField> LogFields = new List<PDFField>();
        public List<PDFField> SpellbookFields = new List<PDFField>();
        public static PDF Load(String file)
        {
            using (TextReader reader = new StreamReader(file))
            {
                PDF p = (PDF)serializer.Deserialize(reader);
                p.File = ConfigManager.Fullpath(Path.GetDirectoryName(file), p.File);
                p.SpellFile = ConfigManager.Fullpath(Path.GetDirectoryName(file), p.SpellFile);
                return p;
            }
        }
        public void Save(String file)
        {
            using (TextWriter writer = new StreamWriter(file)) serializer.Serialize(writer, this);
        }
        public void export(FileStream fs, bool preserveEdit, bool includeResources, bool log, bool spell)
        {
            Dictionary<String, String> trans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> spelltrans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> logtrans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> booktrans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (PDFField pf in Fields) trans.Add(pf.Name, pf.Field);
            foreach (PDFField pf in SpellFields) spelltrans.Add(pf.Name, pf.Field);
            foreach (PDFField pf in LogFields) logtrans.Add(pf.Name, pf.Field);
            foreach (PDFField pf in SpellbookFields) booktrans.Add(pf.Name, pf.Field);
            Dictionary<string, bool> hiddenfeats = Player.current.HiddenFeatures.ToDictionary(f => f, f => true, StringComparer.OrdinalIgnoreCase);
            List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>(from f in Player.current.getFeatures() where f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID != "MULTICLASS" select (SpellcastingFeature)f);
            List<Spell> spellbook = new List<Spell>();
            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfReader sheet = new PdfReader(File))
                {
                    if (preserveEdit) sheet.RemoveUsageRights();
                    using (PdfStamper p = new PdfStamper(sheet, ms))
                    {
                        fillBasicFields(trans, p);
                        String attacks = "";
                        String resources = "";
                        if (trans.ContainsKey("Resources"))
                        {
                            resources = String.Join("\n", Player.current.getResourceInfo(includeResources).Values);
                        }
                        else
                        {
                            if (attacks != "") attacks += "\n";
                            attacks += String.Join("\n", Player.current.getResourceInfo(includeResources).Values);
                        }
                        List<ModifiedSpell> bonusspells=new List<ModifiedSpell>(Player.current.getBonusSpells());
                        if (includeResources)
                        {
                            Dictionary<string, int> bsres = Player.current.getResources();
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
                            p.AcroFields.SetField(trans["BonusSpells"], String.Join("\n", bonusspells));
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
                            p.AcroFields.SetField(trans["Resources"], resources);
                        }
                        if (trans.ContainsKey("Attacks")) p.AcroFields.SetField(trans["Attacks"], attacks);
                        List<HitDie> hd = Player.current.getHitDie();
                        if (trans.ContainsKey("HitDieTotal")) p.AcroFields.SetField(trans["HitDieTotal"], String.Join(", ", from h in hd select h.Total()));
                        int maxhp=Player.current.getHitpointMax();
                        if (trans.ContainsKey("MaxHP")) p.AcroFields.SetField(trans["MaxHP"], maxhp.ToString());
                        if (includeResources)
                        {
                            if (trans.ContainsKey("CurrentHP")) p.AcroFields.SetField(trans["CurrentHP"], (maxhp+Player.current.CurrentHPLoss).ToString());
                            if (trans.ContainsKey("TempHP")) p.AcroFields.SetField(trans["TempHP"], Player.current.TempHP.ToString());
                            for (int d = 1; d <= Player.current.FailedDeathSaves; d++) if (trans.ContainsKey("DeathSaveFail" + d)) p.AcroFields.SetField(trans["DeathSaveFail" + d], "Yes");
                                else break;
                            for (int d = 1; d <= Player.current.SuccessDeathSaves; d++) if (trans.ContainsKey("DeathSaveSuccess" + d)) p.AcroFields.SetField(trans["DeathSaveSuccess" + d], "Yes");
                                else break;
                            if (trans.ContainsKey("HitDie")) p.AcroFields.SetField(trans["HitDie"], String.Join(", ", hd));
                            if (trans.ContainsKey("Inspiration")) if (Player.current.Inspiration) p.AcroFields.SetField(trans["Inspiration"], "Yes");
                        }
                        if (Player.current.Portrait != null)
                        {
                            foreach (AcroFields.FieldPosition pos in p.AcroFields.GetFieldPositions(trans["Portrait"]))
                            {
                                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Player.current.Portrait, BaseColor.WHITE);
                                img.ScaleToFit(pos.position.Width, pos.position.Height);
                                img.SetAbsolutePosition(pos.position.Left + pos.position.Width / 2 - img.ScaledWidth / 2, pos.position.Bottom + pos.position.Height / 2 - img.ScaledHeight / 2);
                                p.GetOverContent(pos.page).AddImage(img);
                            }
                        }
                        if (Player.current.FactionImage != null)
                        {
                            foreach (AcroFields.FieldPosition pos in p.AcroFields.GetFieldPositions(trans["FactionPortrait"]))
                            {
                                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Player.current.FactionImage, BaseColor.WHITE);
                                img.ScaleToFit(pos.position.Width, pos.position.Height);
                                img.SetAbsolutePosition(pos.position.Left + pos.position.Width / 2 - img.ScaledWidth / 2, pos.position.Bottom + pos.position.Height / 2 - img.ScaledHeight / 2);
                                p.GetOverContent(pos.page).AddImage(img);
                            }
                        }
                        foreach (Skill s in Skill.skills.Values) if (trans.ContainsKey("Passive" + s.Name)) p.AcroFields.SetField(trans["Passive" + s.Name], Player.current.getPassiveSkill(s).ToString());
                        foreach (Skill s in Player.current.getSkillProficiencies()) p.AcroFields.SetField(trans[s.Name + "Proficiency"], "Yes");
                        foreach (SkillInfo s in Player.current.getSkills()) p.AcroFields.SetField(trans[s.Skill.Name], Form1.plusMinus(s.Roll));
                        Ability saveprof = Player.current.getSaveProficiencies();
                        if (saveprof.HasFlag(Ability.Strength)) p.AcroFields.SetField(trans["StrengthSaveProficiency"], "Yes");
                        if (saveprof.HasFlag(Ability.Dexterity)) p.AcroFields.SetField(trans["DexteritySaveProficiency"], "Yes");
                        if (saveprof.HasFlag(Ability.Constitution)) p.AcroFields.SetField(trans["ConstitutionSaveProficiency"], "Yes");
                        if (saveprof.HasFlag(Ability.Intelligence)) p.AcroFields.SetField(trans["IntelligenceSaveProficiency"], "Yes");
                        if (saveprof.HasFlag(Ability.Wisdom)) p.AcroFields.SetField(trans["WisdomSaveProficiency"], "Yes");
                        if (saveprof.HasFlag(Ability.Charisma)) p.AcroFields.SetField(trans["CharismaSaveProficiency"], "Yes");
                        foreach (KeyValuePair<Ability, int> v in Player.current.getSaves()) p.AcroFields.SetField(trans[v.Key.ToString("F") + "Save"], Form1.plusMinus(v.Value));
                        List<String> profs = new List<string>();
                        profs.Add(String.Join(", ", Player.current.getLanguages()));
                        profs.Add(String.Join(", ", Player.current.getToolProficiencies()));
                        profs.Add(String.Join("; ", Player.current.getToolKWProficiencies()));
                        //                        foreach (List<Keyword> kws in Player.current.getToolKWProficiencies()) profs.Add("Any "+String.Join(", ", kws));
                        profs.Add(String.Join(", ", Player.current.getOtherProficiencies()));
                        profs.RemoveAll(t => t == "");
                        p.AcroFields.SetField(trans["Proficiencies"], String.Join("\n", profs));
                        Price money = Player.current.getMoney();
                        int level=Player.current.getLevel();
                        // TODO Proper Items:
                        List<Possession> equip=new List<Possession>();
                        List<Possession> treasure=new List<Possession>();
                        List<Feature> onUse=new List<Feature>();
                        foreach (Possession pos in Player.current.getItemsAndPossessions()) {
                            if (pos.BaseItem!=null && pos.BaseItem != "") {
                                Item i=Item.Get(pos.BaseItem, null);
                                if (pos.Equipped != EquipSlot.None || i is Weapon || i is Armor || i is Shield) equip.Add(pos);
                                else treasure.Add(pos);
                            }
                                else treasure.Add(pos);
                            onUse.AddRange(pos.CollectOnUse(level, Player.current));
                        }
                        equip.Sort(delegate(Possession t1, Possession t2)
                        {
                            if (t1.Hightlight && !t2.Hightlight) return -1;
                            else if (t2.Hightlight && !t1.Hightlight) return 1;
                            else
                            {
                                if (!string.Equals(t1.Equipped,EquipSlot.None,StringComparison.InvariantCultureIgnoreCase) && string.Equals(t2.Equipped, EquipSlot.None, StringComparison.InvariantCultureIgnoreCase)) return -1;
                                else if (!string.Equals(t2.Equipped, EquipSlot.None, StringComparison.InvariantCultureIgnoreCase) && string.Equals(t1.Equipped, EquipSlot.None, StringComparison.InvariantCultureIgnoreCase)) return 1;
                                else return (t1.ToString().CompareTo(t2.ToString()));
                            }

                        });
                        if (trans.ContainsKey("CP"))
                        {
                            if (trans.ContainsKey("Equipment")) p.AcroFields.SetField(trans["Equipment"], String.Join("\n", equip));
                            p.AcroFields.SetField(trans["CP"], money.cp.ToString());
                            if (trans.ContainsKey("SP")) p.AcroFields.SetField(trans["SP"], money.sp.ToString());
                            if (trans.ContainsKey("EP")) p.AcroFields.SetField(trans["EP"], money.ep.ToString());
                            if (trans.ContainsKey("GP")) p.AcroFields.SetField(trans["GP"], money.gp.ToString());
                            if (trans.ContainsKey("PP")) p.AcroFields.SetField(trans["PP"], money.pp.ToString());
                        }
                        else if (trans.ContainsKey("GP"))
                        {
                            if (trans.ContainsKey("Equipment")) p.AcroFields.SetField(trans["Equipment"], String.Join("\n", equip));
                            p.AcroFields.SetField(trans["GP"], money.toGold());
                        }
                        else if (trans.ContainsKey("Equipment"))
                        {
                            p.AcroFields.SetField(trans["Equipment"], String.Join("\n", equip) + "\n" + money.ToString());
                            //TODO .CollectOnUseFeatures() foreach Possession
                        }
                        int chigh = 1;
                        foreach (SpellcastingFeature scf in spellcasts)
                        {
                            Spellcasting sc = Player.current.getSpellcasting(scf.SpellcastingID);
                            if (sc.Highlight != null && sc.Highlight != "")
                            {
                                foreach (Spell s in sc.getLearned())
                                {
                                    if (s.Name.ToLowerInvariant() == sc.Highlight.ToLowerInvariant())
                                    {
                                        if (trans.ContainsKey("Attack" + chigh))
                                        {
                                            AttackInfo ai = Player.current.getAttack(s,scf.SpellcastingAbility);
                                            p.AcroFields.SetField(trans["Attack" + chigh], s.Name);
                                            if (ai.SaveDC != "") p.AcroFields.SetField(trans["Attack" + chigh + "Attack"], "DC " + ai.SaveDC);
                                            else p.AcroFields.SetField(trans["Attack" + chigh + "Attack"], Form1.plusMinus(ai.AttackBonus));
                                            if (trans.ContainsKey("Attack" + chigh + "DamageType"))
                                            {
                                                p.AcroFields.SetField(trans["Attack" + chigh + "Damage"], ai.Damage);
                                                p.AcroFields.SetField(trans["Attack" + chigh + "DamageType"], ai.DamageType);
                                            }
                                            else
                                            {
                                                p.AcroFields.SetField(trans["Attack" + chigh + "Damage"], ai.Damage + " " + ai.DamageType);
                                            }
                                            chigh++;
                                        }
                                    }
                                }
                            }
                        }
                        foreach (ModifiedSpell s in bonusspells)
                        {
                            if (Utils.matches(s, "Attack or Save", null))
                            {
                                if (trans.ContainsKey("Attack" + chigh))
                                {
                                    AttackInfo ai = Player.current.getAttack(s, s.differentAbility);
                                    p.AcroFields.SetField(trans["Attack" + chigh], s.Name);
                                    if (ai.SaveDC != "") p.AcroFields.SetField(trans["Attack" + chigh + "Attack"], "DC " + ai.SaveDC);
                                    else p.AcroFields.SetField(trans["Attack" + chigh + "Attack"], Form1.plusMinus(ai.AttackBonus));
                                    if (trans.ContainsKey("Attack" + chigh + "DamageType"))
                                    {
                                        p.AcroFields.SetField(trans["Attack" + chigh + "Damage"], ai.Damage);
                                        p.AcroFields.SetField(trans["Attack" + chigh + "DamageType"], ai.DamageType);
                                    }
                                    else
                                    {
                                        p.AcroFields.SetField(trans["Attack" + chigh + "Damage"], ai.Damage + " " + ai.DamageType);
                                    }
                                    chigh++;
                                }
                            }
                        }
                        foreach (Possession pos in equip)
                        {
                            AttackInfo ai = Player.current.getAttack(pos);
                            if (ai != null)
                            {
                                if (trans.ContainsKey("Attack" + chigh))
                                {
                                    p.AcroFields.SetField(trans["Attack" + chigh], pos.ToString());
                                    if (ai.SaveDC != "") p.AcroFields.SetField(trans["Attack" + chigh + "Attack"], "DC "+ai.SaveDC);
                                    else p.AcroFields.SetField(trans["Attack" + chigh + "Attack"], Form1.plusMinus(ai.AttackBonus));
                                    if (trans.ContainsKey("Attack" + chigh + "DamageType"))
                                    {
                                        p.AcroFields.SetField(trans["Attack" + chigh + "Damage"], ai.Damage);
                                        p.AcroFields.SetField(trans["Attack" + chigh + "DamageType"], ai.DamageType);
                                    }
                                    else
                                    {
                                        p.AcroFields.SetField(trans["Attack" + chigh + "Damage"], ai.Damage + " " + ai.DamageType);
                                    }
                                    chigh++;
                                }
                            }
                        }
                        if (trans.ContainsKey("Treasure")) p.AcroFields.SetField(trans["Treasure"], String.Join("\n", treasure));
                        if (preserveEdit || true)
                        {
                            if (trans.ContainsKey("RaceBackgroundFeatures"))
                            {
                                List<string> feats = new List<string>();
                                foreach (Feature f in onUse) if (!f.Hidden) feats.Add(f.ShortDesc());
                                foreach (Feature f in Player.current.getBackgroundFeatures()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                                foreach (Feature f in Player.current.getRaceFeatures()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                                p.AcroFields.SetField(trans["RaceBackgroundFeatures"], String.Join("\n", feats));
                                List<string> feats2 = new List<string>();
                                foreach (Feature f in Player.current.getClassFeatures()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats2.Add(f.ShortDesc());
                                foreach (Feature f in Player.current.getCommonFeaturesAndFeats()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats2.Add(f.ShortDesc());
                                if (trans.ContainsKey("Features")) p.AcroFields.SetField(trans["Features"], String.Join("\n", feats2));
                            } else if (trans.ContainsKey("Features"))
                            {
                                List<string> feats = new List<string>();
                                foreach (Feature f in onUse) if (!f.Hidden) feats.Add(f.ShortDesc());
                                foreach (Feature f in Player.current.getBackgroundFeatures()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                                foreach (Feature f in Player.current.getRaceFeatures()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                                foreach (Feature f in Player.current.getClassFeatures()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                                foreach (Feature f in Player.current.getCommonFeaturesAndFeats()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                                p.AcroFields.SetField(trans["Features"], String.Join("\n", feats));
                            }
                        }
                        else
                        {
                            var helveticaBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6);
                            var helveticaRegular = FontFactory.GetFont(FontFactory.HELVETICA, 6);
                            Phrase feats = new Phrase();
                            foreach (Feature f in Player.current.getBackgroundFeatures()) if (!f.Hidden)
                                {
                                    feats.Add(new Chunk(f.Name + ": ", helveticaBold));
                                    feats.Add(new Chunk(f.Text + "\n", helveticaRegular));
                                }
                            foreach (Feature f in Player.current.getRaceFeatures()) if (!f.Hidden)
                                {
                                    feats.Add(new Chunk(f.Name + ": ", helveticaBold));
                                    feats.Add(new Chunk(f.Text + "\n", helveticaRegular));
                                }
                            foreach (AcroFields.FieldPosition pos in p.AcroFields.GetFieldPositions(trans["RaceBackgroundFeatures"]))
                            {
                                ColumnText currentColumnText = new ColumnText(p.GetOverContent(pos.page));
                                currentColumnText.SetSimpleColumn(feats, pos.position.Left, pos.position.Bottom, pos.position.Right, pos.position.Top, 13, Element.ALIGN_LEFT);
                                currentColumnText.Go();
                            } 
                            Phrase feats2 = new Phrase();
                            foreach (Feature f in Player.current.getClassFeatures()) if (!f.Hidden)
                                {
                                    feats.Add(new Chunk(f.Name + ": ", helveticaBold));
                                    feats.Add(new Chunk(f.Text + "\n", helveticaRegular));
                                }
                            foreach (Feature f in Player.current.getCommonFeaturesAndFeats()) if (!f.Hidden)
                                {
                                    feats.Add(new Chunk(f.Name + ": ", helveticaBold));
                                    feats.Add(new Chunk(f.Text + "\n", helveticaRegular));
                                }
                            foreach (AcroFields.FieldPosition pos in p.AcroFields.GetFieldPositions(trans["Features"]))
                            {
                                ColumnText currentColumnText = new ColumnText(p.GetOverContent(pos.page));
                                currentColumnText.SetSimpleColumn(feats2, pos.position.Left, pos.position.Bottom, pos.position.Right, pos.position.Top, 13, Element.ALIGN_LEFT);
                                currentColumnText.Go();
                            }
                        }
                        p.FormFlattening = !preserveEdit;
                        p.Writer.CloseStream = false;
                    }
                }
                PdfReader x = new PdfReader(ms.ToArray());
                if (preserveEdit) x.RemoveUsageRights();
                Document sourceDocument = new Document(x.GetPageSizeWithRotation(1));
                using (PdfCopy pdfCopy = new PdfCopy(sourceDocument, fs))
                {
                    sourceDocument.Open();
                    for (int i = 1; i <= x.NumberOfPages; i++)
                    {
                        pdfCopy.SetPageSize(x.GetPageSize(i));
                        pdfCopy.AddPage(pdfCopy.GetImportedPage(x, i));
                    }
                    
                    foreach (SpellcastingFeature scf in spellcasts)
                    {
                        if (scf.SpellcastingID != "MULTICLASS")
                        {
                            Spellcasting sc = Player.current.getSpellcasting(scf.SpellcastingID);
                            int classlevel = Player.current.getClassLevel(scf.SpellcastingID);
                            List<int> SpellSlots = Player.current.getSpellSlots(scf.SpellcastingID);
                            List<int> UsedSpellSlots = Player.current.getUsedSpellSlots(scf.SpellcastingID);
                            List<Spell> Available = new List<Spell>();
                            List<Spell> Prepared = new List<Spell>();
                            if (scf.Preparation == PreparationMode.ClassList)
                            {
                                Available.AddRange(sc.getAdditionalClassSpells());
                                Available.AddRange(Utils.filterSpell(scf.PrepareableSpells, scf.SpellcastingID, classlevel));
                                Prepared.AddRange(sc.getPrepared());
                            }
                            else if (scf.Preparation == PreparationMode.Spellbook)
                            {
                                Available.AddRange(sc.getSpellbook());
                                Prepared.AddRange(sc.getPrepared());
                            }
                            else
                            {
                                Prepared.AddRange(sc.getPrepared());
                            }
                            Prepared.AddRange(sc.getLearned());
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
                                using (PdfReader spellsheet = new PdfReader(SpellFile))
                                {
                                    if (preserveEdit) spellsheet.RemoveUsageRights();
                                    using (MemoryStream sms = new MemoryStream())
                                    {
                                        using (PdfStamper sp = new PdfStamper(spellsheet, sms))
                                        {
                                            fillBasicFields(spelltrans, sp);
                                            if (spelltrans.ContainsKey("SpellcastingClass")) sp.AcroFields.SetField(spelltrans["SpellcastingClass"], scf.DisplayName);
                                            if (spelltrans.ContainsKey("SpellcastingAbility")) sp.AcroFields.SetField(spelltrans["SpellcastingAbility"], Enum.GetName(typeof(Ability), scf.SpellcastingAbility));
                                            if (spelltrans.ContainsKey("SpellSaveDC")) sp.AcroFields.SetField(spelltrans["SpellSaveDC"], Player.current.getSpellSaveDC(scf.SpellcastingID, scf.SpellcastingAbility).ToString());
                                            if (spelltrans.ContainsKey("SpellAttackBonus")) sp.AcroFields.SetField(spelltrans["SpellAttackBonus"], Form1.plusMinus(Player.current.getSpellAttack(scf.SpellcastingID, scf.SpellcastingAbility)));
                                            for (int i = 0; i <= sheetmaxlevel && i < SpellLevels.Count; i++)
                                            {
                                                if (SpellSlots.Count >= i && i > 0 && SpellSlots[i - 1] > 0)
                                                {
                                                    if (spelltrans.ContainsKey("SpellSlots" + i))
                                                    {
                                                        sp.AcroFields.SetField(spelltrans["SpellSlots" + i], (offset > 0 ? "(" + (offset + i) + ") " : "") + SpellSlots[i - 1].ToString());
                                                    }
                                                }
                                                if (includeResources && UsedSpellSlots.Count >= i && i > 0)
                                                {
                                                    if (spelltrans.ContainsKey("SpellSlotsExpended" + i))
                                                    {
                                                        sp.AcroFields.SetField(spelltrans["SpellSlotsExpended" + i], UsedSpellSlots[i - 1].ToString());
                                                    }
                                                }
                                                int field = 1;
                                                if (!spelltrans.ContainsKey("Spell" + i + "-1")) SpellLevels[i].Clear();
                                                while (SpellLevels[i].Count > 0)
                                                {
                                                    if (!spelltrans.ContainsKey("Spell" + i + "-" + field)) break;
                                                    sp.AcroFields.SetField(spelltrans["Spell" + i + "-" + field], SpellLevels[i].First.Value.Name);
                                                    if (Prepared.Contains(SpellLevels[i].First.Value) && spelltrans.ContainsKey("Prepared" + i + "-" + field))
                                                        sp.AcroFields.SetField(spelltrans["Prepared" + i + "-" + field], "Yes");
                                                    SpellLevels[i].RemoveFirst();
                                                    field++;
                                                }
                                            }
                                            sp.FormFlattening = !preserveEdit;
                                            sp.Writer.CloseStream = false;
                                        }
                                        PdfReader sx = new PdfReader(sms.ToArray());
                                        for (int si = 1; si <= sx.NumberOfPages; si++)
                                        {
                                            pdfCopy.SetPageSize(sx.GetPageSize(si));
                                            pdfCopy.AddPage(pdfCopy.GetImportedPage(sx, si));
                                        }
                                    }
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

                    if (log && LogFile != null && LogFile != "")
                    {
                        Queue<JournalEntry> entries = new Queue<JournalEntry>(Player.current.ComplexJournal);
                        


                        Price gold = Player.current.getMoney(false);
                        int xp = Player.current.XP;
                        int renown = 0;
                        int downtime = 0;
                        int magic = 0;
                        int sheet = 0;
                        while (entries.Count > 0)
                        {
                            using (PdfReader logsheet = new PdfReader(LogFile))
                            {
                                if (preserveEdit) logsheet.RemoveUsageRights();
                                using (MemoryStream lms = new MemoryStream())
                                {
                                    int counter = 1;

                                    using (PdfStamper lp = new PdfStamper(logsheet, lms))
                                    {
                                        sheet++;
                                        fillBasicFields(logtrans, lp);
                                        if (logtrans.ContainsKey("Sheet")) lp.AcroFields.SetField(logtrans["Sheet"], sheet.ToString());
                                        while (entries.Count > 0 && (logtrans.ContainsKey("Title" + counter) || logtrans.ContainsKey("XP" + counter)))
                                        {
                                            JournalEntry entry = entries.Dequeue();
                                            if (entry.InSheet)
                                            {
                                                if (logtrans.ContainsKey("Title" + counter)) lp.AcroFields.SetField(logtrans["Title" + counter], entry.Title);
                                                if (logtrans.ContainsKey("Session" + counter)) lp.AcroFields.SetField(logtrans["Session" + counter], entry.Session);
                                                if (logtrans.ContainsKey("Date" + counter)) lp.AcroFields.SetField(logtrans["Date" + counter], entry.Added.ToString());
                                                if (logtrans.ContainsKey("DM" + counter)) lp.AcroFields.SetField(logtrans["DM" + counter], entry.DM);
                                                if (entry.Text != null)
                                                {
                                                    if (logtrans.ContainsKey("Notes" + counter)) lp.AcroFields.SetField(logtrans["Notes" + counter], entry.Text);
                                                    else if (logtrans.ContainsKey("Notes" + counter + "Line1"))
                                                    {
                                                        int line = 1;
                                                        Queue<string> lines = new Queue<string>(entry.Text.Split('\n'));
                                                        while (lines.Count > 0 && logtrans.ContainsKey("Notes" + counter + "Line" + (line + 1)))
                                                        {
                                                            lp.AcroFields.SetField(logtrans["Notes" + counter + "Line" + line], lines.Dequeue());
                                                            line++;
                                                        }
                                                        lp.AcroFields.SetField(logtrans["Notes" + counter + "Line" + line], string.Join(" ", lines));
                                                    }
                                                }
                                                if (logtrans.ContainsKey("XPStart" + counter)) lp.AcroFields.SetField(logtrans["XPStart" + counter], xp.ToString());
                                                if (logtrans.ContainsKey("GoldStart" + counter)) lp.AcroFields.SetField(logtrans["GoldStart" + counter], gold.toGold());
                                                if (logtrans.ContainsKey("DowntimeStart" + counter)) lp.AcroFields.SetField(logtrans["DowntimeStart" + counter], downtime.ToString());
                                                if (logtrans.ContainsKey("RenownStart" + counter)) lp.AcroFields.SetField(logtrans["RenownStart" + counter], renown.ToString());
                                                if (logtrans.ContainsKey("MagicItemsStart" + counter)) lp.AcroFields.SetField(logtrans["MagicItemsStart" + counter], magic.ToString());

                                                if (logtrans.ContainsKey("XP" + counter)) lp.AcroFields.SetField(logtrans["XP" + counter], entry.XP.ToString());
                                                if (logtrans.ContainsKey("Gold" + counter)) lp.AcroFields.SetField(logtrans["Gold" + counter], entry.getMoney());
                                                if (logtrans.ContainsKey("Downtime" + counter)) lp.AcroFields.SetField(logtrans["Downtime" + counter], plusMinus(entry.Downtime));
                                                if (logtrans.ContainsKey("Renown" + counter)) lp.AcroFields.SetField(logtrans["Renown" + counter], plusMinus(entry.Renown));
                                                if (logtrans.ContainsKey("MagicItems" + counter)) lp.AcroFields.SetField(logtrans["MagicItems" + counter], plusMinus(entry.MagicItems));
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
                                                if (logtrans.ContainsKey("XPEnd" + counter)) lp.AcroFields.SetField(logtrans["XPEnd" + counter], xp.ToString());
                                                if (logtrans.ContainsKey("GoldEnd" + counter)) lp.AcroFields.SetField(logtrans["GoldEnd" + counter], gold.toGold());
                                                if (logtrans.ContainsKey("DowntimeEnd" + counter)) lp.AcroFields.SetField(logtrans["DowntimeEnd" + counter], downtime.ToString());
                                                if (logtrans.ContainsKey("RenownEnd" + counter)) lp.AcroFields.SetField(logtrans["RenownEnd" + counter], renown.ToString());
                                                if (logtrans.ContainsKey("MagicItemsEnd" + counter)) lp.AcroFields.SetField(logtrans["MagicItemsEnd" + counter], magic.ToString());
                                                counter++;
                                            }

                                        }
                                        lp.FormFlattening = !preserveEdit;
                                        lp.Writer.CloseStream = false;
                                    }
                                    if (counter > 1)
                                    {
                                        PdfReader lx = new PdfReader(lms.ToArray());
                                        for (int li = 1; li <= lx.NumberOfPages; li++)
                                        {
                                            pdfCopy.SetPageSize(lx.GetPageSize(li));
                                            pdfCopy.AddPage(pdfCopy.GetImportedPage(lx, li));
                                        }
                                    }
                                }
                            }
                        }

                    }

                    if (spell && SpellbookFile != null && SpellbookFile != "")
                    {
                        List<SpellModifyFeature> mods = (from f in Player.current.getFeatures() where f is SpellModifyFeature select f as SpellModifyFeature).ToList();
                        Queue<Spell> entries = new Queue<Spell>(spellbook.OrderBy(s => s.Name).Distinct(new SpellEqualityComparer()));
                        int sheet = 0;
                        while (entries.Count > 0)
                        {
                            using (PdfReader spellbooksheet = new PdfReader(SpellbookFile))
                            {
                                if (preserveEdit) spellbooksheet.RemoveUsageRights();
                                using (MemoryStream sbms = new MemoryStream())
                                {
                                    int counter = 1;

                                    using (PdfStamper sbp = new PdfStamper(spellbooksheet, sbms))
                                    {
                                        sheet++;
                                        fillBasicFields(logtrans, sbp);
                                        if (booktrans.ContainsKey("Sheet")) sbp.AcroFields.SetField(booktrans["Sheet"], sheet.ToString());
                                        while (entries.Count > 0 && (booktrans.ContainsKey("Name" + counter) || booktrans.ContainsKey("Description" + counter)))
                                        {
                                            Spell entry = entries.Dequeue();
                                            StringBuilder description = new StringBuilder();
                                            String name = entry.Name;
                                            String level = "";
                                            List<Keyword> original = entry.getKeywords();
                                            List<Keyword> keywords = new List<Keyword>(original);
                                            if (booktrans.ContainsKey("Level" + counter)) sbp.AcroFields.SetField(booktrans["Level" + counter], entry.Level.ToString());
                                            else level = (entry.Level == 0 ? "" : " " + AddOrdinal(entry.Level) + " Level");
                                            keywords.RemoveAll(k => k.Name.Equals("cantrip", StringComparison.InvariantCultureIgnoreCase));
                                            if (booktrans.ContainsKey("School" + counter)) sbp.AcroFields.SetField(booktrans["School" + counter], getAndRemoveSchool(keywords));
                                            else if (!booktrans.ContainsKey("Keywords" + counter)) level += " " + getAndRemoveSchool(keywords);
                                            if (entry.Level == 0) level += " Cantrip";
                                            if (booktrans.ContainsKey("SchoolLevel" + counter)) sbp.AcroFields.SetField(booktrans["SchoolLevel" + counter], level);
                                            else name += ", " + level;
                                            if (booktrans.ContainsKey("Name" + counter)) sbp.AcroFields.SetField(booktrans["Name" + counter], name);
                                            else description.Append(name).AppendLine();
                                            if (booktrans.ContainsKey("Classes" + counter)) sbp.AcroFields.SetField(booktrans["Classes" + counter], getAndRemoveClasses(keywords));
                                            if (booktrans.ContainsKey("Time" + counter)) sbp.AcroFields.SetField(booktrans["Time" + counter], entry.CastingTime);
                                            else description.Append("Casting Time: ").Append(entry.CastingTime).AppendLine();
                                            if (booktrans.ContainsKey("Range" + counter)) sbp.AcroFields.SetField(booktrans["Range" + counter], entry.Range);
                                            else description.Append("Range: ").Append(entry.Range).AppendLine();
                                            if (booktrans.ContainsKey("Components" + counter)) sbp.AcroFields.SetField(booktrans["Components" + counter], getAndRemoveComponents(keywords));
                                            else if (!booktrans.ContainsKey("Keywords" + counter)) description.Append("Components: ").Append(getAndRemoveComponents(keywords)).AppendLine();
                                            if (booktrans.ContainsKey("Duration" + counter)) sbp.AcroFields.SetField(booktrans["Duration" + counter], entry.Duration);
                                            else description.Append("Duration: ").Append(entry.Duration).AppendLine();

                                            if (booktrans.ContainsKey("Keywords" + counter)) sbp.AcroFields.SetField(booktrans["Keywords" + counter], String.Join(", ",keywords));

                                            description.Append(entry.Description);
                                            StringBuilder add = new StringBuilder();
                                            foreach (Description d in entry.Descriptions)
                                            {
                                                add.Append(d.Name.ToUpperInvariant()).Append(": ").Append(d.Text).AppendLine();
                                                if (d is ListDescription) foreach (Names n in (d as ListDescription).Names) add.Append(n.Title).Append(": ").Append(String.Join(", ", n.ListOfNames)).AppendLine();
                                                if (d is TableDescription) foreach (TableEntry tr in (d as TableDescription).Entries) add.Append(tr.ToFullString()).AppendLine();
                                            }
                                            if (booktrans.ContainsKey("AdditionDescription" + counter)) sbp.AcroFields.SetField(booktrans["AdditionDescription" + counter], add.ToString());
                                            else description.AppendLine().AppendLine().Append(add.ToString());
                                            StringBuilder Modifiers = new StringBuilder();
                                            foreach (SpellModifyFeature m in mods.Where(f => Utils.matches(entry, ((SpellModifyFeature)f).Spells, null)))
                                            {
                                                Modifiers.Append(m.Name.ToUpperInvariant()).Append(": ").Append(m.Text).AppendLine();
                                            }
                                            if (booktrans.ContainsKey("Modifiers" + counter)) sbp.AcroFields.SetField(booktrans["Modifiers" + counter], Modifiers.ToString());
                                            else description.AppendLine().Append(Modifiers.ToString());
                                            if (booktrans.ContainsKey("Source" + counter)) sbp.AcroFields.SetField(booktrans["Source" + counter], entry.Source);
                                            else description.AppendLine().Append("Source: ").Append(entry.Source).AppendLine();
                                            if (booktrans.ContainsKey("Description" + counter)) sbp.AcroFields.SetField(booktrans["Description" + counter], description.ToString());
                                            counter++;
                                        }
                                        sbp.FormFlattening = !preserveEdit;
                                        sbp.Writer.CloseStream = false;
                                    }
                                    if (counter > 1)
                                    {
                                        PdfReader lx = new PdfReader(sbms.ToArray());
                                        for (int li = 1; li <= lx.NumberOfPages; li++)
                                        {
                                            pdfCopy.SetPageSize(lx.GetPageSize(li));
                                            pdfCopy.AddPage(pdfCopy.GetImportedPage(lx, li));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sourceDocument.Close();
                }
            }
        }

        private static List<Keyword> Schools = new List<Keyword>()
        {
            new Keyword("Abjuration"), new Keyword("Conjuration"), new Keyword("Divination"), new Keyword("Enchantment"), new Keyword("Evocation"), new Keyword("Illusion"), new Keyword("Necromancy"), new Keyword("Transmutation")
        };

        private string getAndRemoveSchool(List<Keyword> kw)
        {
            List<string> s = new List<string>();
            foreach (Keyword k in Schools) if (kw.Remove(k)) s.Add(k.Name.ToLowerInvariant());
            string res = string.Join(", ", s);
            return char.ToUpper(res[0]) + res.Substring(1);
        }

        private string getAndRemoveComponents(List<Keyword> kw)
        {
            List<string> r = new List<string>();
            bool v = false, s = false, m = false;
            string mat = "";
            for (int i = kw.Count - 1; i >= 0; i--)
            {
                if (kw[i].Name.Equals("verbal", StringComparison.InvariantCultureIgnoreCase))
                {
                    v = true;
                    kw.RemoveAt(i);
                } else if (kw[i].Name.Equals("somatic", StringComparison.InvariantCultureIgnoreCase))
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
            return char.ToUpper(res[0]) + res.Substring(1);
        }

        private static List<Keyword> Classes = new List<Keyword>();
        private string getAndRemoveClasses(List<Keyword> kw)
        {
            if (Classes.Count == 0) foreach (string cl in ClassDefinition.simple.Keys) Classes.Add(new Keyword(cl));
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

        private void fillBasicFields(Dictionary<string, string> trans, PdfStamper p)
        {
            if (trans.ContainsKey("Background")) p.AcroFields.SetField(trans["Background"], Player.current.BackgroundName);
            if (trans.ContainsKey("Race")) p.AcroFields.SetField(trans["Race"], Player.current.getRaceSubName());
            if (trans.ContainsKey("PersonalityTrait")) p.AcroFields.SetField(trans["PersonalityTrait"], Player.current.PersonalityTrait);
            if (trans.ContainsKey("Ideal")) p.AcroFields.SetField(trans["Ideal"], Player.current.Ideal);
            if (trans.ContainsKey("Bond")) p.AcroFields.SetField(trans["Bond"], Player.current.Bond);
            if (trans.ContainsKey("Flaw")) p.AcroFields.SetField(trans["Flaw"], Player.current.Flaw);
            if (trans.ContainsKey("PlayerName")) p.AcroFields.SetField(trans["PlayerName"], Player.current.PlayerName);
            if (trans.ContainsKey("Alignment")) p.AcroFields.SetField(trans["Alignment"], Player.current.Alignment);
            if (trans.ContainsKey("XP")) p.AcroFields.SetField(trans["XP"], Player.current.getXP().ToString());
            if (trans.ContainsKey("Age")) p.AcroFields.SetField(trans["Age"], Player.current.Age.ToString());
            if (trans.ContainsKey("Height")) p.AcroFields.SetField(trans["Height"], Player.current.Height.ToString());
            if (trans.ContainsKey("Weight")) p.AcroFields.SetField(trans["Weight"], Player.current.Weight.ToString() + " lb");
            if (trans.ContainsKey("Eyes")) p.AcroFields.SetField(trans["Eyes"], Player.current.Eyes.ToString());
            if (trans.ContainsKey("Skin")) p.AcroFields.SetField(trans["Skin"], Player.current.Skin.ToString());
            if (trans.ContainsKey("Hair")) p.AcroFields.SetField(trans["Hair"], Player.current.Hair.ToString());
            if (trans.ContainsKey("Speed")) p.AcroFields.SetField(trans["Speed"], Player.current.getSpeed().ToString());
            if (trans.ContainsKey("FactionName")) p.AcroFields.SetField(trans["FactionName"], Player.current.FactionName);
            if (trans.ContainsKey("Backstory")) p.AcroFields.SetField(trans["Backstory"], Player.current.Backstory);
            if (trans.ContainsKey("Allies")) p.AcroFields.SetField(trans["Allies"], Player.current.Allies);
            if (trans.ContainsKey("Strength")) p.AcroFields.SetField(trans["Strength"], Player.current.getStrength().ToString());
            if (trans.ContainsKey("Dexterity")) p.AcroFields.SetField(trans["Dexterity"], Player.current.getDexterity().ToString());
            if (trans.ContainsKey("Constitution")) p.AcroFields.SetField(trans["Constitution"], Player.current.getConstitution().ToString());
            if (trans.ContainsKey("Intelligence")) p.AcroFields.SetField(trans["Intelligence"], Player.current.getIntelligence().ToString());
            if (trans.ContainsKey("Wisdom")) p.AcroFields.SetField(trans["Wisdom"], Player.current.getWisdom().ToString());
            if (trans.ContainsKey("Charisma")) p.AcroFields.SetField(trans["Charisma"], Player.current.getCharisma().ToString());
            if (trans.ContainsKey("StrengthModifier")) p.AcroFields.SetField(trans["StrengthModifier"], Form1.plusMinus(Player.current.getStrengthMod()));
            if (trans.ContainsKey("DexterityModifier")) p.AcroFields.SetField(trans["DexterityModifier"], Form1.plusMinus(Player.current.getDexterityMod()));
            if (trans.ContainsKey("ConstitutionModifier")) p.AcroFields.SetField(trans["ConstitutionModifier"], Form1.plusMinus(Player.current.getConstitutionMod()));
            if (trans.ContainsKey("IntelligenceModifier")) p.AcroFields.SetField(trans["IntelligenceModifier"], Form1.plusMinus(Player.current.getIntelligenceMod()));
            if (trans.ContainsKey("WisdomModifier")) p.AcroFields.SetField(trans["WisdomModifier"], Form1.plusMinus(Player.current.getWisdomMod()));
            if (trans.ContainsKey("AC")) p.AcroFields.SetField(trans["AC"], Player.current.getAC().ToString());
            if (trans.ContainsKey("ProficiencyBonus")) p.AcroFields.SetField(trans["ProficiencyBonus"], Form1.plusMinus(Player.current.getProficiency()));
            if (trans.ContainsKey("Initiative")) p.AcroFields.SetField(trans["Initiative"], Form1.plusMinus(Player.current.getInitiative()));
            if (trans.ContainsKey("CharismaModifier")) p.AcroFields.SetField(trans["CharismaModifier"], Player.current.getCharismaMod().ToString());
            if (trans.ContainsKey("CharacterName")) p.AcroFields.SetField(trans["CharacterName"], Player.current.Name);
            if (trans.ContainsKey("CharacterName2")) p.AcroFields.SetField(trans["CharacterName2"], Player.current.Name);
            if (trans.ContainsKey("ClassLevel")) p.AcroFields.SetField(trans["ClassLevel"], String.Join(" | ", Player.current.Classes));
            if (trans.ContainsKey("DCI")) p.AcroFields.SetField(trans["DCI"], Player.current.DCI);
        }

        private string plusMinus(int value)
        {
            if (value > 0) return "+" + value;
            if (value == 0) return "--";
            return value.ToString();
        }

        public void scanFields()
        {
            PdfReader x = new PdfReader(File);
            foreach (string field in x.AcroFields.Fields.Keys)
            {
                Fields.Add(new PDFField(field, field));
            }
            //System.Windows.Forms.MessageBox.Show(sb.ToString());
        }
        public void splitSpellPage(int page)
        {
            PdfReader reader = new PdfReader(File);
            PdfReader sreader = new PdfReader(SpellFile);
            Document sourceDocument = new Document(sreader.GetPageSizeWithRotation(1));
            PdfCopy pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream("Alternate-Sheet.pdf", System.IO.FileMode.Create));
            sourceDocument.Open();
            PdfImportedPage importedPage = pdfCopyProvider.GetImportedPage(sreader, 1);
            pdfCopyProvider.AddPage(importedPage);
            pdfCopyProvider.AddPage(pdfCopyProvider.GetImportedPage(reader, 2));
            sourceDocument.Close();
            reader.Close();
            sreader.Close();
            pdfCopyProvider.Close();
        }
        public static void markFields(string file)
        {
            string outfile = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "-marked" + Path.GetExtension(file));
            FileStream temp = new FileStream(outfile, FileMode.CreateNew);
            using (PdfReader sheet = new PdfReader(file))
            {
                sheet.RemoveUsageRights();
                using (PdfStamper p = new PdfStamper(sheet, temp))
                {
                    foreach (string field in sheet.AcroFields.Fields.Keys)
                    {
                        foreach (AcroFields.FieldPosition pos in p.AcroFields.GetFieldPositions(field))
                        {
                            Rectangle pageSize = sheet.GetPageSizeWithRotation(pos.page);
                            PdfContentByte pdfPageContents = p.GetOverContent(pos.page);
                            pdfPageContents.BeginText();
                            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
                            pdfPageContents.SetFontAndSize(baseFont, 6);
                            pdfPageContents.ShowTextAligned(PdfContentByte.ALIGN_CENTER, field, pos.position.Left + pos.position.Width / 2, pos.position.Top - pos.position.Height / 2, 0);
                            pdfPageContents.EndText();
                        }

                    }
                    p.FormFlattening = true;
                }
            }
        }
    }
}
