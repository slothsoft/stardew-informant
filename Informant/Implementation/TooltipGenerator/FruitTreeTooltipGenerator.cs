﻿using System;
using Microsoft.Xna.Framework;
using Slothsoft.Informant.Api;
using Slothsoft.Informant.Implementation.Common;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation.TooltipGenerator;

internal class FruitTreeTooltipGenerator : ITooltipGenerator<TerrainFeature> {

    private readonly IModHelper _modHelper;
    
    public FruitTreeTooltipGenerator(IModHelper modHelper) {
        _modHelper = modHelper;
    }
    
    public string Id => "fruit-tree";
    public string DisplayName => _modHelper.Translation.Get("FruitTreeTooltipGenerator");
    public string Description => _modHelper.Translation.Get("FruitTreeTooltipGenerator.Description");
    
    public bool HasTooltip(TerrainFeature input) {
        return input is FruitTree;
    }

    public Tooltip Generate(TerrainFeature input) {
        return CreateTooltip((FruitTree) input);
    }

    private Tooltip CreateTooltip(FruitTree fruitTree) {
        var displayName = fruitTree.GetDisplayName();
        var daysLeft = CropTooltipGenerator.ToDaysLeftString(_modHelper, CalculateDaysLeft(fruitTree));
        var icon = fruitTree.fruit.Count == 0 ? null :
            Icon.ForParentSheetIndex(
                    fruitTree.fruit[0].ParentSheetIndex,
                    false,
                    IPosition.CenterRight,
                    new Vector2(Game1.tileSize / 2, Game1.tileSize / 2)
                );
        return new Tooltip($"{displayName}\n{daysLeft}") { Icon = icon };
    }

    internal int CalculateDaysLeft(FruitTree fruitTree) {
        var daysLeft = fruitTree.daysUntilMature.Value;
        if (daysLeft <= 0) {
            // if mature, 0 days are left if there are fruits on the tree, else 1 day
            daysLeft = fruitTree.fruit.Count <= 0 ? 1 : 0;
        }
        if (daysLeft > 0) {
            if (fruitTree.Location.IsGreenhouse) {
                // if we are in the greenhouse, we don't need to add anything for seasons
                return daysLeft;
            }
            if (fruitTree.Location.InIslandContext()) {
                // if we are on the island, we don't need to add anything for seasons
                return daysLeft;
            }
            // check that the date we are calculating is in the correct season
            var futureDay = Game1.Date.DayOfMonth + daysLeft;
            int seasonsLeft = futureDay / WorldDate.DaysPerMonth;
            futureDay %= WorldDate.DaysPerMonth;

            int futureSeasonIndex = Game1.Date.SeasonIndex + seasonsLeft;
            futureSeasonIndex %= WorldDate.MonthsPerYear;
            var futureSeason = (Season)futureSeasonIndex;
            while (!fruitTree.GetData()?.Seasons.Contains(futureSeason) ?? false) {
                futureSeasonIndex++;
                futureSeason = (Season)futureSeasonIndex;
                daysLeft += WorldDate.DaysPerMonth - futureDay; // add only the remainder of the month
                futureDay = 0; // and after the remainder was added, all following months are fully added

                if (daysLeft > WorldDate.DaysPerYear) {
                    // daysLeft is now more than one year - which might happen if the fruitSeason is unknown
                    // (or I misspelled "winter" in the seasons constants) -> just ignore this
                    return -1;
                }
            }
        }
        return daysLeft;
    }
}