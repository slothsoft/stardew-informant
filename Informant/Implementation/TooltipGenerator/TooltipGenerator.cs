using System;
using Slothsoft.Informant.Api;

namespace Slothsoft.Informant.Implementation.TooltipGenerator;

internal class TooltipGenerator<TInput> : ITooltipGenerator<TInput> {
    
    private readonly Func<TInput, string?> _generator;

    public TooltipGenerator(string id, string displayName, string description, Func<TInput, string?> generator) {
        Id = id;
        DisplayName = displayName;
        Description = description;
        _generator = generator;
    }

    public string Id { get; }
    public string DisplayName { get; }
    public string Description { get; }

    public bool HasTooltip(TInput input) {
        return _generator(input) != null;
    }

    public Tooltip Generate(TInput input) {
        return new Tooltip(_generator(input)!);
    }
}