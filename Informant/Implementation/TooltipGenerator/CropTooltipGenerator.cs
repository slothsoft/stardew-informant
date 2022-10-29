using System.Linq;
using Slothsoft.Informant.Api;
using Slothsoft.Informant.Implementation.Common;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation.TooltipGenerator;

internal class CropTooltipGenerator : ITooltipGenerator<TerrainFeature> {

    private readonly IModHelper _modHelper;
    
    public CropTooltipGenerator(IModHelper modHelper) {
        _modHelper = modHelper;
    }

    public string DisplayName => _modHelper.Translation.Get("CropTooltipGenerator");
    public string Description => _modHelper.Translation.Get("CropTooltipGenerator.Description");
    
    public string Id => "crop";

    public bool HasTooltip(TerrainFeature input) {
        return input is HoeDirt {crop: { }};
    }

    public Tooltip Generate(TerrainFeature input) {
        return new Tooltip(CreateText(((HoeDirt) input).crop));
    }

    private string CreateText(Crop crop) {
        var displayName = GameInformation.GetObjectDisplayName(crop.indexOfHarvest.Value);
        var daysLeft = CalculateDaysLeftString(crop);
        return $"{displayName}\n{daysLeft}";
    }

    private string CalculateDaysLeftString(Crop crop) {
        if (crop.dead.Value) {
            return _modHelper.Translation.Get("CropTooltipGenerator.Dead");
        }

        var daysLeft = CalculateDaysLeft(crop);
        var daysLeftString = daysLeft == 1
            ? _modHelper.Translation.Get("CropTooltipGenerator.1DayLeft")
            : _modHelper.Translation.Get("CropTooltipGenerator.XDaysLeft", new {X = daysLeft});
        return daysLeftString;
    }

    private static int CalculateDaysLeft(Crop crop) {
        var currentPhase = crop.currentPhase.Value;
        var dayOfCurrentPhase = crop.dayOfCurrentPhase.Value;
        var regrowAfterHarvest = crop.regrowAfterHarvest.Value;
        var cropPhaseDays = crop.phaseDays.ToArray();

        // Amaranth:  current = 4 | day = 0 | days = 1, 2, 2, 2, 99999 | result => 0
        // Fairy Rose:  current = 4 | day = 1 | days = 1, 4, 4, 3, 99999 | result => 0
        // Cranberry:  current = 5 | day = 4 | days = 1, 2, 1, 1, 2, 99999 | result => ???
        // Ancient Fruit: current = 5 | day = 4 | days = 1 5 5 6 4 99999 | result => 4
        // Blueberry (harvested): current = 5 | day = 4 | days = 1 3 3 4 2 99999 | regrowAfterHarvest = 4 | result => 4
        // Blueberry (harvested): current = 5 | day = 0 | days = 1 3 3 4 2 99999 | regrowAfterHarvest = 4 | result => 0
        var result = 0;
        for (var phase = currentPhase; phase < cropPhaseDays.Length; phase++) {
            if (cropPhaseDays[phase] < 99999) {
                result += cropPhaseDays[phase];
                if (phase == currentPhase) {
                    result -= dayOfCurrentPhase;
                }
            } else if (currentPhase == cropPhaseDays.Length - 1 && regrowAfterHarvest > 0) {
                // calculate the repeating harvests, it seems the dayOfCurrentPhase counts backwards now
                result = dayOfCurrentPhase;
            }
        }

        return result;
    }
}