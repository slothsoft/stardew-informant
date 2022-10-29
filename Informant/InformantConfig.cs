using System.Collections.Generic;

namespace Slothsoft.Informant;

internal record InformantConfig {
    public Dictionary<string, bool> DisplayIds { get; set; } = new();
}