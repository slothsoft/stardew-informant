using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Informant.Api;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation; 

internal class TooltipInformant : ITooltipInformant<TerrainFeature> {

    private readonly IModHelper _modHelper;
    private BaseTooltipInformant<TerrainFeature>? _terrainFeatureInformant;
    
    private Tooltip? _tooltip;
        
    public TooltipInformant(IModHelper modHelper) {
        _modHelper = modHelper;
        
        modHelper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        modHelper.Events.Display.Rendered += OnRendered;
    }
    
    public IEnumerable<string> GeneratorIds => _terrainFeatureInformant?.GeneratorIds ?? Enumerable.Empty<string>();

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e2) {
        if (!Context.IsPlayerFree) {
            return;
        }

        _tooltip = GenerateTerrainFeatureTooltip();
    }

    private Tooltip? GenerateTerrainFeatureTooltip() {
        if (_terrainFeatureInformant == null) {
            // if there is no generator in that, we don't need to do anything further
            return null;
        }
        
        var mousePosX = (Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize;
        var mousePosY = (Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize;
        
        return _terrainFeatureInformant.Generate(
            Game1.currentLocation.terrainFeatures.Values
                .Where(t => mousePosX == (int) t.currentTileLocation.X && mousePosY == (int) t.currentTileLocation.Y)
                .ToArray()
        ).SingleOrDefault();
    }

    private void OnRendered(object? sender, RenderedEventArgs e) {
        if (Context.IsPlayerFree && _tooltip != null) {
            DrawSimpleTooltip(Game1.spriteBatch, _tooltip.Text, Game1.smallFont);
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

        IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height, Color.White);

        var position = new Vector2(x + Game1.tileSize / 4, y + Game1.tileSize / 4 + 4);
        b.DrawString(font, hoverText, position + new Vector2(2f, 2f), Game1.textShadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        b.DrawString(font, hoverText, position + new Vector2(0f, 2f), Game1.textShadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        b.DrawString(font, hoverText, position + new Vector2(2f, 0f), Game1.textShadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        b.DrawString(font, hoverText, position, Game1.textColor * 0.9f, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
    }
    
    public void Add(ITooltipGenerator<TerrainFeature> generator) {
        _terrainFeatureInformant ??= new BaseTooltipInformant<TerrainFeature>();
        _terrainFeatureInformant.Add(generator);
    }

    public void Remove(string generatorId) {
        _terrainFeatureInformant?.Remove(generatorId);
    }
}