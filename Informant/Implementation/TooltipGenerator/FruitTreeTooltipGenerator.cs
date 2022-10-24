using Slothsoft.Informant.Api;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation.TooltipGenerator;

internal class FruitTreeTooltipGenerator : ITooltipGenerator<TerrainFeature> {

    private readonly IModHelper _modHelper;
    
    public FruitTreeTooltipGenerator(IModHelper modHelper) {
        _modHelper = modHelper;
    }
    
    public bool HasTooltip(TerrainFeature input) {
        return input is FruitTree;
    }

    public Tooltip Generate(TerrainFeature input) {
        return new Tooltip(CreateText((FruitTree) input));
    }

    private string CreateText(FruitTree fruitTree) {
        var displayName = GetDisplayName(fruitTree.indexOfFruit.Value);
        var daysLeft = CalculateDaysLeftString(fruitTree);
        return $"{displayName}\n{daysLeft}";
    }

    private static string GetDisplayName(int parentSheetIndex) {
        Game1.objectInformation.TryGetValue(parentSheetIndex, out var str);
        if (string.IsNullOrEmpty(str))
            return "???";
        return str.Split('/')[4];
    }

    private string CalculateDaysLeftString(FruitTree fruitTree) {
        var daysLeft = fruitTree.daysUntilMature.Value;
        if (daysLeft == 0) {
            // if mature, 0 days are left if there are fruits on the tree, else 1 day
            daysLeft = fruitTree.fruitsOnTree.Value <= 0 ? 1 : 0;
        }
        var daysLeftString = daysLeft == 1
            ? _modHelper.Translation.Get("CropTooltipGenerator.1DayLeft")
            : _modHelper.Translation.Get("CropTooltipGenerator.XDaysLeft", new {X = daysLeft});
        return daysLeftString;
    }
}