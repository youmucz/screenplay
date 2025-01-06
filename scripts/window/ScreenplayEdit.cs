using Godot;
using Godot.Collections;
using Screenplay.Blocks;
using Screenplay.Factory;
using Screenplay.Resources;

namespace Screenplay.Windows;


[Tool]
public partial class ScreenplayEdit : ScrollContainer
{
    public MainWindow MainWindow;
    public ScreenplayResource MScreenplayResource;
    
    private PackedScene _pageScene;
    private VBoxContainer _pageContainer;

    public override void _Ready()
    {
        _pageContainer = GetNode<VBoxContainer>("PageContainer");
        _pageScene = GD.Load<PackedScene>("res://addons/screenplay/scenes/blocks/page_block_scene.tscn");
    }

    public PageBlockScene AddPage(Dictionary data = null)
    {
        var page = _pageScene.Instantiate<PageBlockScene>();
        page.BlockResource = BlockFactory.Instance.AddBlockResource(Elements.Page.ToString(), data);
        
        _pageContainer.AddChild(page);
        
        page.SEdit = this;
        page.SetPageNumber((_pageContainer.GetChildCount()) + ".");
        
        return page;
    }

    public void DelPage(PageBlockScene pageBlockScene)
    {
        if (_pageContainer.GetChildCount() <= 1) return;
        
        var nextPage = _pageContainer.GetChildOrNull<PageBlockScene>(pageBlockScene.GetIndex() - 1);
        nextPage.FocusBlock();
        
        _pageContainer.RemoveChild(pageBlockScene);
    }
    
    /// <summary>
    /// Load deserialized <see cref="ScreenplayResource" /> from local tres file, and rebuild graph.
    /// </summary>
    /// <param name="resource"></param>
    public void LoadData(ScreenplayResource resource)
    {
        MScreenplayResource = resource;

        Name = resource.Filename?.Split(".")[0];

        foreach (var data in MScreenplayResource.PageData)
        {
            AddPage(data);
        }
    }
	
    /// <summary>
    /// Return serialized node-graph data, include graph global param.
    /// </summary>
    /// <returns><see cref="ScreenplayResource" /></returns>
    public ScreenplayResource DumpsData()
    {
        Array<Dictionary> data = [];

        foreach (var child in _pageContainer.GetChildren())
        {
            if (child is PageBlockScene page)
            {
                data.Add(page.Serialize());
            }
        }
        
        MScreenplayResource.PageData = data;
        
        return MScreenplayResource;
    }
}
