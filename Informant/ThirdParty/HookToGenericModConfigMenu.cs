using System;
using System.Collections.Generic;
using Slothsoft.Informant.Api;

namespace Slothsoft.Informant.ThirdParty; 

internal static class HookToGenericModConfigMenu {
    
    public static void Apply(InformantMod informantMod, IInformant api) {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = informantMod.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null)
            return;

        // register mod
        configMenu.Register(
            mod: informantMod.ModManifest,
            reset: () => informantMod.Config = new InformantConfig(),
            save: () => informantMod.Helper.WriteConfig(informantMod.Config)
        );

        // add some config options
        var configurables = new List<IDisplayable>();
        configurables.AddRange(api.ItemDecorators.Decorators);
        configurables.AddRange(api.ObjectTooltipGenerators.Generators);
        configurables.AddRange(api.TerrainFeatureTooltipGenerators.Generators);
        configurables.Sort((a, b) => string.Compare(a.DisplayName, b.DisplayName, StringComparison.CurrentCulture));
        
        foreach (var configurable in configurables) {
            configMenu.AddBoolOption(
                mod: informantMod.ModManifest,
                name: () => configurable.DisplayName,
                tooltip: () => configurable.Description,
                getValue: () => informantMod.Config.DisplayIds.GetValueOrDefault(configurable.Id, true),
                setValue: value => informantMod.Config.DisplayIds[configurable.Id] = value
            );
        }
    }
}