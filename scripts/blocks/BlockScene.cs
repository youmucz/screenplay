using Godot;
using System;
using Godot.Collections;
using Screenplay.Resources;

namespace Screenplay.Blocks;


[Tool]
public partial class BlockScene : MarginContainer
{
    public Guid Uid;
    public PageBlockScene PageBlockScene;
    public BlockScene Parent { get; set; }
    
    public virtual BlockResource BlockResource { get; set; } = new ();
    public Array<BlockScene> ChildrenBlocks = new ();
    
    protected const int TabMargin = 16;
    protected BlockMenu BlockMenu;

    public override void _Ready()
    {
        Uid = Guid.NewGuid();
        
        BlockMenu = GetNode<BlockMenu>("HBoxContainer/BlockMenu");
        BlockMenu.Block = this;
        BlockMenu.DisableMenu();
        
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    private void OnMouseEntered()
    {
        BlockMenu.EnableMenu();
    }
    
    private void OnMouseExited()
    {
        BlockMenu.DisableMenu();
    }

    /// <summary>
    /// 重置父子关系树
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="indent">是否进行缩进</param>
    public void IndentParent(BlockScene parent, bool indent = true)
    {
        if (parent == Parent || !IsInstanceValid(parent)) return;
        // 1.清理旧父节点关系
        Parent?.ChildrenBlocks.Remove(this);
        // 2.替换子节点的父节点
        foreach (var node in ChildrenBlocks)
        {
            node.Parent = parent;
            if (!parent.ChildrenBlocks.Contains(node)) parent.ChildrenBlocks.Add(node);
        }
        ChildrenBlocks.Clear();
        // 3.重置父节点
        Parent = parent;
        // 4.创建关系树
        if (!parent.ChildrenBlocks.Contains(this)) parent.ChildrenBlocks.Add(this);
        // 5.进行缩进
        SetIndent("margin_left", indent ? parent.GetThemeConstant("margin_left") : 0);
    }
    
    /// <summary>
    /// 顶格块,只改变margin
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="indent"></param>
    public void UnindentParent(BlockScene parent, bool indent = true)
    {
        SetIndent("margin_left", indent ? parent.GetThemeConstant("margin_left") : 0);
        
        foreach (var node in ChildrenBlocks)
        {
            node.UnindentParent(this);
        }
    }
    
    /// <summary>
    /// 删除block后要做的事
    /// </summary>
    /// <param name="block"></param>
    public virtual void Destroy(BlockScene block)
    {
        
    }
    
    public virtual bool CanDestroy() { return true; }

    public virtual bool CanIndent()
    {
        return true;
    }

    public virtual void SetFocus()
    {
        
    }

    public virtual bool GetFocus()
    {
        return HasFocus();
    }
    
    public virtual void SetIndent(StringName name, int value)
    {
        AddThemeConstantOverride(name, value + TabMargin);
    }
}
