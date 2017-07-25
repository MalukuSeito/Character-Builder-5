using OGL;
using OGL.Common;
using OGL.Features;
using System.Collections.Generic;

namespace Character_Builder_Plugin
{
    public interface IPlugin
    {
        string Name { get; }
        List<Feature> FilterClassFeatures(ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context);
        List<Feature> FilterSubClassFeatures(SubClass subcls, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context);
        List<Feature> FilterRaceFeatures(Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context);
        List<Feature> FilterSubRaceFeatures(SubRace subrace, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context);
        List<Feature> FilterBackgroundFeatures(Background background, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context);
        List<Feature> FilterCommonFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context);
        List<Feature> FilterFeats(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context);
        List<Feature> FilterBoons(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context);
        List<Feature> FilterPossessionFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context);
    }
}
 