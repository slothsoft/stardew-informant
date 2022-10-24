namespace Slothsoft.Informant.Api; 

public interface ITooltipGenerator<in TInput> {

    bool HasTooltip(TInput input);
    
    Tooltip Generate(TInput input);
}