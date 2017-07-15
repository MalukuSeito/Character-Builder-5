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
        public Dictionary<string, IPlugin> available = new Dictionary<string, IPlugin>(StringComparer.OrdinalIgnoreCase);
        public List<IPlugin> plugins = new List<IPlugin>();
        public static void FireEvent(PluginManager plug, EventArgs args) => PluginsChanged?.Invoke(plug, args);
        public void Load(List<string> rules)
        {
            plugins.Clear();
            if (rules != null) foreach (string p in rules) if (available.ContainsKey(p)) plugins.Add(available[p]);
            PluginManager.FireEvent(this, EventArgs.Empty);
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
