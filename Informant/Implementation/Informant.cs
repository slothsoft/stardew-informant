using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Informant.Api;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation; 

internal class Informant : IInformant {

    private readonly IModHelper _modHelper;
    
    private ITooltipInformant<TerrainFeature>? _terrainFeatureInformant;
    
    private Tooltip? _tooltip;
    
    public ITooltipInformant<TerrainFeature> TerrainFeatureInformant {
        get {
            _terrainFeatureInformant ??= new TooltipInformant<TerrainFeature>();
            return _terrainFeatureInformant;
        }
    }
    
    public Informant(IModHelper modHelper) {
        _modHelper = modHelper;
        _modHelper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        _modHelper.Events.Display.Rendered += OnRendered;
    }

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e) {
        if (!Context.IsPlayerFree) {
            return;
        }

        _tooltip = null;

        if (_terrainFeatureInformant == null) {
            // no listeners means we don't need to check anything
            return;
        }
        
        var mousePosX = (Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize;
        var mousePosY = (Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize;

        _tooltip = _terrainFeatureInformant!.Generate(
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