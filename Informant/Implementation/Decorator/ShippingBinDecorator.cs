using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Informant.Api;

namespace Slothsoft.Informant.Implementation.Decorator;

internal class ShippingBinDecorator : IDecorator<Item> {

    private static Texture2D? _shippingBin;
    
    private readonly IModHelper _modHelper;
    
    public ShippingBinDecorator(IModHelper modHelper) {
        _modHelper = modHelper;
        _shippingBin ??= modHelper.ModContent.Load<Texture2D>("assets/shipping_bin.png");
    }

    public string Id => "shipping";
    public string DisplayName => _modHelper.Translation.Get("ShippingBinDecorator");
    public string Description => _modHelper.Translation.Get("ShippingBinDecorator.Description");

    public bool HasDecoration(Item input) {
        if (_shippingBin != null && input is SObject obj && !obj.bigCraftable.Value) {
            return obj.countsForShippedCollection() &&
                (!Game1.player.basicShipped.ContainsKey(obj.ParentSheetIndex) || Game1.player.basicShipped[obj.ParentSheetIndex] == 0);
        }
        return false;
    }

    public Decoration Decorate(Item input) {
        return new Decoration(_shippingBin!);
    }
}