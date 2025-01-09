using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Screenplay.Factory;
using Screenplay.Resources;

namespace Screenplay.Blocks;


[Tool, AttributeUsage(AttributeTargets.Class)]
public class BlockTypeAttribute(Elements blockType) : Attribute
{
    public Elements BlockType { get; set; } = blockType;
}


[Tool]
public partial class BlockScene : MarginContainer
{
    public Guid Uid
    {
        get => Guid.Parse(BlockResource.BlockGuid);
        set => BlockResource.BlockGuid = value.ToString();
    }

    public BlockScene Parent
    {
        get => _parent;
        set { _parent = value; BlockResource.BlockParent = value.Uid.ToString(); }
    }
    
    public PageBlockScene Page;
    public BlockResource BlockResource { get; set; } = new ();
    public Array<BlockScene> ChildrenBlocks = new ();
    
    protected const int TabMargin = 16;
    protected BlockMenu BlockMenu;

    private BlockScene _parent;
    
    public override void _Ready()
    {
        BlockResource.Properties = new();
        BlockResource.Children = new();
        
        BlockMenu = GetNodeOrNull<BlockMenu>("HBoxContainer/BlockMenu");
        
        if (BlockMenu != null)
        {
            BlockMenu.Block = this;
            BlockMenu.DisableMenu();
            MouseEntered += OnMouseEntered;
            MouseExited += OnMouseExited;
        }
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
    /// 添加孩子区块
    /// </summary>
    /// <param name="block"></param>
    public void AddChild(BlockScene block)
    {
        ChildrenBlocks.Add(block);

        if (!BlockResource.Children.ContainsKey(block.BlockResource.BlockGuid))
        {
            BlockResource.Children.TryAdd(block.BlockResource.BlockGuid, block.BlockResource.Serialize());
        }
       
    }
    
    /// <summary>
    /// 删除孩子区块
    /// </summary>
    /// <param name="block"></param>
    public void DekChild(BlockScene block)
    {
        ChildrenBlocks.Remove(block);

        if (BlockResource.Children.ContainsKey(block.BlockResource.BlockGuid))
        {
            BlockResource.Children.Remove(block.BlockResource.BlockGuid);
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
    
    /// <summary>
    /// 序列化meta数据,用来存储到resource本地文件上
    /// </summary>
    /// <returns></returns>
    public virtual Dictionary Serialize()
    {
        BlockResource.Children.Clear();
 
        foreach (var block in ChildrenBlocks)
        {
            BlockResource.Children.Add(block.BlockResource.BlockGuid, block.Serialize());
        }
        
        var data = BlockResource.Serialize();

        return data;
    }

    /// <summary>
    /// 反序列化数据
    /// </summary>
    /// <param name="data"></param>
    public virtual void Deserialize(Dictionary data)
    { 
        if (data == null) return;
        
        BlockResource.Deserialize(data);

        foreach (var kv in BlockResource.Children)
        {
            if (kv.Value.TryGetValue("BlockType", out var blockType))
            {
                var element = (Elements)Enum.Parse(typeof(Elements), blockType.ToString());

                BlockScene block = null;
                if (this is PageBlockScene page)
                {
                    block = page.AddBlock(element, this, kv.Value);
                }
                else
                {
                    block = Page.AddBlock(element, this, kv.Value);
                }

                block?.Deserialize(kv.Value);
            }
        }
    }
}
