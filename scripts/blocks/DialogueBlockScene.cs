using Godot;

namespace Screenplay.Blocks;

[Tool]
public partial class DialogueBlockScene : BlockScene
{
    protected override BlockResource BlockResource { get; set; } = new DialogueBlockResource();
    
    private TextEdit _textEdit;
    
    public override void _Ready()
    {
        _textEdit = GetNode<TextEdit>("TextEdit");
        _textEdit.FocusEntered += TextEditOnFocusEntered;
        _textEdit.FocusExited += TextEditOnFocusExited;
        _textEdit.TextChanged += TextEditOnTextChanged;
    }

    private void TextEditOnTextChanged()
    {
        
    }

    private void TextEditOnFocusEntered()
    {
        _textEdit.PlaceholderText = PlaceholderText.Text;
        // EmitSignalFocusEntered();
    }
    
    private void TextEditOnFocusExited()
    {
        _textEdit.PlaceholderText = "";
        // EmitSignalFocusExited();
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
    
    public new bool HasFocus()
    {
        var res = base.HasFocus();
        return _textEdit.HasFocus() || res;
    }

    public override bool CanDestroy()
    {
        return _textEdit.GetCaretColumn() == 0;
    }
    
    /// <summary>
    /// 遗留的文本
    /// </summary>
    /// <returns></returns>
    public string GetRelicText()
    {
        return _textEdit.GetText();
    }

    public void SetText(string text)
    {
        _textEdit.Text += text;
    }

    public new void GrabFocus()
    {
       base.GrabFocus();
       _textEdit.GrabFocus();
    }
}
