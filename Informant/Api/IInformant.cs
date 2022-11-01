using System;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Api; 

/// <summary>
/// Base class for the entire API. Can be used to add custom information providers.
/// </summary>
public interface IInformant {

    /// <summary>
    /// A manager class for the <see cref="TerrainFeature"/>(s) under the mouse position.
    /// </summary>
    ITooltipGeneratorManager<TerrainFeature> TerrainFeatureTooltipGenerators { get; }

    /// <summary>
    /// Adds a tooltip generator for the <see cref="TerrainFeature"/>(s) under the mouse position.
    /// <br/><b>Since Version:</b> 1.1.0
    /// </summary>
    void AddTerrainFeatureTooltipGenerator(string id, string displayName, string description, Func<TerrainFeature, string> generator); 
    
    /// <summary>
    /// A manager class for the <see cref="SObject"/>(s) under the mouse position.
    /// </summary>
    ITooltipGeneratorManager<SObject> ObjectTooltipGenerators { get; }
    
    /// <summary>
    /// Adds a tooltip generator for the <see cref="Object"/>(s) under the mouse position.
    /// <br/><b>Since Version:</b> 1.1.0
    /// </summary>
    void AddObjectTooltipGenerator(string id, string displayName, string description, Func<SObject, string?> generator); 
    
    /// <summary>
    /// A manager class for decorating a tooltip for an <see cref="Item"/>.
    /// </summary>
    IDecoratorManager<Item> ItemDecorators { get; }
    
    /// <summary>
    /// Adds a decorator for the <see cref="Item"/>(s) under the mouse position.
    /// <br/><b>Since Version:</b> 1.1.0
    /// </summary>
    void AddItemDecorator(string id, string displayName, string description, Func<Item, Texture2D?> decorator); 
}