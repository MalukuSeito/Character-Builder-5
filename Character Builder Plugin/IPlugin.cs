using Character_Builder_5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_Plugin
{
    public interface IPlugin
    {
        string Name { get; }
        List<Feature> filterClassFeatures(ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider);
        List<Feature> filterSubClassFeatures(SubClass subcls, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider);
        List<Feature> filterRaceFeatures(Race race, List<Feature> features, int level, IChoiceProvider provider);
        List<Feature> filterSubRaceFeatures(SubRace subrace, Race race, List<Feature> features, int level, IChoiceProvider provider);
        List<Feature> filterBackgroundFeatures(Background background, List<Feature> features, int level, IChoiceProvider provider);
        List<Feature> filterCommonFeatures(List<Feature> features, int level, IChoiceProvider provider);
        List<Feature> filterFeats(List<Feature> features, int level, IChoiceProvider provider);
        List<Feature> filterBoons(List<Feature> features, int level, IChoiceProvider provider);
        List<Feature> filterPossessionFeatures(List<Feature> features, int level, IChoiceProvider provider);
    }
}
 