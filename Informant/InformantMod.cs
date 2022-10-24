using Slothsoft.Informant.Api;
using Slothsoft.Informant.Implementation.TooltipGenerator;

namespace Slothsoft.Informant;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
public class InformantMod : Mod {

    private IInformant? _informant;
    
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        _informant = new Implementation.Informant(modHelper);
        
        _informant.TerrainFeatureInformant.Add(new CropTooltipGenerator(modHelper));
        _informant.TerrainFeatureInformant.Add(new FruitTreeTooltipGenerator(modHelper));
        _informant.TerrainFeatureInformant.Add(new TreeTooltipGenerator(modHelper));
        
        _informant.ObjectInformant.Add(new MachineTooltipGenerator(modHelper));
    }

    public override IInformant? GetApi() {
        return _informant;
    }
}