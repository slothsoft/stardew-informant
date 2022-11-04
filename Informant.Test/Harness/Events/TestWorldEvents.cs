using System;
using StardewModdingAPI.Events;

namespace StardewTests.Harness.Events;

public class TestWorldEvents : IWorldEvents {
    public event EventHandler<LocationListChangedEventArgs>? LocationListChanged;
    public event EventHandler<BuildingListChangedEventArgs>? BuildingListChanged;
    public event EventHandler<DebrisListChangedEventArgs>? DebrisListChanged;
    public event EventHandler<LargeTerrainFeatureListChangedEventArgs>? LargeTerrainFeatureListChanged;
    public event EventHandler<NpcListChangedEventArgs>? NpcListChanged;
    public event EventHandler<ObjectListChangedEventArgs>? ObjectListChanged;
    public event EventHandler<ChestInventoryChangedEventArgs>? ChestInventoryChanged;
    public event EventHandler<TerrainFeatureListChangedEventArgs>? TerrainFeatureListChanged;
    public event EventHandler<FurnitureListChangedEventArgs>? FurnitureListChanged;
}