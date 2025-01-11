using Godot;
using Screenplay.Utils;

namespace Screenplay.Windows;


[Tool]
public partial class MainWindowMenus : HBoxContainer
{
    private MenuButton _fileMenuButton;
    private MenuButton _editMenuButton;
    private MenuButton _debugMenuButton;
    private MenuButton _helpMenuButton;
    
    public override void _Ready()
    {
        base._Ready();
        
        _fileMenuButton = GetNode<MenuButton>("FileMenuButton");
        _editMenuButton = GetNode<MenuButton>("EditMenuButton");
        _debugMenuButton = GetNode<MenuButton>("DebugMenuButton");
        _helpMenuButton = GetNode<MenuButton>("HelpMenuButton");

        SetupFileMenu();
    }

    private void SetupFileMenu()
    {
        var saveShortcut = SUtils.AddShortcut(Key.S, true, false, false);
        _fileMenuButton.GetPopup().SetItemShortcut(4, saveShortcut);
    }
}
