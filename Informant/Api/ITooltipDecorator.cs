namespace Slothsoft.Informant.Api; 

/// <summary>
/// A class that decorates a vanilla tooltip using <see cref="Decoration"/>.
/// </summary>
/// <typeparam name="TInput">input object.</typeparam>
public interface ITooltipDecorator<in TInput> {

    /// <summary>
    /// The unique ID of the decorator. 
    /// </summary>
    string Id { get; }
    
    /// <summary>
    /// The display name of the decorator for the configuration. 
    /// </summary>
    string DisplayName { get; }
    
    /// <summary>
    /// Returns true if <see cref="Decorate"/> should be called on this object. 
    /// </summary>
    bool HasDecoration(TInput input);
    
    /// <summary>
    /// Generates the decorator for this object.
    /// </summary>
    Decoration Decorate(TInput input);
}