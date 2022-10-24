using Slothsoft.Informant.Api;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation; 

internal class Informant : IInformant {

    private readonly IModHelper _modHelper;
    
    private TooltipInformant? _terrainFeatureInformant;
    private ItemDecoratorInformant? _itemDecoratorInformant;
    
    public ITooltipInformant<TerrainFeature> TerrainFeatureTooltips {
        get {
            _terrainFeatureInformant ??= new TooltipInformant(_modHelper);
            return _terrainFeatureInformant;
        }
    }
    
    public ITooltipInformant<SObject> ObjectTooltips {
        get {
            _terrainFeatureInformant ??= new TooltipInformant(_modHelper);
            return _terrainFeatureInformant;
        }
    }
    
    public IDecoratorInformant<Item> ItemDecorators {
        get {
            _itemDecoratorInformant ??= new ItemDecoratorInformant(_modHelper);
            return _itemDecoratorInformant;
        }
    }
    
    public Informant(IModHelper modHelper) {
        _modHelper = modHelper;
    }
}