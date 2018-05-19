using Character_Builder;
using Character_Builder_Forms;
using Character_Builder_Plugin;
using OGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Character_Builder_Base;

namespace Character_Builder_5
{
    public static class PlayerExtensions
    {

        public static PDF Load(String file)
        {
            using (TextReader reader = new StreamReader(file))
            {
                PDF p = (PDF)PDF.Serializer.Deserialize(reader);
                p.File = ImportExtensions.Fullpath(Path.GetDirectoryName(file), p.File);
                p.SpellFile = ImportExtensions.Fullpath(Path.GetDirectoryName(file), p.SpellFile);
                p.ActionsFile = ImportExtensions.Fullpath(Path.GetDirectoryName(file), p.ActionsFile);
                p.ActionsFile2 = ImportExtensions.Fullpath(Path.GetDirectoryName(file), p.ActionsFile2);
                p.LogFile = ImportExtensions.Fullpath(Path.GetDirectoryName(file), p.LogFile);
                p.SpellbookFile = ImportExtensions.Fullpath(Path.GetDirectoryName(file), p.SpellbookFile);
                return p;
            }
        }

        public static void Save(this PDF p, String file)
        {
            using (TextWriter writer = new StreamWriter(file)) PDF.Serializer.Serialize(writer, p);
        }

        public static void Save(this Player p, FileStream fs)
        {
            Player.Serializer.Serialize(fs, p);
        }
        public static void Save(this Player p, TextWriter fs)
        {
            Player.Serializer.Serialize(fs, p);
        }
        public static Player Load(BuilderContext context, FileStream fs)
        {
            try
            {
                Player p = (Player)Player.Serializer.Deserialize(fs);
                p.Context = context;
                p.Allies = p.Allies.Replace("\n", Environment.NewLine);
                p.Backstory = p.Backstory.Replace("\n", Environment.NewLine);
                foreach (Possession pos in p.Possessions) if (pos.Description != null) pos.Description = pos.Description.Replace("\n", Environment.NewLine);
                for (int i = 0; i < p.Journal.Count; i++) p.Journal[i] = p.Journal[i].Replace("\n", Environment.NewLine);
                for (int i = 0; i < p.ComplexJournal.Count; i++) if (p.ComplexJournal[i].Text != null) p.ComplexJournal[i].Text = p.ComplexJournal[i].Text.Replace("\n", Environment.NewLine);
                if (p.Portrait == null && p.PortraitLocation != null && File.Exists(p.PortraitLocation)) p.SetPortrait(new Bitmap(p.PortraitLocation));
                if (p.FactionImage == null && p.FactionImageLocation != null && File.Exists(p.FactionImageLocation)) p.SetFactionImage(new Bitmap(p.FactionImageLocation));
                p.PortraitLocation = null;
                p.FactionImageLocation = null;
                foreach (Spellcasting sc in p.Spellcasting)
                {
                    sc.PostLoad(p.GetLevel());
                }
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Bitmap GetPortrait(this Player o)
        {
            if (o.Portrait == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.Portrait)) return new Bitmap(ms);
        }
        public static void SetPortrait(this Player o, Bitmap value)
        {
            if (value == null) o.Portrait = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.Portrait = ms.ToArray();
                }
        }
        public static Bitmap GetFactionImage(this Player o)
        {
            if (o.FactionImage == null) return null;
            else using (MemoryStream ms = new MemoryStream(o.FactionImage)) return new Bitmap(ms);
        }
        public static void SetFactionImage(this Player o, Bitmap value)
        {
            if (value == null) o.Portrait = null;
            else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    o.FactionImage = ms.ToArray();
                }
        }
        public static void LoadPluginManager(this BuilderContext context, string path)
        {
            PluginManager plug = new PluginManager();
            context.Plugins = plug;
            plug.Add(new NoFreeEquipment());
            string[] dllFileNames = null;
            if (Directory.Exists(path))
            {
                try
                {
                    dllFileNames = Directory.GetFiles(path, "*.dll");
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error loading Plugins", e);
                }
            }
            else
            {
                return;
            }

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFile in dllFileNames)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dllFile);
                    assemblies.Add(assembly);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error loading Plugin " + dllFile, e);
                }
            }
            Type pluginType = typeof(IPlugin);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    try
                    {
                        Type[] types = assembly.GetTypes();
                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }
                            else
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    pluginTypes.Add(type);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ConfigManager.LogError("Error loading Plugin Assembly " + assembly, e);
                    }
                }
            }
            foreach (Type type in pluginTypes)
            {
                try
                {
                    IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                    plug.available.Add(plugin.Name, plugin);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error loading Plugin Type " + type, e);
                }
            }
        }
    }
}
