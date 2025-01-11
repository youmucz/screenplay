using Godot;

namespace Screenplay.Utils;


public class SUtils
{
    /// <summary>
    /// 添加快捷键
    /// </summary>
    /// <param name="keycode"> <see cref="Key"/> </param>
    /// <param name="ctrlPressed"> <see cref="InputEventWithModifiers.CtrlPressed"/> </param>
    /// <param name="shiftPressed"> <see cref="InputEventWithModifiers.ShiftPressed"/> </param>
    /// <param name="altPressed"> <see cref="InputEventWithModifiers.AltPressed"/> </param>
    /// <returns></returns>
    public static Shortcut AddShortcut(Key keycode, bool ctrlPressed, bool shiftPressed, bool altPressed)
    {
        var shortcut = new Shortcut();
        
        var inputEventKey = new InputEventKey()
        {
            CtrlPressed = ctrlPressed, // Ctrl键
            ShiftPressed = shiftPressed, // Shift键
            AltPressed = altPressed, // Alt键
            Keycode = keycode, // 按下key键
        };
        shortcut.Events.Add(inputEventKey);
        
        return shortcut;
    }
}
