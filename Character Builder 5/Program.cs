using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Character_Builder_5
{
    public static class Program
    {
        public static void Exit()
        {
            Application.Exit();
        }
        public static Form1 MainWindow = null;
        public static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static void register()
        {
            if (IsRunAsAdmin())
            {
                string ext = ".cb5";
                RegistryKey key = Registry.ClassesRoot.CreateSubKey(ext);
                key.SetValue("", "Character Builder 5");
                key.Close();

                key = Registry.ClassesRoot.CreateSubKey(ext + "\\Shell\\Open\\command");
                //key = key.CreateSubKey("command");

                key.SetValue("", "\"" + Application.ExecutablePath + "\" \"%L\"");
                key.Close();

                key = Registry.ClassesRoot.CreateSubKey(ext + "\\DefaultIcon");
                key.SetValue("", Application.ExecutablePath);
                key.Close();
            }
        }
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            /*            Weapon battleaxe = new Weapon("Battleaxe", "", new Price(0, 0, 10), 4, "1d8", "slashing", new Versatile("1d10"), new Keyword("martial"), new Keyword("melee"));
                        Weapon handaxe = new Weapon("Handaxe", "", new Price(0, 0, 5), 2, "1d6", "slashing", new Keyword("light"), new Keyword("thrown"), new Range(20, 60), new Keyword("simple"), new Keyword("melee"));
                        Weapon lighthammer = new Weapon("Light Hammer", "", new Price(0, 2, 0), 2, "1d4", "bludgeoning", new Keyword("light"), new Keyword("thrown"), new Range(20, 60), new Keyword("simple"), new Keyword("melee"));
                        Weapon warhammer = new Weapon("Warhammer", "", new Price(0, 0, 15), 2, "1d8", "bludgeoning", new Versatile("1d10"), new Keyword("martial"), new Keyword("melee"));
                        Language dwarven = new Language("Dwarvish");
                        Language common = new Language("Common");
                        Tool smith = new Tool("Smith's Tools", "These special tools include the items needed to pursue a craft or trade. Proficiency with a set of artisan's tools lets you add your proficiency bonus to any ability checks you make using the tools in your craft. Each type of artisan's tools requires a separate proficiency",new Price(0,0,20),8);
                        Tool brewer = new Tool("Brewer's Supplies", "These special tools include the items needed to pursue a craft or trade. Proficiency with a set of artisan's tools lets you add your proficiency bonus to any ability checks you make using the tools in your craft. Each type of artisan's tools requires a separate proficiency",new Price(0,0,20),9);
                        Tool mason = new Tool("Mason's Tools", "These special tools include the items needed to pursue a craft or trade. Proficiency with a set of artisan's tools lets you add your proficiency bonus to any ability checks you make using the tools in your craft. Each type of artisan's tools requires a separate proficiency",new Price(0,0,10),8);
                        

                        XmlSerializer raceser = new XmlSerializer(typeof(Race));
                        TextWriter writer = new StreamWriter("dwarf.xml");
                        raceser.Serialize(writer, dwarf);
                        writer.Close();
                        */
            Config.LoadConfig(Application.StartupPath);
            //register();
            //Generate.GenerateWeapons();
            //Generate.GenerateGear();
            //Generate.generateTools();
            //Item.ExportAll();
            //Generate.generateSkills();
            //Generate.generateLangs();
            //Language.ExportAll();

            /*MagicProperty m = new MagicProperty();
            m.OnUseFeatures.Add(new BonusFeature("", "", 0, new List<string>(), 1, 1, 0, 0, 0, "Weapon", 1, true));
            m.Name = "Test";
            MagicCategory test=new MagicCategory("Test");
            test.Contents.Add(m);
            MagicProperty.Categories.Add("Test", test);
            MagicProperty.ExportAll();*/
            try
            {
                if (SourceManager.init(Application.StartupPath))
                {
                    PluginManager.manager = new PluginManager(Path.Combine(Application.StartupPath, ConfigManager.Directory_Plugins));
                    Skill.ImportAll();
                    Language.ImportAll();
                    Spell.ImportAll();
                    Item.ImportAll();
                    Background.ImportAll();
                    Race.ImportAll();
                    //Generate2.generatePHBRaces();
                    //Race.ExportAll();
                    SubRace.ImportAll();

                    FeatureCollection.ImportAll();
                    Condition.ImportAll();

                    /*Generate.generatePacks();
                    Generate.generateTrinkets();
                    Item.ExportAll();*/

                    //Generate.generateConditions();
                    //Condition.ExportAll();
                    //Generate.generateSpells();
                    //Spell.ExportAll();
                    //Generate.generateMissingSpells();
                    MagicProperty.ImportAll();
                    ClassDefinition.ImportAll();
                    SubClass.ImportAll();

                    //Generate.generateClasses();
                    //Generate2.generateStuff();
                    //ClassDefinition.ExportAll();
                    //SubClass.ExportAll();
                    //XmlSerializer serializer = new XmlSerializer(typeof(SubClass));
                    /*using (TextReader reader = new StreamReader("SubClasses/Battlemaster.xml"))
                    {
                        SubClass s = (SubClass)serializer.Deserialize(reader);
                        s.register();
                    }*/
                    //ConfigManager.PDFExporter.markFields();

                    //FeatureCollection.Get("Category = 'Feats'");
                    //FeatureCollection.Get("Invocations").Add(new SkillProficiencyFeature("Beguiling Influence","You gain proficiency in the Deception and Persuasion skills.",Skill.Get("Deception"),Skill.Get("Persuasion")));
                    //FeatureCollection.Get("Invocations").Add(new Feature("Devil's Sight", "You can see normally in darkness, both magical and nonmagical, to a distance of 120 feet."));
                    //FeatureCollection.Get("Invocations").Add(new Feature("Eyes of the Rune Keeper", "You can read all writing"));
                    //FeatureCollection.Get("Invocations").Add(new Feature("LifeDrinker", "bla"));
                    //FeatureCollection.ExportAll();

                    //Race.ImportAll();

                    //PDF f = new PDF();
                    //f.File = "Fillable CharacterSheet 3 pages.pdf";
                    //f.Name = "Default Sheet";
                    //f.Fields.Add(new PDFField("x","y"));
                    //f.scanFields();
                    //f.Save("DefaultPDF.xml");
                    //f.export(Player.current);
                    //Level.Generate();

                    //AbilityScores.Generate();
                    //Generate.generateRaces();
                    //Generate.generateSubRaces();
                    //Race.ExportAll();
                    // SubRace.ExportAll();


                    //Generate.generateBackgrounds();
                    //Background.ExportAll();

                    /* new Language("Dwarvish");
                    new Language("Common");
                    new Tool("Smith's Tools", "These special tools include the items needed to pursue a craft or trade. Proficiency with a set of artisan's tools lets you add your proficiency bonus to any ability checks you make using the tools in your craft. Each type of artisan's tools requires a separate proficiency", new Price(0, 0, 20), 8);
                    new Tool("Brewer's Supplies", "These special tools include the items needed to pursue a craft or trade. Proficiency with a set of artisan's tools lets you add your proficiency bonus to any ability checks you make using the tools in your craft. Each type of artisan's tools requires a separate proficiency", new Price(0, 0, 20), 9);
                    new Item("Abacus", "", new Price(0, 0, 2), 2);
                    new Item("Acid (vial)", "As an action, you can splash the contents of this vial onto a creature within 5 feet of you or throw the vial up to 20 feet, shattering it on impact. In either case, make a ranged attack against a creature or object, treating the acid as an im provised w eapon. On a hit, the target takes 2d6 acid damage.", new Price(0, 0, 25), 1);
                     */
                } else
                {
                    Exit();
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Error while loading Data Files");
                Program.Exit();
            }

            HashSet<Keyword> kws = new HashSet<Keyword>();
            //foreach (Item s in Item.items.Values)
            //{
            //    foreach (Keyword kw in s.Keywords) kws.Add(kw);
            //}
            foreach (Spell s in Spell.spells.Values)
            {
                foreach (Keyword kw in s.Keywords) if (!(kw is Material)) kws.Add(kw);
            }
            //foreach (ClassDefinition s in ClassDefinition.classes.Values)
            //{
            //    kws.Add(new Keyword(s.Name));
            //}
            Console.Out.WriteLine(String.Join(", ", kws));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow = new Form1();
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() > 1)
            {
                string file = args[1];
                if (File.Exists(file))
                {
                    MainWindow.lastfile = file;
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        try
                        {
                            Player.current = Player.Load(fs);
                            MainWindow.UpdateLayout();
                        }
                        catch (Exception e)
                        {
                            System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Error while loading "+file);
                            Program.Exit();
                        }
                    }
                }
            }
            if (args.Count() > 2 && args[2] == "register") register();
            Application.Run(MainWindow);
        }
        public static void resetglobals()
        {
            MainWindow.resetglobals();
        }
    }
}
