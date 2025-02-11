using System.Collections.Generic;
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
    
    /// <summary>
    /// Update new text to resource.
    /// </summary>
    private void TextEditOnTextChanged()
    {
        if (BlockResource.Properties.TryGetValue("text", out var text))
        {
            BlockResource.Properties["text"] = _textEdit.Text;
        }
        else
        {
            BlockResource.Properties.TryAdd("text", _textEdit.Text);
        }
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
        _textEdit.SetCaretColumn(_textEdit.Text.Length);
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
    
    /// <summary>
    /// This block will be destroyed, do something.
    /// </summary>
    /// <param name="toGrabBlock"> Will to grab after this block be destroyed. </param>
    public override void Destroy(BlockScene toGrabBlock)
    {
        if (toGrabBlock is TextBlockScene textBlockScene)
        {
            textBlockScene.SetText(GetRelicText());
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
