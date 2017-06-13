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


namespace Character_Builder_5
{
    public class PDF
    {
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(PDF));
        public String Name { get; set; }
        public String File { get; set; }
        public String SpellFile { get; set; }
        public List<PDFField> Fields = new List<PDFField>();
        public List<PDFField> SpellFields = new List<PDFField>();
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
        public void export(FileStream fs, bool preserveEdit, bool includeResources)
        {
            Dictionary<String, String> trans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<String, String> spelltrans = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (PDFField pf in Fields) trans.Add(pf.Name, pf.Field);
            foreach (PDFField pf in SpellFields) spelltrans.Add(pf.Name, pf.Field);
            Dictionary<string, bool> hiddenfeats = Player.current.HiddenFeatures.ToDictionary(f => f, f => true, StringComparer.OrdinalIgnoreCase);
            List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>(from f in Player.current.getFeatures() where f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID != "MULTICLASS" select (SpellcastingFeature)f);
            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfReader sheet = new PdfReader(File))
                {
                    if (preserveEdit) sheet.RemoveUsageRights();
                    using (PdfStamper p = new PdfStamper(sheet, ms))
                    {
                        p.AcroFields.SetField(trans["Background"], Player.current.BackgroundName);
                        p.AcroFields.SetField(trans["Race"], Player.current.getRaceSubName());
                        p.AcroFields.SetField(trans["PersonalityTrait"], Player.current.PersonalityTrait);
                        p.AcroFields.SetField(trans["Ideal"], Player.current.Ideal);
                        p.AcroFields.SetField(trans["Bond"], Player.current.Bond);
                        p.AcroFields.SetField(trans["Flaw"], Player.current.Flaw);
                        p.AcroFields.SetField(trans["PlayerName"], Player.current.PlayerName);
                        p.AcroFields.SetField(trans["Alignment"], Player.current.Alignment);
                        p.AcroFields.SetField(trans["XP"], Player.current.getXP().ToString());
                        p.AcroFields.SetField(trans["Age"], Player.current.Age.ToString());
                        p.AcroFields.SetField(trans["Height"], Player.current.Height.ToString());
                        p.AcroFields.SetField(trans["Weight"], Player.current.Weight.ToString() + " lb");
                        p.AcroFields.SetField(trans["Eyes"], Player.current.Eyes.ToString());
                        p.AcroFields.SetField(trans["Skin"], Player.current.Skin.ToString());
                        p.AcroFields.SetField(trans["Hair"], Player.current.Hair.ToString());
                        p.AcroFields.SetField(trans["Speed"], Player.current.getSpeed().ToString());
                        p.AcroFields.SetField(trans["FactionName"], Player.current.FactionName);
                        p.AcroFields.SetField(trans["Backstory"], Player.current.Backstory);
                        p.AcroFields.SetField(trans["Allies"], Player.current.Allies);
                        p.AcroFields.SetField(trans["Strength"], Player.current.getStrength().ToString());
                        p.AcroFields.SetField(trans["Dexterity"], Player.current.getDexterity().ToString());
                        p.AcroFields.SetField(trans["Constitution"], Player.current.getConstitution().ToString());
                        p.AcroFields.SetField(trans["Intelligence"], Player.current.getIntelligence().ToString());
                        p.AcroFields.SetField(trans["Wisdom"], Player.current.getWisdom().ToString());
                        p.AcroFields.SetField(trans["Charisma"], Player.current.getCharisma().ToString());
                        p.AcroFields.SetField(trans["StrengthModifier"], Form1.plusMinus(Player.current.getStrengthMod()));
                        p.AcroFields.SetField(trans["DexterityModifier"], Form1.plusMinus(Player.current.getDexterityMod()));
                        p.AcroFields.SetField(trans["ConstitutionModifier"], Form1.plusMinus(Player.current.getConstitutionMod()));
                        p.AcroFields.SetField(trans["IntelligenceModifier"], Form1.plusMinus(Player.current.getIntelligenceMod()));
                        p.AcroFields.SetField(trans["WisdomModifier"], Form1.plusMinus(Player.current.getWisdomMod()));
                        p.AcroFields.SetField(trans["AC"], Player.current.getAC().ToString());
                        p.AcroFields.SetField(trans["ProficiencyBonus"], Form1.plusMinus(Player.current.getProficiency()));
                        p.AcroFields.SetField(trans["Initiative"], Form1.plusMinus(Player.current.getInitiative()));
                        p.AcroFields.SetField(trans["CharismaModifier"], Player.current.getCharismaMod().ToString());
                        p.AcroFields.SetField(trans["CharacterName"], Player.current.Name);
                        p.AcroFields.SetField(trans["CharacterName2"], Player.current.Name);
                        p.AcroFields.SetField(trans["ClassLevel"], String.Join(" | ", Player.current.Classes));
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
                        p.AcroFields.SetField(trans["Attacks"], attacks);
                        List<HitDie> hd = Player.current.getHitDie();
                        p.AcroFields.SetField(trans["HitDieTotal"], String.Join(", ", from h in hd select h.Total()));
                        int maxhp=Player.current.getHitpointMax();
                        p.AcroFields.SetField(trans["MaxHP"], maxhp.ToString());
                        if (includeResources)
                        {
                            p.AcroFields.SetField(trans["CurrentHP"], (maxhp+Player.current.CurrentHPLoss).ToString());
                            p.AcroFields.SetField(trans["TempHP"], Player.current.TempHP.ToString());
                            for (int d = 1; d <= Player.current.FailedDeathSaves; d++) if (trans.ContainsKey("DeathSaveFail" + d)) p.AcroFields.SetField(trans["DeathSaveFail" + d], "Yes");
                                else break;
                            for (int d = 1; d <= Player.current.SuccessDeathSaves; d++) if (trans.ContainsKey("DeathSaveSuccess" + d)) p.AcroFields.SetField(trans["DeathSaveSuccess" + d], "Yes");
                                else break;
                            p.AcroFields.SetField(trans["HitDie"], String.Join(", ", hd));
                            if (Player.current.Inspiration) p.AcroFields.SetField(trans["Inspiration"], "Yes");
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
                            p.AcroFields.SetField(trans["Equipment"], String.Join("\n", equip));
                            p.AcroFields.SetField(trans["CP"], money.cp.ToString());
                            p.AcroFields.SetField(trans["SP"], money.sp.ToString());
                            p.AcroFields.SetField(trans["EP"], money.ep.ToString());
                            p.AcroFields.SetField(trans["GP"], money.gp.ToString());
                            p.AcroFields.SetField(trans["PP"], money.pp.ToString());
                        }
                        else
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
                        p.AcroFields.SetField(trans["Treasure"], String.Join("\n", treasure));
                        if (preserveEdit || true)
                        {
                            List<string> feats = new List<string>();
                            foreach (Feature f in onUse) if (!f.Hidden) feats.Add(f.ShortDesc());
                            foreach (Feature f in Player.current.getBackgroundFeatures()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                            foreach (Feature f in Player.current.getRaceFeatures()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats.Add(f.ShortDesc());
                            p.AcroFields.SetField(trans["RaceBackgroundFeatures"], String.Join("\n", feats));
                            List<string> feats2 = new List<string>();
                            foreach (Feature f in Player.current.getClassFeatures()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats2.Add(f.ShortDesc());
                            foreach (Feature f in Player.current.getCommonFeaturesAndFeats()) if (!f.Hidden && !hiddenfeats.ContainsKey(f.Name)) feats2.Add(f.ShortDesc());
                            p.AcroFields.SetField(trans["Features"], String.Join("\n", feats2));
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
                            Prepared.AddRange(sc.getLearned());
                            Available.AddRange(Prepared);
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
                    sourceDocument.Close();
                }
            }
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
        public void markFields()
        {
            FileStream temp = new FileStream("temp.pdf", FileMode.Truncate);
            using (PdfReader sheet = new PdfReader(SpellFile))
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
