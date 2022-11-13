using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slothsoft.Informant.Api; 

/// <summary>
/// All information needed to display an icon somewhere.
/// </summary>
/// <param name="Texture">the texture to display.</param>
public record Icon(Texture2D Texture) {
    public static Icon ForObject(SObject obj, IPosition? position = null, Vector2? iconSize = null) {
        return ForParentSheetIndex(obj.ParentSheetIndex, obj.bigCraftable.Value, position, iconSize);
    }
    
    public static Icon ForParentSheetIndex(int parentSheetIndex, bool bigCraftable, IPosition? position = null, Vector2? iconSize = null) {
        position ??= IPosition.TopRight;
        iconSize ??= new Vector2(Game1.tileSize, Game1.tileSize);
        
        if (bigCraftable) {
            return new Icon(Game1.bigCraftableSpriteSheet) {
                SourceRectangle = SObject.getSourceRectForBigCraftable(parentSheetIndex),
                Position = position,
                IconSize = iconSize,
            };
        }
        return new Icon(Game1.objectSpriteSheet) {
            SourceRectangle = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, parentSheetIndex, 16, 16),
            Position = position,
            IconSize = iconSize,
        };
    }
    
    public Rectangle? SourceRectangle { get; init; }
    public IPosition? Position { get; init; }
    public Vector2? IconSize { get; init; }

    internal Rectangle NullSafeSourceRectangle => SourceRectangle ?? new Rectangle(0, 0, Texture.Width, Texture.Height);
    private IPosition NullSafePosition => Position ?? IPosition.TopLeft;
    private Vector2 NullSafeIconSize =>  IconSize ?? new Vector2(NullSafeSourceRectangle.Width, NullSafeSourceRectangle.Height);

    internal Rectangle CalculateIconPosition(Rectangle tooltipBounds) {
        return NullSafePosition.CalculateIconPosition(tooltipBounds, NullSafeIconSize);
    }
    
    internal Rectangle CalculateTooltipPosition(Rectangle tooltipBounds) {
        return NullSafePosition.CalculateTooltipPosition(tooltipBounds, NullSafeIconSize);
    }
}