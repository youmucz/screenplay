using Godot;
using System;
using Godot.Collections;


[Tool]
public partial class PageBlockScene : BlockScene
{
    [Export] private Dictionary<Elements, PackedScene> _blockScenes = new();
    
    private const int MaxBlocks = 20;
    
    private VBoxContainer _blockContainer;
    private VBoxContainer _emptyContainer;
    
    /// <summary>
    /// 当前聚焦的block,通过focus参数进行判断
    /// </summary>
    private TextBlockScene CurrentBlockScene
    {
        set => value?.GrabFocus();
        get
        {
            foreach (var node in _blockContainer.GetChildren())
            {
                if (node is TextBlockScene block && block.HasFocus())
                {
                    return block;
                }
            }
            
            return null;
        }
    }
    
    public override void _Ready()
    {
        _emptyContainer = GetNode<VBoxContainer>("EmptyContainer");
        _blockContainer = GetNode<VBoxContainer>("BlockContainer");
    }
    
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        
        // if (!IsInstanceValid(Plugin.GetMainWindow()) || !Plugin.GetMainWindow().IsVisible())
        //     return;
        
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

    private TextBlockScene GetBlockByUid(Guid uid)
    {
        foreach (var node in _blockContainer.GetChildren())
        {
            var block = (TextBlockScene)node;

            if (block.Uid == uid) return block;
        }
        
        return null;
    }
    
    /// <summary>
    /// 空白页面提示面板显隐逻辑
    /// </summary>
    private bool CheckBeginEdit()
    {
        var visible = IsInstanceValid(CurrentBlockScene);
        _blockContainer.Visible = visible;
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
        var currentBlock = CurrentBlockScene;
        
        if (currentBlock == null) return;
        
        // 1.遍历查找最近的
        TextBlockScene indentBlock = null;
        foreach (var node in _blockContainer.GetChildren())
        {
            var block = (TextBlockScene)node;
            
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
        var currentBlock = CurrentBlockScene;
        
        GD.Print("Unindent Block currentBlock ", currentBlock);
        
        if (!IsInstanceValid(currentBlock)) return false;
        
        TextBlockScene indentBlock = null;
        foreach (var node in _blockContainer.GetChildren())
        {
            var block = (TextBlockScene)node;
            
            // 只需要判断序号比自己小的即可
            if (block.GetIndex() >= currentBlock.GetIndex()) continue;
            
            if (block.Parent.Uid == currentBlock.Parent.Uid)
            {
                indentBlock = block;
            }
        }

        var frontBlock = _blockContainer.GetChildOrNull<TextBlockScene>(currentBlock.GetIndex() - 1);
        var backBlock = _blockContainer.GetChildOrNull<TextBlockScene>(currentBlock.GetIndex() + 1);

        if (frontBlock == currentBlock || backBlock == currentBlock)
        {
            DelBlock(currentBlock, null);
            return true;
        }
        
        if (IsInstanceValid(backBlock) && backBlock.Parent != currentBlock.Parent && currentBlock.CanDestroy())
        {
            GD.Print("Unindent Block backBlock ", backBlock);
            currentBlock.Parent.ChildrenBlocks.Remove(currentBlock);
            currentBlock.Parent = currentBlock.Parent.Parent ?? this;
            currentBlock.Parent.ChildrenBlocks.Add(this);
            currentBlock.UnindentParent(currentBlock.Parent);
            return true;
        }
        
        if (IsInstanceValid(frontBlock) && frontBlock.Parent == currentBlock.Parent && currentBlock.CanDestroy())
        {
            GD.Print("Unindent Block frontBlock ", frontBlock);
            DelBlock(currentBlock, frontBlock);
            return true;
        }
        
        GD.Print("Unindent Block DelBlock ", frontBlock);
        
        DelBlock(currentBlock, frontBlock);

        return false;
    }
    
    /// <summary>
    /// 在指定位置插入block,需要处理所有的父子关系
    /// </summary>
    /// <param name="currentBlock"></param>
    /// <param name="parent"></param>
    private void InsertBlock(TextBlockScene currentBlock, TextBlockScene parent)
    {
        
    }
    
    /// <summary>
    /// 切换关注的block
    /// </summary>
    private void FocusBlock(Key keycode)
    {
        var block = CurrentBlockScene;
        
        if (!IsInstanceValid(block)) return;
        
        var index = block.GetIndex();
        
        block = keycode switch
        {
            Key.Up => _blockContainer.GetChildOrNull<TextBlockScene>(index - 1),
            Key.Down => _blockContainer.GetChildOrNull<TextBlockScene>(index + 1),
            _ => block
        };
        
        CurrentBlockScene = block;
    }
    
    /// <summary>
    /// 添加块
    /// </summary>
    private void AddBlock(BlockScene parent = null)
    {
        // 固定每页的长度
        if (_blockContainer.GetChildCount() >= MaxBlocks) return;

        var currentBlockScene = CurrentBlockScene;
        parent ??= currentBlockScene?.Parent ?? this;
        
        var toIndex = IsInstanceValid(currentBlockScene) ? currentBlockScene.GetIndex() + 1 : 0;
        var newBlock = _blockScenes[Elements.Text].Instantiate<TextBlockScene>();
        newBlock.IndentParent(parent);
        
        _blockContainer.AddChild(newBlock);
        _blockContainer.MoveChild(newBlock, toIndex);
            
        CurrentBlockScene = newBlock;
        CheckBeginEdit();
    }
    
    /// <summary>
    /// 删除块
    /// </summary>
    private void DelBlock(TextBlockScene currentBlockScene, TextBlockScene nextBlock)
    {
        _blockContainer.RemoveChild(currentBlockScene);
        currentBlockScene.QueueFree();
            
        // if index < 0 means that all block already been deleted.
        nextBlock?.SetText(currentBlockScene.GetRelicText());
        CurrentBlockScene = nextBlock;
        
        CheckBeginEdit();
    }
}
