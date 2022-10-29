using Slothsoft.Informant.Api;
using Slothsoft.Informant.Implementation.TooltipDecorator;
using Slothsoft.Informant.Implementation.TooltipGenerator;
using Slothsoft.Informant.ThirdParty;
using StardewModdingAPI.Events;

namespace Slothsoft.Informant;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
public class InformantMod : Mod {
    internal static InformantMod? Instance;

    private IInformant? _api;
    internal InformantConfig Config = null!;
    
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        Instance = this;
        Config = Helper.ReadConfig<InformantConfig>();
        _api = new Implementation.Informant(modHelper);
        
        _api.TerrainFeatureTooltips.Add(new CropTooltipGenerator(modHelper));
        _api.TerrainFeatureTooltips.Add(new FruitTreeTooltipGenerator(modHelper));
        _api.TerrainFeatureTooltips.Add(new TreeTooltipGenerator(modHelper));
        
        _api.ObjectTooltips.Add(new MachineTooltipGenerator(modHelper));
        
        _api.ItemDecorators.Add(new BundleTooltipDecorator(modHelper));
        
        // has to be done after registering all the tooltips and decorators
        Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
    }
    
    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e) {
        HookToGenericModConfigMenu.Apply(this, _api!);
    }

    public override IInformant? GetApi() {
        return _api;
    }
}