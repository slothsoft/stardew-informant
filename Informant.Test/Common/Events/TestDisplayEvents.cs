using System;
using StardewModdingAPI.Events;

namespace StardewTests.Common.Events;

public class TestDisplayEvents : IDisplayEvents {
    public event EventHandler<MenuChangedEventArgs>? MenuChanged;
    public event EventHandler<RenderingEventArgs>? Rendering;
    public event EventHandler<RenderedEventArgs>? Rendered;
    public event EventHandler<RenderingWorldEventArgs>? RenderingWorld;
    public event EventHandler<RenderedWorldEventArgs>? RenderedWorld;
    public event EventHandler<RenderingActiveMenuEventArgs>? RenderingActiveMenu;
    public event EventHandler<RenderedActiveMenuEventArgs>? RenderedActiveMenu;
    public event EventHandler<RenderingHudEventArgs>? RenderingHud;
    public event EventHandler<RenderedHudEventArgs>? RenderedHud;
    public event EventHandler<WindowResizedEventArgs>? WindowResized;
}