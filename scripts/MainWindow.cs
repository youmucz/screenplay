using Godot;
using Godot.Collections;
using Screenplay.Blocks;


[Tool]
public partial class MainWindow : Control
{
    [Export] private Dictionary<StringName, PackedScene> _blocksScenes;
    
    private VBoxContainer _pageContainer;
    private Array<PageBlockScene> _pages;
    
    public override void _Ready()
    {
        _pageContainer = GetNode<VBoxContainer>("ScrollContainer/PageContainer");
        CreatePages();
    }

    private void CreatePages()
    {
        var pageBlock = AddBlock<PageBlockScene>("Page");
        
        _pageContainer.AddChild(pageBlock);
    }

    public T AddBlock<T>(StringName type) where T : BlockScene
    {
        var scene = _blocksScenes[type];
        var block = scene.Instantiate<T>();
        
        return block;
    }
}
