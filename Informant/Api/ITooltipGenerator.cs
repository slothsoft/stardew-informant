namespace Slothsoft.Informant.Api; 

public interface ITooltipGenerator<in TInput> {

    string Id { get; }
    
    string DisplayName { get; }
    
    bool HasTooltip(TInput input);
    
    Tooltip Generate(TInput input);
}