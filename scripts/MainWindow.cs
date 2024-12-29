using Godot;
using Godot.Collections;
using Screenplay.Blocks;


[Tool]
public partial class MainWindow : Control
{
    [Export] private PackedScene _pageScene;
    
    private VBoxContainer _pageContainer;
    
    public override void _Ready()
    {
        _pageContainer = GetNode<VBoxContainer>("ScrollContainer/PageContainer");
        
        AddPage();
    }

    public Page AddPage()
    {
        var page = _pageScene.Instantiate<Page>();
        page.MainWindow = this;
        _pageContainer.AddChild(page);
        
        return page;
    }

    public void DelPage(Page page)
    {
        if (_pageContainer.GetChildCount() <= 1) return;
        
        var nextPage = _pageContainer.GetChildOrNull<Page>(page.GetIndex() - 1);
        nextPage.FocusBlock();
        
        _pageContainer.RemoveChild(page);
    }
}
