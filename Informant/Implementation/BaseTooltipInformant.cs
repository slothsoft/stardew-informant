using System.Collections.Generic;
using System.Linq;
using Slothsoft.Informant.Api;

namespace Slothsoft.Informant.Implementation; 

internal class BaseTooltipInformant<TInput> : ITooltipInformant<TInput> {

    private readonly List<ITooltipGenerator<TInput>> _generators = new();

    public IEnumerable<string> GeneratorIds => _generators.Select(g => g.DisplayName);

    internal IEnumerable<Tooltip> Generate(params TInput[] inputs) {
        return _generators
            .SelectMany(g => inputs
                .Where(g.HasTooltip)
                .Select(g.Generate)
            );
    }
    
    public void Add(ITooltipGenerator<TInput> generator) {
        _generators.Add(generator);
    }

    public void Remove(string generatorId) {
        _generators.RemoveAll(g => g.Id == generatorId);
    }
}