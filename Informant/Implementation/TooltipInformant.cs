using System.Collections.Generic;
using System.Linq;
using Slothsoft.Informant.Api;

namespace Slothsoft.Informant.Implementation; 

internal class TooltipInformant<TInput> : ITooltipInformant<TInput> {

    private readonly List<ITooltipGenerator<TInput>> _generators = new();

    public void Add(ITooltipGenerator<TInput> generator) {
        _generators.Add(generator);
    }

    public void Remove(ITooltipGenerator<TInput> generator) {
        _generators.Remove(generator);
    }
    
    public IEnumerable<Tooltip> Generate(params TInput[] inputs) {
        return _generators
            .SelectMany(g => inputs
                .Where(g.HasTooltip)
                .Select(g.Generate)
            );
    }
}