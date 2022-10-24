using Slothsoft.Informant.Api;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation; 

internal class Informant : IInformant {

    private readonly IModHelper _modHelper;
    
    private TooltipInformant? _terrainFeatureInformant;
    
    public ITooltipInformant<TerrainFeature> TerrainFeatureInformant {
        get {
            _terrainFeatureInformant ??= new TooltipInformant(_modHelper);
            return _terrainFeatureInformant;
        }
    }
    
    public ITooltipInformant<SObject> ObjectInformant {
        get {
            _terrainFeatureInformant ??= new TooltipInformant(_modHelper);
            return _terrainFeatureInformant;
        }
    }
    
    public Informant(IModHelper modHelper) {
        _modHelper = modHelper;
    }
}