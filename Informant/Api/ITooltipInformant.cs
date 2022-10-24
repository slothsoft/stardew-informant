using System.Collections.Generic;

namespace Slothsoft.Informant.Api; 

public interface ITooltipInformant<TInput> {

    IEnumerable<Tooltip> Generate(params TInput[] inputs);

    void Add(ITooltipGenerator<TInput> generator);
    
    void Remove(ITooltipGenerator<TInput> generator);
}