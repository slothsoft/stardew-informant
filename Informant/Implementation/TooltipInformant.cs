using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Informant.Api;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation; 

internal class TooltipInformant : ITooltipInformant<TerrainFeature>, ITooltipInformant<SObject> {

    private readonly IModHelper _modHelper;
    private BaseTooltipInformant<TerrainFeature>? _terrainFeatureInformant;
    private BaseTooltipInformant<SObject>? _objectInformant;
    
    private IEnumerable<Tooltip>? _tooltips;

    public TooltipInformant(IModHelper modHelper) {
        _modHelper = modHelper;
        
        modHelper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        modHelper.Events.Display.Rendered += OnRendered;
    }

    IEnumerable<ITooltipGenerator<SObject>> ITooltipInformant<SObject>.Generators => 
        _objectInformant?.Generators.ToImmutableArray() ?? Enumerable.Empty<ITooltipGenerator<SObject>>();

    IEnumerable<ITooltipGenerator<TerrainFeature>> ITooltipInformant<TerrainFeature>.Generators => 
        _terrainFeatureInformant?.Generators.ToImmutableArray() ?? Enumerable.Empty<ITooltipGenerator<TerrainFeature>>();

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e2) {
        if (!Context.IsPlayerFree) {
            return;
        }
        _tooltips = GenerateObjectTooltips().Concat(GenerateTerrainFeatureTooltips());
    }

    private IEnumerable<Tooltip> GenerateTerrainFeatureTooltips() {
        if (_terrainFeatureInformant == null) {
            // if there is no generator in that, we don't need to do anything further
            return Enumerable.Empty<Tooltip>();
        }
        
        var mousePosX = (Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize;
        var mousePosY = (Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize;
        
        return _terrainFeatureInformant.Generate(
            Game1.currentLocation.terrainFeatures.Values
                .Where(t => mousePosX == (int) t.currentTileLocation.X && mousePosY == (int) t.currentTileLocation.Y)
                .ToArray()
        );
    }
    
    private IEnumerable<Tooltip> GenerateObjectTooltips() {
        if (_objectInformant == null) {
            // if there is no generator in that, we don't need to do anything further
            return Enumerable.Empty<Tooltip>();
        }
        
        var mousePosX = (Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize;
        var mousePosY = (Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize;

        return _objectInformant.Generate(
            Game1.currentLocation.netObjects.Values
                .Where(t => mousePosX == (int) t.TileLocation.X && mousePosY == (int) t.TileLocation.Y)
                .ToArray()
        );
    }

    private void OnRendered(object? sender, RenderedEventArgs e) {
        if (Context.IsPlayerFree && _tooltips != null) {
            var tooltipsArray = _tooltips.ToArray();

            if (tooltipsArray.Length == 0) {
                return;
            }
            const int borderSize = 3 * Game1.pixelZoom;
            int? startY = null;
            var font = Game1.smallFont;
            var tooltipsWidth = (int) tooltipsArray.Select(t => font.MeasureString(t.Text).X).Max() + Game1.tileSize / 2;
            
            foreach (var tooltip in tooltipsArray) {
                var bounds = DrawSimpleTooltip(Game1.spriteBatch, tooltip, font, startY, tooltipsWidth);
                startY = bounds.Y + bounds.Height - borderSize;
            }
        }
    }

    private static Rectangle DrawSimpleTooltip(SpriteBatch b, Tooltip tooltip, SpriteFont font, int? startY, int width) {
        var textSize = font.MeasureString(tooltip.Text);
        var height = Math.Max(60, (int) textSize.Y + Game1.tileSize / 2);
        var x = Game1.getOldMouseX() + Game1.tileSize / 2;
        var y = startY ?? Game1.getOldMouseY() + Game1.tileSize / 2;

        if (x + width > Game1.viewport.Width) {
            x = Game1.viewport.Width - width;
            y += Game1.tileSize / 4;
        }

        if (y + height > Game1.viewport.Height) {
            x += Game1.tileSize / 4;
            y = Game1.viewport.Height - height;
        }

        var textureBoxBounds = new Rectangle(x, y, width, height);
        IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), textureBoxBounds.X, textureBoxBounds.Y, 
            textureBoxBounds.Width, textureBoxBounds.Height, Color.White);

        var position = new Vector2(x + Game1.tileSize / 4, y + Game1.tileSize / 4 + 4);
        b.DrawString(font, tooltip.Text, position + new Vector2(2f, 2f), Game1.textShadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        b.DrawString(font, tooltip.Text, position + new Vector2(0f, 2f), Game1.textShadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        b.DrawString(font, tooltip.Text, position + new Vector2(2f, 0f), Game1.textShadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        b.DrawString(font, tooltip.Text, position, Game1.textColor * 0.9f, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

        return textureBoxBounds;
    }
    
    public void Add(ITooltipGenerator<TerrainFeature> generator) {
        _terrainFeatureInformant ??= new BaseTooltipInformant<TerrainFeature>();
        _terrainFeatureInformant.Add(generator);
    }

    public void Remove(string generatorId) {
        _terrainFeatureInformant?.Remove(generatorId);
        _objectInformant?.Remove(generatorId);
    }

    public void Add(ITooltipGenerator<SObject> generator) {
        _objectInformant ??= new BaseTooltipInformant<SObject>();
        _objectInformant.Add(generator);
    }
}