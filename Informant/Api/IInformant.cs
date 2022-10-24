using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Api; 

public interface IInformant {

    ITooltipInformant<TerrainFeature> TerrainFeatureInformant { get; }
}