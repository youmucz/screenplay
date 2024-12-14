using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;


[Tool]
public partial class BlockScene : MarginContainer
{
    public Guid Uid { get; set; } = Guid.NewGuid();
    
    public BlockScene Parent;
    public Array<BlockScene> ChildrenBlocks = new ();
    
    protected const int TabMargin = 16;
    
    /// <summary>
    /// 重置父子关系树
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(BlockScene parent)
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
