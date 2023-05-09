using Character_Builder;
using OGL;
using Character_Builder_Forms;
using ToJSON;
using Character_Builder_5;
using System.Text.Json;
using System.Text.Json.Serialization;

var context = new BuilderContext();
ConfigManager.LogEvents += (sender, text, e) => 
    Console.Error.WriteLine((text != null ? text + ": " : "") + e?.Message);
if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: ToJSON file.cb5");
    return 1;
}
string basepath = AppContext.BaseDirectory;
if (basepath.EndsWith(Path.DirectorySeparatorChar) || basepath.EndsWith(Path.AltDirectorySeparatorChar)) basepath = basepath.Substring(0, basepath.Length - 1);
if (!File.Exists(Path.Combine(basepath, "Config.xml"))) Console.Error.WriteLine("Can't find Config.xml in " + basepath);
ConfigManager loaded = context.LoadConfig(basepath);
context.LoadAbilityScores(ImportExtensions.Fullpath(basepath, loaded.AbilityScores));
ConfigManager.LicenseProvider = new LicenseManager();
if (SourceManager.Init(context, basepath, true))
{
    string file = args[0];
    if (File.Exists(file))
    {
        using (FileStream fs = new FileStream(file, FileMode.Open))
        {
            try
            {
                context.LoadPluginManager(Path.Combine(basepath, context.Config.Plugins_Directory));
                context.LoadLevel(ImportExtensions.Fullpath(basepath, "Levels.xml"));
                context.Player = PlayerExtensions.Load(context, fs);
                context.ImportZips(false);
                context.ImportSkills(false);
                context.ImportLanguages(false);
                context.ImportSpells(false);
                context.ImportItems(false);
                context.ImportBackgrounds(false);
                context.ImportRaces(false);
                context.ImportSubRaces(false);
                context.ImportStandaloneFeatures(false);
                context.ImportConditions(false);
                context.ImportMagic(false);
                context.ImportClasses(false, true);
                context.ImportSubClasses(false, true);
                foreach (ClassDefinition c in context.Classes.Values) c.ApplyKeywords(context);
                foreach (SubClass c in context.SubClasses.Values) c.ApplyKeywords(context);
                context.ImportMonsters(false);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                return 1;
            }
        }
    }
    else
    {
        Console.Error.WriteLine($"File not Found: {file} in {Environment.CurrentDirectory}");
        return 1;
    }
} 
else
{
    return 1;
}
JsonWriterOptions options = new JsonWriterOptions()
{
    Indented = true
};
context.Player.ComplexJournal.ForEach(x => x.Possessions.ForEach(xx => xx.Context = context));
JsonSerializer.Serialize<ExportPlayer>(Console.OpenStandardOutput(), new ExportPlayer(context.Player), new JsonSerializerOptions()
{
    WriteIndented = true,
    Converters = {new JsonStringEnumConverter()},
});
return 0;