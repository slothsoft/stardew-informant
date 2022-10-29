using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Informant.Api;
using StardewValley.Menus;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Slothsoft.Informant.Implementation;

// ReSharper disable UnusedParameter.Local
internal class ItemDecoratorInformant : IDecoratorInformant<Item> {
    
    private readonly Harmony _harmony;

    private static readonly List<ITooltipDecorator<Item>> DecoratorsList = new();
    private static Rectangle? _lastToolTipCoordinates;

    public ItemDecoratorInformant(IModHelper modHelper) {
        _harmony = new Harmony(InformantMod.Instance!.ModManifest.UniqueID);
        _harmony.Patch(
            original: AccessTools.Method(
                typeof(IClickableMenu),
                nameof(IClickableMenu.drawToolTip)
            ),
            postfix: new HarmonyMethod(typeof(ItemDecoratorInformant), nameof(DrawToolTip))
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
            postfix: new HarmonyMethod(typeof(ItemDecoratorInformant), nameof(RememberToolTipCoordinates))
        );
    }

    private static void DrawToolTip(SpriteBatch b, ref string hoverText, string hoverTitle, Item? hoveredItem,
        bool heldItem = false, int healAmountToDisplay = -1, int currencySymbol = 0, int extraItemToShowIndex = -1,
        int extraItemToShowAmount = -1, CraftingRecipe? craftingIngredients = null,
        int moneyAmountToShowAtBottom = -1) {
        if (_lastToolTipCoordinates == null || hoveredItem == null) {
            return;
        }

        var config = InformantMod.Instance?.Config ?? new InformantConfig();
        var decoration = DecoratorsList
            .Where(g => config.DisplayIds.GetValueOrDefault(g.Id, true))
            .Where(d => d.HasDecoration(hoveredItem))
            .Select(d => d.Decorate(hoveredItem))
            .SingleOrDefault();

        if (decoration == null) {
            return;
        }
        
        const int indent = 16;
        const int scaleFactor = 3;
        var tipCoordinates = _lastToolTipCoordinates.Value;
        var destinationRectangle = new Rectangle(
            tipCoordinates.X + tipCoordinates.Width - decoration.Texture.Width * scaleFactor - indent,
            tipCoordinates.Y + indent,
            decoration.Texture.Width * scaleFactor,
            decoration.Texture.Height * scaleFactor);
        b.Draw(decoration.Texture, destinationRectangle, null, Color.White);
    }

    private static void RememberToolTipCoordinates(SpriteBatch b, Texture2D texture, Rectangle sourceRect, int x, int y,
        int width, int height, Color color, float scale = 1f, bool drawShadow = true, float draw_layer = -1f) {
        _lastToolTipCoordinates = new Rectangle(x, y, width, height);
    }

    public IEnumerable<ITooltipDecorator<Item>> Decorators => DecoratorsList.ToImmutableArray();

    public void Add(ITooltipDecorator<Item> decorator) {
        DecoratorsList.Add(decorator);
    }

    public void Remove(string decoratorId) {
        DecoratorsList.RemoveAll(g => g.Id == decoratorId);
    }
}