using System;
using Slothsoft.Informant.Api;
using Slothsoft.Informant.Implementation.Common;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation.TooltipGenerator;

internal class FruitTreeTooltipGenerator : ITooltipGenerator<TerrainFeature> {

    private readonly IModHelper _modHelper;
    
    public FruitTreeTooltipGenerator(IModHelper modHelper) {
        _modHelper = modHelper;
    }
    
    public string DisplayName => _modHelper.Translation.Get("FruitTreeTooltipGenerator");
    
    public string Id => "fruit-tree";
    
    public bool HasTooltip(TerrainFeature input) {
        return input is FruitTree;
    }

    public Tooltip Generate(TerrainFeature input) {
        return new Tooltip(CreateText((FruitTree) input));
    }

    private string CreateText(FruitTree fruitTree) {
        var displayName = GameInformation.GetObjectDisplayName(fruitTree.indexOfFruit.Value);
        var daysLeft = CalculateDaysLeftString(fruitTree);
        return $"{displayName}\n{daysLeft}";
    }

    private string CalculateDaysLeftString(FruitTree fruitTree) {
        var daysLeft = CalculateDaysLeft(fruitTree);
        var daysLeftString = daysLeft == 1
            ? _modHelper.Translation.Get("CropTooltipGenerator.1DayLeft")
            : _modHelper.Translation.Get("CropTooltipGenerator.XDaysLeft", new {X = daysLeft});
        return daysLeftString;
    }

    private int CalculateDaysLeft(FruitTree fruitTree) {
        var daysLeft = fruitTree.daysUntilMature.Value;
        if (daysLeft <= 0) {
            // if mature, 0 days are left if there are fruits on the tree, else 1 day
            daysLeft = fruitTree.fruitsOnTree.Value <= 0 ? 1 : 0;
        }
        if (daysLeft > 0) {
            // check that the date we are calculating is in the correct season   
            var futureDay = Game1.dayOfMonth + daysLeft;
            var seasonsLeft = futureDay / Seasons.LengthInDays;
            futureDay %= Seasons.LengthInDays;

            var futureSeasonIndex = Array.IndexOf(Seasons.All, Game1.currentSeason) + seasonsLeft;
            var futureSeason = Seasons.All[futureSeasonIndex % Seasons.All.Length];
            while (futureSeason != fruitTree.fruitSeason.Value) {
                futureSeasonIndex++;
                futureSeason = Seasons.All[futureSeasonIndex % Seasons.All.Length];
                daysLeft += Seasons.LengthInDays - futureDay; // add only the remainder of the month
                futureDay = 0; // and after the remainder was added, all following months are fully added
            }
        }
        return daysLeft;
    }
}