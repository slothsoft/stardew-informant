using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Informant.Api;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Informant.Implementation; 

/// <summary>
/// Generating tooltips for all kinds of objects is actually pretty similar, that's why this
/// is only one instance for all <see cref="ITooltipGeneratorManager{TInput}"/> implementations.
/// </summary>

internal class TooltipGeneratorManager : ITooltipGeneratorManager<TerrainFeature>, ITooltipGeneratorManager<SObject> {

    internal static Rectangle TooltipSourceRect = new(0, 256, 60, 60);
    
    private readonly IModHelper _modHelper;
    private BaseTooltipGeneratorManager<TerrainFeature>? _terrainFeatureManager;
    private BaseTooltipGeneratorManager<SObject>? _objectInformant;
    
    private readonly PerScreen<IEnumerable<Tooltip>?> _tooltips = new();

    public TooltipGeneratorManager(IModHelper modHelper) {
        _modHelper = modHelper;
        
        modHelper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        modHelper.Events.Display.Rendered += OnRendered;
    }

    IEnumerable<IDisplayable> ITooltipGeneratorManager<SObject>.Generators => 
        _objectInformant?.Generators.ToImmutableArray() ?? Enumerable.Empty<IDisplayable>();

    IEnumerable<IDisplayable> ITooltipGeneratorManager<TerrainFeature>.Generators => 
        _terrainFeatureManager?.Generators.ToImmutableArray() ?? Enumerable.Empty<IDisplayable>();

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e2) {
        if (!Context.IsPlayerFree) {
            _tooltips.Value = Array.Empty<Tooltip>();
            return;
        }

        if (!WasTriggered()) {
            _tooltips.Value = Array.Empty<Tooltip>();
            return;
        }
        
        _tooltips.Value = GenerateObjectTooltips().Concat(GenerateTerrainFeatureTooltips());
    }

    private bool WasTriggered() {
        var config = InformantMod.Instance?.Config;
        if (config == null) {
            return false; // something went wrong here!
        }

        return config.TooltipTrigger switch {
            TooltipTrigger.Hover => true, // hover is ALWAYS triggered
            TooltipTrigger.ButtonHeld => _modHelper.Input.GetState(config.TooltipTriggerButton) == SButtonState.Held,
            _ => false // we don't know this trigger
        };
    }

    private IEnumerable<Tooltip> GenerateTerrainFeatureTooltips() {
        return GenerateTooltips(_terrainFeatureManager, (mousePosX, mousePosY) => 
            Game1.currentLocation.terrainFeatures.Values
            .Where(t => mousePosX == (int) t.currentTileLocation.X && mousePosY == (int) t.currentTileLocation.Y)
            .ToArray());
    }
    
    private static IEnumerable<Tooltip> GenerateTooltips<TTile>(BaseTooltipGeneratorManager<TTile>? manager, Func<int, int, TTile[]> getTilesForPosition) {
        if (manager == null) {
            // if there is no generator in that, we don't need to do anything further
            return Enumerable.Empty<Tooltip>();
        }
        
        var mouseX = Game1.getOldMouseX();
        var mouseY = Game1.getOldMouseY();
        
        var toolbar = Game1.onScreenMenus.FirstOrDefault(m => m is Toolbar);
        if (toolbar != null && toolbar.isWithinBounds(mouseX, mouseY)) {
            // mouse is over the toolbar, so we won't generate tooltips for the map
            return Enumerable.Empty<Tooltip>();
        }
        
        var mousePosX = (mouseX + Game1.viewport.X) / Game1.tileSize;
        var mousePosY = (mouseY + Game1.viewport.Y) / Game1.tileSize;
        
        return manager.Generate(getTilesForPosition.Invoke(mousePosX, mousePosY));
    }
    
    private IEnumerable<Tooltip> GenerateObjectTooltips() {
        return GenerateTooltips(_objectInformant, (mousePosX, mousePosY) => 
            Game1.currentLocation.netObjects.Values
                .Where(t => mousePosX == (int) t.TileLocation.X && mousePosY == (int) t.TileLocation.Y)
                .ToArray());
    }

    private void OnRendered(object? sender, RenderedEventArgs e) {
        if (Context.IsPlayerFree && _tooltips.Value != null) {
            var tooltipsArray = _tooltips.Value.ToArray();

            if (tooltipsArray.Length == 0) {
                return;
            }
            const int borderSize = 3 * Game1.pixelZoom;
            var font = Game1.smallFont;
            var approximateBounds = CalculateApproximateBounds(tooltipsArray, font);
            var startY = approximateBounds.Y;
            
            foreach (var tooltip in tooltipsArray) {
                var height = Math.Max(60, (int) font.MeasureString(tooltip.Text).Y + Game1.tileSize / 2);
                DrawSimpleTooltip(Game1.spriteBatch, tooltip, font, approximateBounds with {
                    Y = startY,
                    Height = height,
                });
                startY += height - borderSize;
            }
        }
    }

    private static Rectangle CalculateApproximateBounds(IEnumerable<Tooltip> tooltips, SpriteFont font) {
        var textSize = font.MeasureString(string.Join("\n\n", tooltips.Select(t => t.Text)));
        var height = Math.Max(60, (int) textSize.Y + Game1.tileSize / 2);
        var x = Game1.getOldMouseX() + Game1.tileSize / 2;
        var y = Game1.getOldMouseY() + Game1.tileSize / 2;

        if (x + textSize.X > Game1.viewport.Width) {
            x = (int) (Game1.viewport.Width - textSize.X);
            y += Game1.tileSize / 4;
        }

        if (y + height > Game1.viewport.Height) {
            x += Game1.tileSize / 4;
            y = Game1.viewport.Height - height;
        }
        return new Rectangle(x, y, (int) textSize.X + Game1.tileSize / 2, (int) textSize.Y);
    }

    private static void DrawSimpleTooltip(SpriteBatch b, Tooltip tooltip, SpriteFont font, Rectangle textureBoxBounds) {
        IClickableMenu.drawTextureBox(b, Game1.menuTexture, TooltipSourceRect, textureBoxBounds.X, textureBoxBounds.Y, 
            textureBoxBounds.Width, textureBoxBounds.Height, Color.White);

        var position = new Vector2(textureBoxBounds.X + Game1.tileSize / 4, textureBoxBounds.Y + Game1.tileSize / 4 + 4);
        b.DrawString(font, tooltip.Text, position + new Vector2(2f, 2f), Game1.textShadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        b.DrawString(font, tooltip.Text, position + new Vector2(0f, 2f), Game1.textShadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        b.DrawString(font, tooltip.Text, position + new Vector2(2f, 0f), Game1.textShadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        b.DrawString(font, tooltip.Text, position, Game1.textColor * 0.9f, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
    }
    
    public void Add(ITooltipGenerator<TerrainFeature> generator) {
        _terrainFeatureManager ??= new BaseTooltipGeneratorManager<TerrainFeature>();
        _terrainFeatureManager.Add(generator);
    }

    public void Remove(string generatorId) {
        _terrainFeatureManager?.Remove(generatorId);
        _objectInformant?.Remove(generatorId);
    }

    public void Add(ITooltipGenerator<SObject> generator) {
        _objectInformant ??= new BaseTooltipGeneratorManager<SObject>();
        _objectInformant.Add(generator);
    }
}