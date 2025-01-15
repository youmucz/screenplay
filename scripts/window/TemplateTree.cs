using Godot;
using System;
using Godot.Collections;

namespace Screenplay.Windows.Templates;


[Tool]
public partial class TemplateTree : Tree
{
	private TreeItem _root;
	private Texture2D _fileThumbnail = GD.Load<Texture2D>("res://addons/screenplay/resources/icons/Files/file-text.svg");
	
	[Export] private Dictionary<string, Texture2D> _scriptsTempThumb = new ();
	[Export] private Dictionary<string, Texture2D> _documentTempThumb = new ();
	[Export] private Dictionary<string, Texture2D> _myTemplatesTempThumb = new ();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	/// <summary>
	/// 重置模板数据到树形窗口中
	/// </summary>
	public void AddScripts()
	{
		Clear();
		
		_root = CreateItem();
		
		foreach (var name in _scriptsTempThumb.Keys)
		{
			var item = _root.CreateChild();
			item.SetText(0, name);
			item.SetIcon(0, _fileThumbnail);
		}
	}
	
	public void AddDocuments()
	{
		Clear();
		
		_root = CreateItem();
		
		foreach (var name in _documentTempThumb.Keys)
		{
			var item = _root.CreateChild();
			item.SetText(0, name);
			item.SetIcon(0, _fileThumbnail);
		}
	}

	public void AddTemplates()
	{
		Clear();
		
		_root = CreateItem();
		
		foreach (var name in _myTemplatesTempThumb.Keys)
		{
			var item = _root.CreateChild();
			item.SetText(0, name);
			item.SetIcon(0, _fileThumbnail);
		}
	}
}
