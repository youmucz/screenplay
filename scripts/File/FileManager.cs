using Godot;
using System;
using Godot.Collections;
using Screenplay.Windows;
using Screenplay.Resources;
using Array = Godot.Collections.Array;

namespace Screenplay.File;


[Tool]
public partial class FileManager : Control
{
	public Workspace Workspace;
    public MainWindow MainWindow;
	
	private PackedScene _screenplayEdit;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenplayEdit = GD.Load<PackedScene>("res://addons/screenplay/scenes/windows/screenplay_edit.tscn");
	}
	
	public void NewFile(string dir, string filename, string filepath)
	{
		var tabIndex = Workspace.GetTabEditor(filepath);
		
		if (tabIndex >= 0)
		{
			Workspace.SetCurrentTab(tabIndex);
		}
		else
		{
			CreateFile(dir, filename, filepath);
		}
	}

	public void OpenFile(string dir, string filename, string filepath)
	{
		/*
		var filepath = Path.Combine(dir, filename).Replace("\\", "/");
		*/
		var tabIndex = Workspace.GetTabEditor(filepath);
		
		if (tabIndex >= 0)
		{
			Workspace.SetCurrentTab(tabIndex);
		}
		else
		{
			var data = ResourceLoader.Load<ScreenplayResource>(filepath, "", ResourceLoader.CacheMode.Replace);
			data.FileDir = dir;
			data.Filename = filename;
			data.Filepath = filepath;
			
			var editor = _screenplayEdit.Instantiate<ScreenplayEdit>();
			Workspace.AddEditor(editor);
			editor.LoadData(data);
		}
		
		GD.Print("打开文件:", filename);
	}

	/// <summary>
	/// Create local res file.
	/// </summary>
	/// <param name="dir"></param>
	/// <param name="filename"></param>
	/// <param name="filepath"></param>
	public void CreateFile(string dir, string filename, string filepath)
	{
		var editor = _screenplayEdit.Instantiate<ScreenplayEdit>();
		
		// var pageData = new Array<Dictionary>(){
		// 	new()
		// 	{
		// 		{"BlockGuid", Guid.NewGuid().ToString()}, 
		// 		{"BlockType", Elements.Page.ToString()}, 
		// 		{"BlockParent", ""},
		// 		{"Content", new Array()},
		// 		{"Properties", new Dictionary()},
		// 	}
		// };
	
		var data = new ScreenplayResource()
		{
			FileDir = dir,
			Filename = filename,
			Filepath = filepath,
			// PageData = pageData,
		};
		
		Workspace.AddEditor(editor);
		editor.LoadData(data);
		var error = ResourceSaver.Save(data, filepath);
		
		if (error == Error.Ok)
		{
			GD.Print("New Dialogue create successfully!");
		}
		else
		{
			GD.Print("New Dialogue create failed. Error: " + error + " path:" + filepath);
		}
	}

	public void SaveFile()
	{
		var editor = Workspace.GetCurrentEditor();
		
		if (editor == null) return;
		
		var data = editor.DumpsData();
		var error = ResourceSaver.Save(data, data.Filepath);
		if (error == Error.Ok)
		{
			GD.Print("Dialogue Resource saved successfully!");
		}
		else
		{
			GD.Print("Failed to save dialogue resource. Error: " + error);
		}
	}

	public void SaveFile(string filepath)
	{
		var tabIndex = Workspace.GetTabEditor(filepath);
		if (tabIndex >= 0)
		{
			var editor = Workspace.GetTabEditor(tabIndex);
			var error = ResourceSaver.Save(editor.DumpsData(), filepath);
			
			if (error == Error.Ok)
			{
				GD.Print("Dialogue Resource saved successfully!");
			}
			else
			{
				GD.Print("Failed to save dialogue resource. Error: "+ error);
			}
		}
	}
}
