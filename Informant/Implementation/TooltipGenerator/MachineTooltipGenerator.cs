using Slothsoft.Informant.Api;

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
        return input.bigCraftable.Value;
    }

    public Tooltip Generate(SObject input) {
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

    private string CalculateMinutesLeftString(SObject input) {
        var minutesLeft = input.MinutesUntilReady % 60;
        var hoursLeft = (input.MinutesUntilReady / 60) % 24;
        var daysLeft = input.MinutesUntilReady / 60 / 24;
        return $"{daysLeft:D2}:{hoursLeft:D2}:{minutesLeft:D2}";
    }
}