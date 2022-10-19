using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Slothsoft.CropInformant;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
public class CropInformantMod : Mod {
    private string? _hoverText;

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        modHelper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        modHelper.Events.Display.Rendered += OnRendered;
    }

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e) {
        if (!Context.IsPlayerFree) {
            return;
        }

        _hoverText = null;

        var mousePosX = Game1.getOldMouseX();
        var mousePosY = Game1.getOldMouseY();
        var crops = FetchCrops(Game1.currentLocation);

        foreach (var crop in crops) {
            var boundaries = new RectangleF(
                crop.currentTileLocation.X * Game1.tileSize - Game1.viewport.X,
                crop.currentTileLocation.Y * Game1.tileSize - Game1.viewport.Y,
                Game1.tileSize,
                Game1.tileSize);

            if (boundaries.Contains(mousePosX, mousePosY)) {
                _hoverText = CreateHoverText(crop.crop);
                break;
            }
        }
    }

    private IEnumerable<HoeDirt> FetchCrops(GameLocation location) {
        return location.terrainFeatures.Values.OfType<HoeDirt>().Where(d => d.crop != null);
    }

    private string CreateHoverText(Crop crop) {
        var displayName = GetDisplayName(crop.indexOfHarvest.Value);
        var daysLeft = CalculateDaysLeftString(crop);
        return $"{displayName}\n{daysLeft}";
    }

    private static string GetDisplayName(int parentSheetIndex) {
        Game1.objectInformation.TryGetValue(parentSheetIndex, out var str);
        if (string.IsNullOrEmpty(str))
            return "???";
        return str.Split('/')[4];
    }

    private string CalculateDaysLeftString(Crop crop) {
        if (crop.dead.Value) {
            return Helper.Translation.Get("Dead");
        }
        
        var daysLeft = CalculateDaysLeft(crop);
        var daysLeftString = daysLeft == 1
            ? Helper.Translation.Get("1DayLeft")
            : Helper.Translation.Get("XDaysLeft", new {X = daysLeft});
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
            } else if (currentPhase == cropPhaseDays.Length -1 && regrowAfterHarvest > 0) {
                // calculate the repeating harvests, it seems the dayOfCurrentPhase counts backwards now
                result = dayOfCurrentPhase;
            }
        }
        return result;
    }

    private void OnRendered(object? sender, RenderedEventArgs e) {
        if (Context.IsPlayerFree && _hoverText != null) {
            DrawSimpleTooltip(Game1.spriteBatch, _hoverText, Game1.smallFont);
        }
    }

    private static void DrawSimpleTooltip(SpriteBatch b, string hoverText, SpriteFont font) {
        var textSize = font.MeasureString(hoverText);
        var width = (int) textSize.X + Game1.tileSize / 2;
        var height = Math.Max(60, (int) textSize.Y + Game1.tileSize / 2);
        var x = Game1.getOldMouseX() + Game1.tileSize / 2;
        var y = Game1.getOldMouseY() + Game1.tileSize / 2;

        if (x + width > Game1.viewport.Width) {
            x = Game1.viewport.Width - width;
            y += Game1.tileSize / 4;
        }

        if (y + height > Game1.viewport.Height) {
            x += Game1.tileSize / 4;
            y = Game1.viewport.Height - height;
        }

        IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height,
            Color.White);

        var position = new Vector2(x + Game1.tileSize / 4, y + Game1.tileSize / 4 + 4);
        b.DrawString(font, hoverText, position + new Vector2(2f, 2f), Game1.textShadowColor, 0, Vector2.Zero, 1f,
            SpriteEffects.None, 0);
        b.DrawString(font, hoverText, position + new Vector2(0f, 2f), Game1.textShadowColor, 0, Vector2.Zero, 1f,
            SpriteEffects.None, 0);
        b.DrawString(font, hoverText, position + new Vector2(2f, 0f), Game1.textShadowColor, 0, Vector2.Zero, 1f,
            SpriteEffects.None, 0);
        b.DrawString(font, hoverText, position, Game1.textColor * 0.9f, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
    }
}