using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Api; 

/// <summary>
/// Base class for the entire API. Can be used to add custom information providers.
/// </summary>
public interface IInformant {

    /// <summary>
    /// An informant class for the <see cref="TerrainFeature"/>(s) under the mouse position.
    /// </summary>
    ITooltipInformant<TerrainFeature> TerrainFeatureTooltips { get; }
    
    /// <summary>
    /// An informant class for the <see cref="SObject"/>(s) under the mouse position.
    /// </summary>
    ITooltipInformant<SObject> ObjectTooltips { get; }
    
    /// <summary>
    /// An informant class for decorating a tooltip for an <see cref="Item"/>.
    /// </summary>
    IDecoratorInformant<Item> ItemDecorators { get; }
}