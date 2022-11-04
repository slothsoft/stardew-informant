using System.Collections.Generic;
using StardewModdingAPI.Utilities;

namespace StardewTests.Common; 

public class TestInputHelper : IInputHelper {

    private List<SButton> _downButtons = new(); 
    private List<SButton> _suppressButtons = new(); 
    private TestCursorPosition? _cursorPosition;
    
    public string ModID { get; init; } = "";
    
    public ICursorPosition GetCursorPosition() {
        _cursorPosition ??= new TestCursorPosition();
        return _cursorPosition;
    }

    public void Down(SButton sButton) {
        _downButtons.Add(sButton);
    }
    
    public bool IsDown(SButton button) {
        return _downButtons.Contains(button);
    }

    public void Suppress(SButton button) {
        _suppressButtons.Add(button);
    }

    public bool IsSuppressed(SButton button) {
        return _suppressButtons.Contains(button);
    }

    public void SuppressActiveKeybinds(KeybindList keybindList) {
        foreach (var keybinds in keybindList.Keybinds) {
            _suppressButtons.AddRange(keybinds.Buttons);
        }
    }

    public SButtonState GetState(SButton button) {
        if (IsDown(button))
            return SButtonState.Held;
        if (IsSuppressed(button))
            return SButtonState.None;
        return SButtonState.Released;
    }

}