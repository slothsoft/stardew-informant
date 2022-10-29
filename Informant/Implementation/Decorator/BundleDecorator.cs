using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Informant.Api;

namespace Slothsoft.Informant.Implementation.Decorator;

internal class BundleDecorator : IDecorator<Item> {

    private static Texture2D? _bundle;
    
    private readonly IModHelper _modHelper;
    
    public BundleDecorator(IModHelper modHelper) {
        _modHelper = modHelper;
        _bundle ??= modHelper.ModContent.Load<Texture2D>("assets/bundle.png");
    }

    public string Id => "bundles";
    public string DisplayName => _modHelper.Translation.Get("BundleTooltipDecorator");
    public string Description => _modHelper.Translation.Get("BundleTooltipDecorator.Description");

    public bool HasDecoration(Item input) {
        if (_bundle != null) {
            return GetNeededItems().Contains(input.ParentSheetIndex);
        }
        return false;
    }

    private static IEnumerable<int> GetNeededItems() {
        // BUNDLE DATA
        // ============
        // ParentSheetIndex Stack Quality (-> BundleGenerator.ParseItemList)
        // Examples:
        //
        // Pantry/0
        // Spring Crops/O 465 20/24 1 0 188 1 0 190 1 0 192 1 0/0/4/0
        //
        // Boiler Room/22
        // Adventurer's/R 518 1/766 99 0 767 10 0 768 1 0 881 10 0/1/2/22

        var bundleData = Game1.netWorldState.Value.BundleData;
        var bundlesCompleted = Game1.netWorldState.Value.Bundles.Pairs
            .ToDictionary(p => p.Key, p => p.Value.ToArray());

        foreach (var bundleTitle in bundleData.Keys) {
            var bundleIndex = Convert.ToInt32(bundleTitle.Split('/')[1]);
            var bundleDataSplit = bundleData[bundleTitle].Split('/');
            var indexStackQuality = bundleDataSplit[2].Split(' ');
            for (var index = 0; index < indexStackQuality.Length; index += 3) {
                if (!bundlesCompleted[bundleIndex][index / 3]) {
                    var parentSheetIndex = Convert.ToInt32(indexStackQuality[index]);
                    if (parentSheetIndex > 0) {
                        yield return parentSheetIndex;
                    }
                }
            }
        }
    }
    
    public Decoration Decorate(Item input) {
        return new Decoration(_bundle!);
    }
}