using Godot;
using System;
using Godot.Collections;

namespace Screenplay.Blocks;


[Tool]
public partial class BlockScene : MarginContainer
{
    public Guid Uid { get; set; } = Guid.NewGuid();
    
    public BlockScene Parent;
    protected virtual BlockResource BlockResource { get; set; } = new ();
    public Array<BlockScene> ChildrenBlocks = new ();
    
    protected const int TabMargin = 16;
    
    /// <summary>
    /// 重置父子关系树
    /// </summary>
    /// <param name="parent"></param>
    public void IndentParent(BlockScene parent)
    {
        if (parent == Parent || parent is null) return;
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
        SetIndent("margin_left", parent.GetThemeConstant("margin_left"));
    }
    
    /// <summary>
    /// 顶格块,只改变margin
    /// </summary>
    /// <param name="parent"></param>
    public void UnindentParent(BlockScene parent)
    {
        SetIndent("margin_left", parent.GetThemeConstant("margin_left"));
        
        foreach (var node in ChildrenBlocks)
        {
            node.UnindentParent(this);
        }
    }
    
    public virtual bool CanDestroy() { return true; }

    public virtual bool CanIndent()
    {
        return true;
    }
    
    public virtual void SetIndent(StringName name, int value)
    {
        AddThemeConstantOverride(name, value + TabMargin);
    }
}
