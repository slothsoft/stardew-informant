using System;
using StardewModdingAPI.Events;

namespace StardewTests.Harness.Events;

public class TestContentEvents : IContentEvents {
    public event EventHandler<AssetRequestedEventArgs>? AssetRequested;
    public event EventHandler<AssetsInvalidatedEventArgs>? AssetsInvalidated;
    public event EventHandler<AssetReadyEventArgs>? AssetReady;
    public event EventHandler<LocaleChangedEventArgs>? LocaleChanged;
}