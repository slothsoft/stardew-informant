using System.Collections.Generic;

namespace Slothsoft.Informant.Api; 

/// <summary>
/// A general informant that allows hooking new providers for a specific type into.
/// </summary>
public interface IDecoratorInformant<TInput> {

    /// <summary>
    /// Returns the decorators this informant has.
    /// </summary>
    IEnumerable<ITooltipDecorator<TInput>> Decorators { get; }

    /// <summary>
    /// Add a decorator that provides information for a specific type.
    /// </summary>
    /// <param name="decorator">the decorator to add.</param>
    void Add(ITooltipDecorator<TInput> decorator);
    
    /// <summary>
    /// Removes a decorator that provides information for a specific type.
    /// </summary>
    /// <param name="decoratorId">the decorator's ID to remove.</param>
    void Remove(string decoratorId);
}