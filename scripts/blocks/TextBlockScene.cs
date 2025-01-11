using Godot;
using Godot.Collections;
using Screenplay.Utils;

namespace Screenplay.Blocks;

[Tool, BlockType(Elements.Text)]
public partial class TextBlockScene : BlockScene
{
    private TextEdit _textEdit;
    
    public override void _Ready()
    {
        base._Ready();
        
        _textEdit = GetNode<TextEdit>("HBoxContainer/TextEdit");
        _textEdit.FocusEntered += TextEditOnFocusEntered;
        _textEdit.FocusExited += TextEditOnFocusExited;
        _textEdit.TextChanged += TextEditOnTextChanged;
    }

    private void TextEditOnTextChanged()
    {
        BlockResource.Properties.Add("text", _textEdit.Text);
    }

    private void TextEditOnFocusEntered()
    {
        _textEdit.PlaceholderText = PlaceholderText.Text;
        EmitSignalFocusEntered();
    }
    
    private void TextEditOnFocusExited()
    {
        _textEdit.PlaceholderText = "";
        EmitSignalFocusExited();
    }
    
    public override void SetFocus()
    {
        base.SetFocus();
        _textEdit.GrabFocus();
    }

    public override bool GetFocus()
    {
        var res = base.GetFocus();
        return _textEdit.HasFocus() || res;
    }

    public override bool CanDestroy()
    {
        return _textEdit.GetCaretColumn() == 0;
    }
    
    public override void Destroy(BlockScene block)
    {
        if (block is TextBlockScene textBlockScene)
        {
            SetText(textBlockScene.GetRelicText());
        }
    }
    
    public void SetText(string text)
    {
        _textEdit.Text += text;
    }
    
    /// <summary>
    /// 遗留的文本
    /// </summary>
    /// <returns></returns>
    public string GetRelicText()
    {
        return _textEdit.GetText();
    }

    public override void Deserialize(Dictionary data)
    {
        base.Deserialize(data);
        
        if (BlockResource.Properties.TryGetValue("text", out var text))
        {
            _textEdit.Text = text;
        }
    }
}
