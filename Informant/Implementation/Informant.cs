using Slothsoft.Informant.Api;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation; 

public class Informant : IInformant {

    private readonly IModHelper _modHelper;
    
    private TooltipGeneratorManager? _terrainFeatureInformant;
    private ItemDecoratorManager? _itemDecoratorInformant;
    
    public ITooltipGeneratorManager<TerrainFeature> TerrainFeatureTooltipGenerators {
        get {
            _terrainFeatureInformant ??= new TooltipGeneratorManager(_modHelper);
            return _terrainFeatureInformant;
        }
    }
    
    public ITooltipGeneratorManager<SObject> ObjectTooltipGenerators {
        get {
            _terrainFeatureInformant ??= new TooltipGeneratorManager(_modHelper);
            return _terrainFeatureInformant;
        }
    }
    
    public IDecoratorManager<Item> ItemDecorators {
        get {
            _itemDecoratorInformant ??= new ItemDecoratorManager(_modHelper);
            return _itemDecoratorInformant;
        }
    }
    
    public Informant(IModHelper modHelper) {
        _modHelper = modHelper;
    }
}