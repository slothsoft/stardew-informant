using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Informant.Api;


namespace Slothsoft.Informant.Implementation.Decorator;

public class AquariumDecorator : IDecorator<Item>
{
    private static Texture2D? _fish;
    
    private readonly IModHelper _modHelper;
    
    public AquariumDecorator(IModHelper modHelper) {
        _modHelper = modHelper;
        _fish ??= modHelper.ModContent.Load<Texture2D>("assets/aquarium.png");
    }

    public string Id => "aquarium";
    public string DisplayName => _modHelper.Translation.Get("AquariumTooltipDecorator");
    public string Description => _modHelper.Translation.Get("AquariumTooltipDecorator.Description");
    
    public bool HasDecoration(Item input) {
        if (_fish != null && input.Category == SObject.FishCategory) {
            // Aquarium Data
            // ==========
            // See https://www.nexusmods.com/stardewvalley/mods/6372?tab=description
            //      (Section "Extra"->"Modder's reference")
            // "Mail flags are added to the Host player for each fish donated,
            //      with any spaces removed from the Fish name, for example:
            //      AquariumDonated:RainbowTrout."
            string mailFlag = $"AquariumDonated:{input.Name.Replace(" ", string.Empty)}";
            return !Game1.MasterPlayer.mailReceived.Contains(mailFlag);
        }
        return false;
    }

    public Decoration Decorate(Item input) {
        return new Decoration(_fish!);
    }
}