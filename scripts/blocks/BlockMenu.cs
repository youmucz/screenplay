using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace Screenplay.Blocks;

[Tool]
public partial class BlockMenu : TextureButton
{
    public BlockScene Block;
    
    private PopupMenu _popupMenu;
    private PopupMenu _elementMenu;
    [Export] private Dictionary<string, Texture2D> _item2Texture;
    
    public override void _Ready()
    {
        _popupMenu = GetNode<PopupMenu>("PopupMenu");
        _elementMenu = GetNode<PopupMenu>("PopupMenu/ElementMenu");
        
        Pressed += OnPressed;

        SetupMenu();
    }

    private void SetupMenu()
    {
        _popupMenu.IndexPressed += PopupMenuOnIndexPressed;
        _elementMenu.IndexPressed += ElementMenuOnIndexPressed;
        _popupMenu.CloseRequested += () => DisableMenu(false);
        _elementMenu.CloseRequested += () => DisableMenu(false);
        
        _popupMenu.AddSubmenuNodeItem("Turn into", _elementMenu);
        _popupMenu.SetItemIcon(5, _item2Texture["Turn into"]);
    }

    private void ElementMenuOnIndexPressed(long index)
    {
        Block.PageBlockScene.TurnInto((int)index + 1, Block);
    }

    private void PopupMenuOnIndexPressed(long index)
    {
        
    }

    private void OnPressed()
    {
        var pos = GlobalPosition + GetWindow().Position;
        
        _popupMenu.Popup(new Rect2I(
            (int)pos.X - _popupMenu.Size.X, (int)pos.Y - _popupMenu.Size.Y / 2, 
            _popupMenu.Size.X, _popupMenu.Size.Y
            ));
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
            case true when !_popupMenu.IsVisible():
                SetTransparency(0.0f);
                break;
            case false:
                SetTransparency(0.0f);
                break;
        }
    }
}
