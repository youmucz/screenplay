using Godot;
using System;
using Godot.Collections;

namespace Screenplay.Blocks;


[Tool]
public partial class BlockScene : MarginContainer
{
    public Guid Uid { get; set; }
    public BlockScene Parent { get; set; }
    
    public BlockResource BlockResource { get; set; } = new ();
    public Array<BlockScene> ChildrenBlocks = new ();
    
    protected const int TabMargin = 16;

    public override void _Ready()
    {
        Uid = Guid.NewGuid();
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
        if (indent)
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
        GrabFocus();
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
