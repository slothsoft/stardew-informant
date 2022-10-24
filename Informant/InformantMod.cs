using Slothsoft.Informant.Implementation.TooltipGenerator;

namespace Slothsoft.Informant;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
public class InformantMod : Mod {

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        var informant = new Implementation.Informant(modHelper);
        informant.TerrainFeatureInformant.Add(new FruitTreeTooltipGenerator(modHelper));
    }

}