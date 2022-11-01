using System;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Informant.Api;

namespace Slothsoft.Informant.Implementation.Decorator;

internal class Decorator<TInput> : IDecorator<TInput> {
    
    private readonly Func<TInput, Texture2D?> _decorator;

    public Decorator(string id, string displayName, string description, Func<TInput, Texture2D?> decorator) {
        Id = id;
        DisplayName = displayName;
        Description = description;
        _decorator = decorator;
    }

    public string Id { get; }
    public string DisplayName { get; }
    public string Description { get; }

    public bool HasDecoration(TInput input) {
        return _decorator(input) != null;
    }

    public Decoration Decorate(TInput input) {
        return new Decoration(_decorator(input)!);
    }
}