using Godot;
using System;
using Godot.Collections;
using Screenplay.Resources;

namespace Screenplay.Windows;


[Tool]
public partial class Workspace : VBoxContainer
{
    public event Action<ScreenplayResource> EventTabClosed = delegate { };
    public event Action<ScreenplayResource> EventTabSelected = delegate { };
	
    private TabBar _tabBar;
    private TabContainer _tabContainer;
    
    public override void _Ready()
    {
        _tabBar = GetNode<TabBar>("TabBar");
        _tabContainer = GetNode<TabContainer>("TabContainer");

        _tabBar.TabCloseDisplayPolicy = TabBar.CloseButtonDisplayPolicy.ShowAlways;
        _tabBar.TabSelected += OnTabSelected;
        _tabBar.TabClosePressed += OnTabClosePressed;
    }
	
    /// <summary>
    /// 关闭选项卡
    /// </summary>
    /// <param name="tab"></param>
    private void OnTabClosePressed(long tab)
    {
        EventTabClosed?.Invoke(GetTabEditor((int)tab).MScreenplayResource);
        _tabBar.RemoveTab((int)tab);
        _tabContainer.RemoveChild(GetTabEditor((int)tab));
    }
	
    /// <summary>
    /// 选项卡切换
    /// </summary>
    /// <param name="tab"></param>
    private void OnTabSelected(long tab)
    {
        _tabContainer.CurrentTab = (int)tab;
        EventTabSelected?.Invoke(GetCurrentEditor().MScreenplayResource);
    }
	
    /// <summary>
    /// 设置当前选项卡
    /// </summary>
    /// <param name="index"></param>
    public void SetCurrentTab(int index)
    {
        _tabBar.CurrentTab = index;
        _tabContainer.CurrentTab = index;
        EventTabSelected?.Invoke(GetTabEditor(index).MScreenplayResource);
    }
	
    /// <summary>
    /// 返回当前选项卡数量
    /// </summary>
    /// <returns></returns>
    public int GetTabCount()
    {
        return _tabContainer.GetTabCount();
    }
	
    /// <summary>
    /// 获取当前选项卡内的节点图编辑器
    /// </summary>
    /// <returns></returns>
    public Editor GetCurrentEditor()
    {
        return (Editor)_tabContainer.GetCurrentTabControl();
    }

    public Editor GetTabEditor(int index)
    {
        return (Editor)_tabContainer.GetTabControl(index);
    }

    public int GetTabEditor(string filepath)
    {
        for (var i = 0; i < _tabContainer.GetTabCount(); i++)
        {
            var editor = GetTabEditor(i);
            if (filepath == editor.MScreenplayResource.Filepath) return i;
        }

        return -1;
    }

    /// <summary>
    /// 新增节点图到工作区中
    /// </summary>
    /// <param name="editor"></param>
    /// <param name="data"></param>
    public void AddEditor(Editor editor, ScreenplayResource data)
    {
        _tabContainer.AddChild(editor);
        editor.LoadData(data);
        _tabBar.AddTab(editor.MScreenplayResource.Filename);
    }
}
