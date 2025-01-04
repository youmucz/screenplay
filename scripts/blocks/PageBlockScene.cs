using Godot;
using System;
using Godot.Collections;
using Screenplay.Factory;
using Screenplay.Windows;
using Screenplay.Resources;

namespace Screenplay.Blocks;


[Tool, BlockType(Elements.Page)]
public partial class PageBlockScene : BlockScene
{
    public ScreenplayEdit SEdit;
    
    /// <summary> current focus block. </summary>
    public BlockScene GrabBlock
    {
        set { _grabBlock = value; _grabBlock?.SetFocus(); }
        get => _grabBlock;
    }
    /// <summary> current focus block. </summary>
    private BlockScene _grabBlock;
    
    private const int MaxBlocks = 20;

    private Label _pageNumber;
    private VBoxContainer _vboxContainer;
    private VBoxContainer _blockContainer;
    private VBoxContainer _emptyContainer;
    
    [Export] private Dictionary<Elements, PackedScene> _blockScenes;
    
    public override void _Ready()
    {
        _vboxContainer = GetNode<VBoxContainer>("BlockMarginContainer/VBoxContainer");
        _emptyContainer = GetNode<VBoxContainer>("BlockMarginContainer/EmptyContainer");
        _blockContainer = GetNode<VBoxContainer>("BlockMarginContainer/VBoxContainer/BlockContainer");
        _pageNumber = GetNode<Label>("BlockMarginContainer/VBoxContainer/PageNumber");

        _pageNumber.Text = "1.";
    }
    
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        
        if (!IsInstanceValid(Plugin.GetMainWindow()) || !Plugin.GetMainWindow().IsVisible())
            return;
        
        switch (@event)
        {
            case InputEventKey keyEvent when @event.IsPressed():
            {
                switch (keyEvent.Keycode)
                {
                    //  屏蔽textedit的回车键输入, 创建新block
                    case Key.Enter or Key.KpEnter:
                        AddBlock();
                        // 将事件标记为已处理，阻止进一步传播
                        GetViewport().SetInputAsHandled();
                        break;
                    case Key.Backspace:
                    {
                        if (UnindentBlock())
                            GetViewport().SetInputAsHandled();
                        break;
                    }
                    case Key.Up:
                        FocusBlock(keyEvent.Keycode);
                        break;
                    case Key.Down:
                        FocusBlock(keyEvent.Keycode);
                        break;
                    case Key.Tab:
                        IndentBlock();
                        GetViewport().SetInputAsHandled();
                        break;
                }

                break;
            }
            // 鼠标双击创建新block
            case InputEventMouseButton mouseEvent:
            {
                if (mouseEvent.DoubleClick && mouseEvent.ButtonIndex == MouseButton.Left)
                {
                    if (!CheckBeginEdit()) AddBlock();
                }

                break;
            }
        }
        
