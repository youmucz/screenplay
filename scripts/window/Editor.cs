using System;
using Godot;
using Godot.Collections;
using Screenplay.Utils;
using Screenplay.Blocks;
using Screenplay.Factory;
using Screenplay.Resources;

namespace Screenplay.Windows;


[Tool]
public partial class Editor : ScrollContainer
{
    public MainWindow MainWindow;
    public EditorResource MEditorResource;
    
    [Export] public Dictionary<Elements, PackedScene> BlockScenes;
    
    private PageBlockScene _page;
    private Button _addTemplateButton;
    private VBoxContainer _pageContainer;
    private VBoxContainer _emptyContainer;

    public override void _Ready()
    {
        _pageContainer = GetNode<VBoxContainer>("PageContainer");
        _addTemplateButton = GetNode<Button>("PageContainer/EmptyContainer/AddTemplateButton");
        _emptyContainer = GetNode<VBoxContainer>("PageContainer/EmptyContainer");
        
        _addTemplateButton.Pressed += AddTemplateButtonOnPressed;
    }

    public override void _GuiInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        switch (@event)
        {
            // 鼠标双击创建新block
            case InputEventMouseButton mouseEvent:
            {
                if (mouseEvent.DoubleClick && mouseEvent.ButtonIndex == MouseButton.Left)
                {
                    if (_emptyContainer.Visible)
                    {
                        _page.AddBlock();
                        ShowTemplateContainer();
                    }
                }

                break;
            }
        }
    }

    public PageBlockScene AddPage(Elements blockType = Elements.Document, Dictionary data = null)
    {
        _page = (PageBlockScene)BlockFactory.Instance.AddBlockScene(blockType, BlockScenes, data);
        _pageContainer.AddChild(_page);
        
        _page.SEditor = this;
        _page.Deserialize(data);
        // _page.AddBlock();
        
        return _page;
    }

    public void DelPage(PageBlockScene pageBlockScene)
    {
        
    }
    
    /// <summary>
    /// Load deserialized <see cref="EditorResource" /> from local tres file, and rebuild graph.
    /// </summary>
    /// <param name="resource"></param>
    public void LoadData(EditorResource resource)
    {
        MEditorResource = resource;

        Name = resource.Filename?.Split(".")[0];

        if (MEditorResource.Data.TryGetValue("BlockType", out var blockType))
        {
            AddPage((Elements)Enum.Parse(typeof(Elements), blockType.ToString()), MEditorResource.Data);
        }
        
        ShowTemplateContainer();
    }
	
    /// <summary>
    /// Return serialized node-graph data, include graph global param.
    /// </summary>
    /// <returns><see cref="EditorResource" /></returns>
    public EditorResource DumpsData()
    {
        MEditorResource.Data = _page.Serialize();
        
        return MEditorResource;
    }
       
    /// <summary>
    /// 创建模板文件
    /// </summary>
    private void AddTemplateButtonOnPressed()
    {
        ScreenplayPlugin.GetMainWindow().PopupTemplateWindow();
    }
    
    /// <summary>
    /// 空白页面提示面板显隐逻辑
    /// </summary>
    private void ShowTemplateContainer()
    {
        if (_page.HasEmpty)
        {
            _page.SetVisible(false);
            _emptyContainer.SetVisible(true);
        }
        else
        {
            _page.SetVisible(true);
            _emptyContainer.SetVisible(false);
        }
    }
}
