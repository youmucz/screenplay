using Godot;
using System;

[Tool]
public partial class STextEdit : TextEdit
{
    // public override void _Input(InputEvent @event)
    // {
    //     if (@event is InputEventKey keyEvent)
    //     {
    //         //  屏蔽textedit的回车键输入
    //         if (keyEvent.Keycode == Key.Enter && HasFocus())
    //         {
    //             // 将事件标记为已处理，阻止进一步传播
    //             GetViewport().SetInputAsHandled();
    //             return;
    //         }
    //     }
    //     
    //     base._Input(@event);
    // }
}
