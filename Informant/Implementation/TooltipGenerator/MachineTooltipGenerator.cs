using System.Linq;
using Slothsoft.Informant.Api;
using Slothsoft.Informant.Implementation.Common;
using StardewValley.Objects;

namespace Slothsoft.Informant.Implementation.TooltipGenerator;

internal class MachineTooltipGenerator : ITooltipGenerator<SObject> {

    private readonly IModHelper _modHelper;
    
    public MachineTooltipGenerator(IModHelper modHelper) {
        _modHelper = modHelper;
    }

    public string Id => "machine";
    public string DisplayName => _modHelper.Translation.Get("MachineTooltipGenerator");
    public string Description => _modHelper.Translation.Get("MachineTooltipGenerator.Description");
    
    public bool HasTooltip(SObject input) {
        return HasTooltip(input, InformantMod.Instance?.Config.HideMachineTooltips ?? HideMachineTooltips.ForNonMachines);
    }
    
    internal static bool HasTooltip(SObject input, HideMachineTooltips hideMachineTooltips) {
        if (!input.bigCraftable.Value) return false;
        if (input.ParentSheetIndex == BigCraftableIds.GardenPot) {
            var gardenPot = input as IndoorPot;
            var crop = gardenPot?.hoeDirt.Value.crop;
            return crop != null;
        }
        
        return hideMachineTooltips switch {
            HideMachineTooltips.Never => true,
            HideMachineTooltips.ForChests => !BigCraftableIds.AllChests.Contains(input.ParentSheetIndex),
            _ => !BigCraftableIds.AllChests.Contains(input.ParentSheetIndex) &&
                 !BigCraftableIds.AllStaticCraftables.Contains(input.ParentSheetIndex)
        };
    }

    public Tooltip Generate(SObject input) {
        if (input.ParentSheetIndex == BigCraftableIds.GardenPot) {
            var gardenPot = input as IndoorPot;
            var crop = gardenPot?.hoeDirt.Value.crop;
            return new Tooltip(crop == null ? "???" : CropTooltipGenerator.CreateText(_modHelper, crop));
        }
        return new Tooltip(CreateText(input));
    }

    private string CreateText(SObject input) {
        var displayName = input.DisplayName;
        
        if (input.heldObject.Value == null) {
            return displayName;
        }
        var heldObject = input.heldObject.Value.DisplayName;
        var daysLeft = CalculateMinutesLeftString(input);
        return $"{displayName}\n> {heldObject}\n{daysLeft}";
    }

    internal string CalculateMinutesLeftString(SObject input) {
        switch (input.MinutesUntilReady) {
            case < 0:
                return _modHelper.Translation.Get("MachineTooltipGenerator.CannotBeUnloaded");
            case 0:
                return _modHelper.Translation.Get("MachineTooltipGenerator.Finished");
        }
        var minutesLeft = input.MinutesUntilReady % 60;
        var hoursLeft = (input.MinutesUntilReady / 60) % 24;
        var daysLeft = input.MinutesUntilReady / 60 / 24;
        return $"{daysLeft:D2}:{hoursLeft:D2}:{minutesLeft:D2}";
    }
}