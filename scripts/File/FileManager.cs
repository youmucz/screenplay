using Godot;
using System;
using Godot.Collections;
using Screenplay.Windows;
using Screenplay.Resources;
using Screenplay.Utils;
using Array = Godot.Collections.Array;

namespace Screenplay.File;


[Tool]
public partial class FileManager : Control
{
	public Workspace Workspace;
    public MainWindow MainWindow;
	
	private PackedScene _editor;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_editor = GD.Load<PackedScene>("res://addons/screenplay/scenes/windows/editor.tscn");
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
	
	/// <summary>
	/// Create a doc from template.
	/// </summary>
	/// <param name="dir"></param>
	/// <param name="filename"></param>
	/// <param name="filepath"></param>
	/// <param name="elements"></param>
	public void NewFileFromTemplate(string dir, string filename, string filepath, Elements? elements)
	{
		var tabIndex = Workspace.GetTabEditor(filepath);
		
		if (tabIndex >= 0)
		{
			Workspace.SetCurrentTab(tabIndex);
		}
		else
		{
			CreateFile(dir, filename, filepath, elements);
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
			var data = ResourceLoader.Load<EditorResource>(filepath, "", ResourceLoader.CacheMode.Replace);
			data.FileDir = dir;
			data.Filename = filename;
			data.Filepath = filepath;
			
			var editor = _editor.Instantiate<Editor>();
			Workspace.AddEditor(editor, data);
		}
		
		GD.Print("打开文件:", filename);
	}

	/// <summary>
	/// Create local res file.
	/// </summary>
	/// <param name="dir"></param>
	/// <param name="filename"></param>
	/// <param name="filepath"></param>
	/// <param name="elements"></param>
	public void CreateFile(string dir, string filename, string filepath, Elements? elements=Elements.Document)
	{
		var editor = _editor.Instantiate<Editor>();
	
		var data = new EditorResource()
		{
			FileDir = dir,
			Filename = filename,
			Filepath = filepath,
			Data = new Dictionary()
			{
				{"BlockType", elements.ToString()}
			}
		};
		
		Workspace.AddEditor(editor, data);
		var error = ResourceSaver.Save(data, filepath);
		
		if (error == Error.Ok)
		{
			GD.Print($"New {elements.ToString()} file create successfully!");
		}
		else
		{
			GD.Print($"New {elements.ToString()} file create failed. Error: " + error + " path:" + filepath);
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
			GD.Print($"{data.Filename} File Resource saved successfully!");
		}
		else
		{
			GD.Print($"Failed to save {data.Filename} file resource. Error: "+ error);
		}
	}

	public void SaveFile(string filepath)
	{
		var tabIndex = Workspace.GetTabEditor(filepath);
		if (tabIndex >= 0)
		{
			var editor = Workspace.GetTabEditor(tabIndex);
			var data = editor.DumpsData();
			var error = ResourceSaver.Save(data, filepath);
			
			if (error == Error.Ok)
			{
				GD.Print($"{data.Filename} File Resource saved successfully!");
			}
			else
			{
				GD.Print($"Failed to save {data.Filename} file resource. Error: "+ error);
			}
		}
	}
}
