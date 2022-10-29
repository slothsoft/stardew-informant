using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Api; 

/// <summary>
/// Base class for the entire API. Can be used to add custom information providers.
/// </summary>
public interface IInformant {

    /// <summary>
    /// An informant class for the <see cref="TerrainFeature"/>(s) under the mouse position.
    /// </summary>
    ITooltipGeneratorManager<TerrainFeature> TerrainFeatureTooltipsGenerator { get; }
    
    /// <summary>
    /// An informant class for the <see cref="SObject"/>(s) under the mouse position.
    /// </summary>
    ITooltipGeneratorManager<SObject> ObjectTooltipsGenerator { get; }
    
    /// <summary>
    /// An informant class for decorating a tooltip for an <see cref="Item"/>.
    /// </summary>
    IDecoratorManager<Item> ItemDecorators { get; }
}