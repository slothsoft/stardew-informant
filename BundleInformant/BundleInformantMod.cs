using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Slothsoft.BundleInformant;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
public class BundleInformantMod : Mod {

    private Harmony? _harmony;
    
    private static Texture2D? _bundle;
    private static Rectangle? _lastToolTipCoordinates;

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        _bundle = modHelper.ModContent.Load<Texture2D>("assets/bundle.png");

        _harmony = new Harmony(ModManifest.UniqueID);
        _harmony.Patch(
            original: AccessTools.Method(
                typeof(IClickableMenu),
                nameof(IClickableMenu.drawToolTip)
            ),
            postfix: new HarmonyMethod(typeof(BundleInformantMod), nameof(DrawToolTip))
        );
        _harmony.Patch(
            original: AccessTools.Method(
                typeof(IClickableMenu),
                nameof(IClickableMenu.drawTextureBox),
                new[] {
                    typeof(SpriteBatch),
                    typeof(Texture2D),
                    typeof(Rectangle),
                    typeof(int),
                    typeof(int),
                    typeof(int),
                    typeof(int),
                    typeof(Color),
                    typeof(float),
                    typeof(bool),
                    typeof(float)
                }
            ),
            postfix: new HarmonyMethod(typeof(BundleInformantMod), nameof(RememberToolTipCoordinates))
        );
    }

    private static void DrawToolTip(SpriteBatch b, ref string hoverText, string hoverTitle, Item? hoveredItem,
        bool heldItem = false, int healAmountToDisplay = -1, int currencySymbol = 0, int extraItemToShowIndex = -1,
        int extraItemToShowAmount = -1, CraftingRecipe? craftingIngredients = null,
        int moneyAmountToShowAtBottom = -1) {

        if (_lastToolTipCoordinates == null || hoveredItem == null || _bundle == null) {
            return;
        }
        
        var bundles = GetNeededItems().ToArray();
        if (bundles.Contains(hoveredItem.ParentSheetIndex)) {
            const int indent = 16;
            const int scaleFactor = 3;
            var tipCoordinates = _lastToolTipCoordinates.Value;
            var destinationRectangle = new Rectangle(
                tipCoordinates.X + tipCoordinates.Width - _bundle.Width * scaleFactor- indent, 
                tipCoordinates.Y + indent, 
                _bundle.Width * scaleFactor, 
                _bundle.Height * scaleFactor);
            b.Draw(_bundle, destinationRectangle, null, Color.White);
        }
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
    
    private static void RememberToolTipCoordinates(SpriteBatch b, Texture2D texture, Rectangle sourceRect, int x, int y,
        int width, int height, Color color, float scale = 1f, bool drawShadow = true, float draw_layer = -1f) {
        _lastToolTipCoordinates = new Rectangle(x, y, width, height);
    }

    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        _harmony?.UnpatchAll(ModManifest.UniqueID);
        _harmony = null;
    }
}