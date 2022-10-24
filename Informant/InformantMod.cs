using Slothsoft.Informant.Api;
using Slothsoft.Informant.Implementation.TooltipDecorator;
using Slothsoft.Informant.Implementation.TooltipGenerator;

namespace Slothsoft.Informant;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
public class InformantMod : Mod {
    internal static InformantMod? Instance;

    private IInformant? _informant;
    
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        Instance = this;
        _informant = new Implementation.Informant(modHelper);
        
        _informant.TerrainFeatureTooltips.Add(new CropTooltipGenerator(modHelper));
        _informant.TerrainFeatureTooltips.Add(new FruitTreeTooltipGenerator(modHelper));
        _informant.TerrainFeatureTooltips.Add(new TreeTooltipGenerator(modHelper));
        
        _informant.ObjectTooltips.Add(new MachineTooltipGenerator(modHelper));
        
        _informant.ItemDecorators.Add(new BundleTooltipDecorator(modHelper));
    }

    public override IInformant? GetApi() {
        return _informant;
    }
}