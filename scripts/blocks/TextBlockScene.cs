using Godot;
using System;

[Tool]
public partial class TextBlockScene : BlockScene
{
    private TextEdit _textEdit;
    
    public override void _Ready()
    {
        _textEdit = GetNode<TextEdit>("TextEdit");
        _textEdit.FocusEntered += EmitSignalFocusEntered;
        _textEdit.FocusExited += EmitSignalFocusExited;
    }
    
    // public override void _Input(InputEvent @event)
    // {
    //     if (@event is InputEventKey keyEvent)
    //     {
    //         if (keyEvent.Keycode is Key.Backspace && keyEvent.Pressed)
    //         {
    //             if (_textEdit.Text == "" && _textEdit.HasFocus())
    //             {
    //                 DestroySelf.Invoke(this);
    //             }
    //         }
    //     }
    //     
    //     base._Input(@event);
    // }

    public override bool CanDestroy()
    {
        return _textEdit is { Text: "" };
    }

    public new void GrabFocus()
    {
       base.GrabFocus();
       
       _textEdit.GrabFocus();
    }
}
