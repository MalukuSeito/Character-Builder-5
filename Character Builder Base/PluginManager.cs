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
        public event EventHandler PluginsChanged;
        public Dictionary<string, IPlugin> available = new Dictionary<string, IPlugin>(StringComparer.OrdinalIgnoreCase);
        public List<IPlugin> plugins = new List<IPlugin>();
        public void FireEvent(PluginManager plug, EventArgs args) => PluginsChanged?.Invoke(plug, args);
        public void Load(List<string> rules)
        {
            plugins.Clear();
            if (rules != null) foreach (string p in rules) if (available.ContainsKey(p)) plugins.Add(available[p]);
            FireEvent(this, EventArgs.Empty);
        }
        public List<Feature> FilterBackgroundFeatures(Background background, List<Feature> features, int level, IChoiceProvider provider, OGLContext context)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.FilterBackgroundFeatures(background, features, level, provider, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> FilterBoons(List<Feature> features, int level, IChoiceProvider provider, OGLContext context)
        {
            foreach (IPlugin i in plugins) {
                try { features = i.FilterBoons(features, level, provider, context); }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> FilterClassFeatures(ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext context)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.FilterClassFeatures(cls, classlevel, features, level, provider, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> FilterCommonFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext context)
        {
            foreach (IPlugin i in plugins) {
                try
                {
                    features = i.FilterCommonFeatures(features, level, provider, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> FilterFeats(List<Feature> features, int level, IChoiceProvider provider, OGLContext context)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.FilterFeats(features, level, provider, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> FilterPossessionFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext context)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.FilterPossessionFeatures(features, level, provider, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> FilterRaceFeatures(Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext context)
        {
            foreach (IPlugin i in plugins) {
                try
                {
                    features = i.FilterRaceFeatures(race, features, level, provider, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> FilterSubClassFeatures(SubClass subcls, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext context)
        {
            foreach (IPlugin i in plugins)
            {
                try
                {
                    features = i.FilterSubClassFeatures(subcls, cls, classlevel, features, level, provider, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error in Plugin " + i.Name, e);
                }
            }
            return features;
        }

        public List<Feature> FilterSubRaceFeatures(SubRace subrace, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext context)
        {
            foreach (IPlugin i in plugins) {
                try
                {
                    features = i.FilterSubRaceFeatures(subrace, race, features, level, provider, context);
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