        // CheckBeginEdit();
    }

    public void SetPageNumber(string pageNumber)
    {
        _pageNumber.Text = pageNumber;
    }

    private BlockScene GetBlockByUid(Guid uid)
    {
        foreach (var node in _blockContainer.GetChildren())
        {
            var block = (BlockScene)node;

            if (block.Uid == uid) return block;
        }
        
        return null;
    }
    
    /// <summary>
    /// 空白页面提示面板显隐逻辑
    /// </summary>
    private bool CheckBeginEdit()
    {
        var visible = IsInstanceValid(GrabBlock);
        
        _vboxContainer.Visible = visible;
        _emptyContainer.Visible = !visible;
        
        return visible;
    }

    /// <summary>
    /// 缩进block,构建父子树形结构
    /// 1.检索最近的同层级block,设置为其子节点
    /// 2.重建父子关系树,即该block父节点改变的时候,其子节点的父节点也替换为其旧父节点
    /// </summary>
    private void IndentBlock()
    {
        var currentBlock = GrabBlock;
        
        if (currentBlock == null) return;
        
        // 1.遍历查找最近的
        BlockScene indentBlock = null;
        foreach (var node in _blockContainer.GetChildren())
        {
            var block = (BlockScene)node;
            
            // 只需要判断序号比自己小的即可
            if (block.GetIndex() >= currentBlock.GetIndex()) continue;
            
            if (block.Parent.Uid == currentBlock.Parent.Uid)
            {
                indentBlock = block;
            }
        }
        
        // 2.重置父子关系树
        if (indentBlock != null) currentBlock.IndentParent(indentBlock);
    }
    
    /// <summary>
    /// 顶格块,重新构建父子关系树
    /// 1.若当前节点无后排兄弟节点,则顶格一级,其子节点(以及子节点的子节点)跟着顶格一级
    /// 2.否则删除该节点,复制其文本到前面的兄弟节点
    /// </summary>
    private bool UnindentBlock()
    {
        var currentBlock = GrabBlock;
        
        if (!IsInstanceValid(currentBlock) || !currentBlock.CanDestroy()) return false;

        var frontBlock = _blockContainer.GetChildOrNull<BlockScene>(currentBlock.GetIndex() - 1);
        var backBlock = _blockContainer.GetChildOrNull<BlockScene>(currentBlock.GetIndex() + 1);

        if (frontBlock == currentBlock || backBlock == currentBlock)
        {
            DelBlock(currentBlock, null);
            return true;
        }

        if (IsInstanceValid(backBlock) && backBlock.Parent.Uid == currentBlock.Parent.Uid)
        {
            DelBlock(currentBlock, frontBlock);
            return true;
        }
        
        if (currentBlock.Parent.Uid == this.Uid)
        {
            DelBlock(currentBlock, frontBlock);
            return true;
        }
        
        currentBlock.Parent.ChildrenBlocks.Remove(currentBlock);
        currentBlock.Parent = currentBlock.Parent.Parent ?? this;
        currentBlock.Parent.ChildrenBlocks.Add(this);
        currentBlock.UnindentParent(currentBlock.Parent, currentBlock.Parent != this);
        
        return false;
    }
    
    /// <summary>
    /// 在指定位置插入block,需要处理所有的父子关系
    /// </summary>
    /// <param name="currentBlock"></param>
    /// <param name="parent"></param>
    public void InsertBlock(BlockScene currentBlock, BlockScene parent)
    {
        
    }
    
    /// <summary>
    /// 转换元素类型
    /// </summary>
    /// <param name="index"></param>
    /// <param name="block"></param>
    public void TurnInto(int index, BlockScene block)
    {
        var blockType = (Elements)index;
        var newBlock = BlockFactory.Instance.AddBlockScene(blockType, _blockScenes);
        
        if (newBlock != null)
        {
            newBlock.PageBlockScene = this;
            newBlock.IndentParent(block.Parent, block.Parent != this);
        
            newBlock.FocusEntered += () => OnBlockOnFocusEntered(newBlock);
            newBlock.FocusExited += () =>  OnBlockOnFocusExited(newBlock);
        
            _blockContainer.AddChild(newBlock);
            _blockContainer.MoveChild(newBlock, block.GetIndex());
            _blockContainer.RemoveChild(block);
        }
    }
    
    /// <summary>
    /// 切换关注的block
    /// </summary>
    public void FocusBlock()
    {
        var block = (BlockScene)_blockContainer.GetChild(_blockContainer.GetChildCount() - 1);
        block.SetFocus();
    }
    
    /// <summary>
    /// 切换关注的block
    /// </summary>
    private void FocusBlock(Key keycode)
    {
        var block = GrabBlock;
        
        if (!IsInstanceValid(block)) return;
        
        var index = block.GetIndex();
        
        block = keycode switch
        {
            Key.Up => _blockContainer.GetChildOrNull<BlockScene>(index - 1),
            Key.Down => _blockContainer.GetChildOrNull<BlockScene>(index + 1),
            _ => block
        };
        
        if (block != null) GrabBlock = block;
    }
    
    /// <summary>
    /// 添加块
    /// </summary>
    private void AddBlock(BlockScene parent = null)
    {
        // 固定每页的长度
        if (_blockContainer.GetChildCount() >= MaxBlocks)
        {
            SEdit.AddPage();
            return;
        }

        var currentBlockScene = GrabBlock;
        parent ??= currentBlockScene?.Parent ?? this;
        
        var toIndex = IsInstanceValid(currentBlockScene) ? currentBlockScene.GetIndex() + 1 : 0;
        var newBlock = BlockFactory.Instance.AddBlockScene(Elements.Text, _blockScenes);
        newBlock.IndentParent(parent, parent != this);
        newBlock.PageBlockScene = this;
        
        newBlock.FocusEntered += () => OnBlockOnFocusEntered(newBlock);
        newBlock.FocusExited += () =>  OnBlockOnFocusExited(newBlock);
        
        _blockContainer.AddChild(newBlock);
        _blockContainer.MoveChild(newBlock, toIndex);
        
        GrabBlock = newBlock;
        CheckBeginEdit();
    }
    
    /// <summary>
    /// 删除块,删掉的块进入undo堆栈中
    /// </summary>
    private void DelBlock(BlockScene currentBlockScene, BlockScene nextBlock)
    {
        _blockContainer.RemoveChild(currentBlockScene);
        // TODO: input del block into undo stack. 
            
        // if index < 0 means that all block already been deleted.
        nextBlock?.Destroy(currentBlockScene);
        GrabBlock = nextBlock;

        if (!CheckBeginEdit())
        {
            SEdit.CallDeferred("DelPage", this);
        }
    }
    
    /// <summary>
    /// 当前选中的块
    /// </summary>
    /// <param name="block"></param>
    private void OnBlockOnFocusEntered(BlockScene block)
    {
        _grabBlock = block;
    }

    private void OnBlockOnFocusExited(BlockScene block)
    {
        
    }
}
