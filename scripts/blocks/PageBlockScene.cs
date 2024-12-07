using Godot;
using System;
using Godot.Collections;


[Tool]
public partial class PageBlockScene : BlockScene
{
    [Export] private Dictionary<Elements, PackedScene> _blockScenes = new();
    
    private const int _maxBlocks = 20;
    
    private VBoxContainer _blockContainer;
    private VBoxContainer _emptyContainer;
    private TextBlockScene _currentBlockScene;
    
    public override void _Ready()
    {
        _emptyContainer = GetNode<VBoxContainer>("EmptyContainer");
        _blockContainer = GetNode<VBoxContainer>("BlockContainer");
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && @event.IsPressed() && Plugin.GetMainWindow().IsVisible())
        {
            //  屏蔽textedit的回车键输入
            if (keyEvent.Keycode is Key.Enter or Key.KpEnter)
            {
                AddBlock();
                // 将事件标记为已处理，阻止进一步传播
                GetViewport().SetInputAsHandled();
            }
            else if (keyEvent.Keycode is Key.Backspace)
            {
                if (IsInstanceValid(_currentBlockScene) && _currentBlockScene.CanDestroy())
                {
                    DelBlock();
                    GetViewport().SetInputAsHandled();
                }
            }
        }
        
        CheckBeginEdit();
        
        base._Input(@event);
    }
    
    /// <summary>
    /// 空白页面提示面板显隐逻辑
    /// </summary>
    /// <param name="visible"></param>
    private void CheckBeginEdit()
    {
        var visible = IsInstanceValid(_currentBlockScene);
        _blockContainer.Visible = visible;
        _emptyContainer.Visible = !visible;
    }

    private void AddBlock()
    {
        if (_blockContainer.GetChildCount() < _maxBlocks)
        {
            var index = IsInstanceValid(_currentBlockScene) ? _currentBlockScene.GetIndex() + 1 : 0;
            var block = _blockScenes[Elements.Text].Instantiate<TextBlockScene>();
            
            _blockContainer.AddChild(block);
            _blockContainer.MoveChild(block, index);
            _currentBlockScene = block;
            
            block.GrabFocus();
            block.FocusEntered += () =>
            {
                _currentBlockScene = block; 
                GD.Print("Add Block: " + _currentBlockScene.GetIndex());
            };
        }
    }

    private void DelBlock()
    {
        if (_currentBlockScene != null)
        {
            var nextBlockIndex = _currentBlockScene.GetIndex() - 1;
            GD.Print("Del Block: " + nextBlockIndex);
            _blockContainer.RemoveChild(_currentBlockScene);
            _currentBlockScene.QueueFree();
            
            // if index < 0 means that all block already been deleted.
            _currentBlockScene = _blockContainer.GetChildOrNull<TextBlockScene>(nextBlockIndex);
            _currentBlockScene?.GrabFocus();
        }
    }
}
