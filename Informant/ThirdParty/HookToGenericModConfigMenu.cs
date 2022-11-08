using System;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.Informant.Api;
using Slothsoft.Informant.Implementation;
using Slothsoft.Informant.Implementation.Decorator;

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
        
        // add some config options for tooltip generators
        configMenu.AddSectionTitle(informantMod.ModManifest, () => informantMod.Helper.Translation.Get("TooltipGenerators.GeneralSection"));
        configMenu.AddEnumOption(
            mod: informantMod.ModManifest,
            name: () => informantMod.Helper.Translation.Get("TooltipTrigger"),
            getValue: () => informantMod.Config.TooltipTrigger,
            setValue: value => informantMod.Config.TooltipTrigger = value,
            getDisplayName: value => informantMod.Helper.Translation.Get("TooltipTrigger." + value)
        );
        configMenu.AddKeybind(
            mod: informantMod.ModManifest,
            name: () => informantMod.Helper.Translation.Get("TooltipTriggerButton"),
            getValue: () => informantMod.Config.TooltipTriggerButton,
            setValue: value => informantMod.Config.TooltipTriggerButton = value
        );
        configMenu.AddEnumOption(
            mod: informantMod.ModManifest,
            name: () => informantMod.Helper.Translation.Get("HideMachineTooltips"),
            getValue: () => informantMod.Config.HideMachineTooltips,
            setValue: value => informantMod.Config.HideMachineTooltips = value,
            getDisplayName: value => informantMod.Helper.Translation.Get("HideMachineTooltips." + value)
        );
        
        configMenu.AddSectionTitle(informantMod.ModManifest, () => informantMod.Helper.Translation.Get("TooltipGenerators.Visibility"));
        var configurables = new List<IDisplayable>();
        configurables.AddRange(api.ObjectTooltipGenerators.Generators);
        configurables.AddRange(api.TerrainFeatureTooltipGenerators.Generators);
        CreateDisplayableOptions(configMenu, configurables, informantMod);
        
        // add some config options for decorators
        // TODO: commong soon? configMenu.AddSectionTitle(informantMod.ModManifest, () => informantMod.Helper.Translation.Get("Decorators.GeneralSection"));
        configMenu.AddSectionTitle(informantMod.ModManifest, () => informantMod.Helper.Translation.Get("Decorators.Visibility"));
        configurables = new List<IDisplayable>();
        configurables.AddRange(api.ItemDecorators.Decorators);
        configurables.Add(new Decorator<object>(SellPricePainter.Id, informantMod.Helper.Translation.Get("SellPricePainter"), 
            informantMod.Helper.Translation.Get("SellPricePainter.Description"), _ => null)); 
        CreateDisplayableOptions(configMenu, configurables, informantMod);
    }
    
    private static void AddEnumOption<TEnum>(this IGenericModConfigMenuApi configMenu, IManifest mod, Func<TEnum> getValue, Action<TEnum> setValue, 
        Func<string> name, Func<TEnum, string> getDisplayName) where TEnum: notnull {
        var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
        var enumDisplayNames = enumValues.Select(getDisplayName).ToArray();
        
        configMenu.AddTextOption(
            mod: mod,
            name: name,
            getValue: () => getDisplayName(getValue())!,
            setValue: value => setValue(enumValues[Array.IndexOf(enumDisplayNames, value)]),
            allowedValues: enumDisplayNames
        );
    }

    private static void CreateDisplayableOptions(IGenericModConfigMenuApi configMenu, IEnumerable<IDisplayable> configurables, InformantMod informantMod) {
        foreach (var configurable in configurables.OrderBy(d => d.DisplayName)) {
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