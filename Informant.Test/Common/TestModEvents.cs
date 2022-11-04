using System;
using StardewTests.Common.Events;
using StardewModdingAPI.Events;

namespace StardewTests.Common;

public class TestModEvents : IModEvents {
    public IContentEvents Content { get; } = new TestContentEvents();
    public IDisplayEvents Display { get; } = new TestDisplayEvents();
    public IGameLoopEvents GameLoop { get; } = new TestGameLoopEvents();
    public IInputEvents Input { get; } = new TestInputEvents();
    public IMultiplayerEvents Multiplayer { get; } = new TestMultiplayerEvents();
    public IPlayerEvents Player { get; } = new TestPlayerEvents();
    public IWorldEvents World { get; } = new TestWorldEvents();
    public ISpecializedEvents Specialized { get; } = new TestSpecializedEvents();
}