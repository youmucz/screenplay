using Godot;
using System;

namespace Screenplay.Blocks;

[Tool]
public partial class BlockMenu : TextureButton
{
    private LineEdit _searchEdit;
    private VBoxContainer _vBoxContainer;
    
    public override void _Ready()
    {
        _searchEdit = GetNode<LineEdit>("VBoxContainer/Searcher");
        _vBoxContainer = GetNode<VBoxContainer>("VBoxContainer");
        
        Pressed += OnPressed;
    }
    
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            if (_vBoxContainer.Visible && !_vBoxContainer.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
            {
                DisableMenu(false);
            }
        }
    }

    private void OnPressed()
    {
        _vBoxContainer.SetVisible(true);
        _searchEdit.GrabFocus();
    }

    private void SetTransparency(float value)
    {
        Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, value);
    }

    public void EnableMenu()
    {
        SetTransparency(1.0f);
    }

    public void DisableMenu(bool mouse=true)
    {
        switch (mouse)
        {
            case true when !_vBoxContainer.Visible:
                SetTransparency(0.0f);
                break;
            case false:
                SetTransparency(0.0f);
                _vBoxContainer.SetVisible(false);
                break;
        }
    }
}
