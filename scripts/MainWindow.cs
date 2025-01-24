using Godot;
using Godot.Collections;
using Screenplay.Blocks;
using Screenplay.File;
using Screenplay.Resources;
using Screenplay.Utils;
using Screenplay.Windows.Templates;

namespace Screenplay.Windows;


[Tool]
public partial class MainWindow : Control
{
    private Label _version;
    private Workspace _workspace;
    private MenuButton _fileMenu;
    private MenuButton _debugMenu;
    private FileDialog _newDialog;
    private FileDialog _saveDialog;
    private FileDialog _openDialog;
    private FileManager _fileManager;
    private TemplateWindow _tempWindow;
    
    private Elements? _elements;

    public override void _Ready()
    {
        InitMenu();
        InitParam();
        InitEvent();
    }
    
    /// <summary>
	/// 初始化GUI
	/// </summary>
	private void InitMenu()
	{
		_fileManager = GetNode<FileManager>("FileManager");
		_workspace   = GetNode<Workspace>("Main/Workspace");
		_version     = GetNode<Label>("Main/Status/Version");
		_fileMenu    = GetNode<MenuButton>("Main/Menus/FileMenuButton");
		_debugMenu   = GetNode<MenuButton>("Main/Menus/DebugMenuButton");
		_newDialog   = GetNode<FileDialog>("FileManager/NewFileDialog");
		_saveDialog  = GetNode<FileDialog>("FileManager/SaveFileDialog");
		_openDialog  = GetNode<FileDialog>("FileManager/OpenFileDialog");
		_tempWindow  = GetNode<TemplateWindow>("FileManager/TemplateWindow");
	}

	/// <summary>
	/// 初始化参数
	/// </summary>
	private void InitParam()
	{
		_fileManager.MainWindow = this;
		_fileManager.Workspace = _workspace;
		_version.Text = "1.2.4";
	}

	/// <summary>
	/// 绑定事件
	/// </summary>
	private void InitEvent()
	{
		_newDialog.FileSelected += OnNewFile;
		_openDialog.FileSelected += OnOpenFile;
		_saveDialog.FileSelected += OnSaveAsFile;

		_fileMenu.GetPopup().IndexPressed += OnFileMenuPressed;
		
		_fileManager.Workspace.EventTabSelected += WorkspaceOnEventTabSelected;
		_fileManager.Workspace.EventTabClosed += WorkspaceOnEventTabClosed;
		
		_tempWindow.EventTemplateItemSelected += TempWindowOnEventTemplateItemSelected;
	}

	public void PopupTemplateWindow()
	{
		_tempWindow.PopupCentered();
	}

	/// <summary>
	/// 文件菜单
	/// </summary>
	/// <param name="index">0:新建 1:打开 2:保存 3:另存为</param>
	private void OnFileMenuPressed(long index)
	{
		switch (index)
		{
			case 0:
				_newDialog.PopupCentered();
				break;
			case 1:
				_tempWindow.PopupCentered();
				break;
			case 3:
				_openDialog.PopupCentered();
				break;
			case 4:
				_fileManager.SaveFile();
				break;
			case 5:
				_saveDialog.PopupCentered();
				break;
		}
	}

	private void OnNewFile(string filepath)
	{
		if (_elements != null)
		{
			_fileManager.NewFileFromTemplate(_newDialog.CurrentDir, _newDialog.CurrentFile, filepath, _elements);
		}
		else
		{
			_fileManager.NewFile(_newDialog.CurrentDir, _newDialog.CurrentFile, filepath);
		}
		
		_elements = null;
	}

	private void OnOpenFile(string filepath)
	{
		_fileManager.OpenFile(_openDialog.CurrentDir, _openDialog.CurrentFile, filepath);
	}

	private void OnSaveAsFile(string filepath)
	{
		_fileManager.SaveFile(filepath);
	}
	
	private void TempWindowOnEventTemplateItemSelected(Elements element)
	{
		_elements = element;
		_newDialog.PopupCentered();
	}
	
	private void WorkspaceOnEventTabClosed(EditorResource resource)
	{
		
	}

	private void WorkspaceOnEventTabSelected(EditorResource resource)
	{
		
	}
}
