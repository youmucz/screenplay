using Godot;
using System;

namespace Screenplay.Blocks;

[Tool]
public partial class BlockMenu : TextureButton
{
    private VBoxContainer _vBoxContainer;
    
    public override void _Ready()
    {
        _vBoxContainer = GetNode<VBoxContainer>("VBoxContainer");
            
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        _vBoxContainer.SetVisible(true);
    }

    public void SetTransparency(float value)
    {
        Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, value);
    }
}
