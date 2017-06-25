using Character_Builder_Plugin;
using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class PluginManager : IPlugin
    {
        string IPlugin.Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public static event EventHandler PluginsChanged;
        public static PluginManager manager;
        public Dictionary<string, IPlugin> available = new Dictionary<string, IPlugin>(StringComparer.InvariantCultureIgnoreCase);
        public List<IPlugin> plugins = new List<IPlugin>();
        public void Load(List<string> rules)
        {
            plugins.Clear();
            if (rules != null) foreach (string p in rules) if (available.ContainsKey(p)) plugins.Add(available[p]);
            PluginsChanged?.Invoke(this, EventArgs.Empty);
        }
        public PluginManager(string path)
        {
            string[] dllFileNames = null;
            if (Directory.Exists(path))
            {
                try
                {
                    dllFileNames = Directory.GetFiles(path, "*.dll");
                } catch (Exception e)
                {
                    ConfigManager.LogError("Error loading Plugins", e);
                }
            } else
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
                    available.Add(plugin.Name, plugin);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error loading Plugin Type " + type, e);
                }
            }
        }
        public List<Feature> filterBackgroundFeatures(Background background, List<Feature> features, int level, IChoiceProvider provider)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.filterBackgroundFeatures(background, features, level, provider);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> filterBoons(List<Feature> features, int level, IChoiceProvider provider)
        {
            foreach (IPlugin i in plugins) {
                try { features = i.filterBoons(features, level, provider); }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> filterClassFeatures(ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.filterClassFeatures(cls, classlevel, features, level, provider);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> filterCommonFeatures(List<Feature> features, int level, IChoiceProvider provider)
        {
            foreach (IPlugin i in plugins) {
                try
                {
                    features = i.filterCommonFeatures(features, level, provider);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> filterFeats(List<Feature> features, int level, IChoiceProvider provider)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.filterFeats(features, level, provider);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> filterPossessionFeatures(List<Feature> features, int level, IChoiceProvider provider)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.filterPossessionFeatures(features, level, provider);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> filterRaceFeatures(Race race, List<Feature> features, int level, IChoiceProvider provider)
        {
            foreach (IPlugin i in plugins) {
                try
                {
                    features = i.filterRaceFeatures(race, features, level, provider);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> filterSubClassFeatures(SubClass subcls, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.filterSubClassFeatures(subcls, cls, classlevel, features, level, provider);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> filterSubRaceFeatures(SubRace subrace, Race race, List<Feature> features, int level, IChoiceProvider provider)
        {
            foreach (IPlugin i in plugins) {
                try
                {
                    features = i.filterSubRaceFeatures(subrace, race, features, level, provider);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }
    }
}
