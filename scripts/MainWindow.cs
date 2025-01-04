using Godot;
using Godot.Collections;
using Screenplay.Blocks;
using Screenplay.File;
using Screenplay.Resources;

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
    
    public override void _Ready()
    {
        InitMenu();
        InitParam();
        InitEvent();
        _workspace = GetNode<Workspace>("Main/Workspace");
    }
    
    /// <summary>
	/// 初始化GUI
	/// </summary>
	private void InitMenu()
	{
		_fileManager = GetNode<FileManager>("FileManager");
		_workspace   = GetNode<Workspace>("Main/Workspace");
		_version     = GetNode<Label>("Main/Status/Version");
		_fileMenu    = GetNode<MenuButton>("Main/Menus/FileMenu");
		_debugMenu   = GetNode<MenuButton>("Main/Menus/DebugMenu");
		_newDialog   = GetNode<FileDialog>("FileManager/NewFileDialog");
		_saveDialog  = GetNode<FileDialog>("FileManager/SaveFileDialog");
		_openDialog  = GetNode<FileDialog>("FileManager/OpenFileDialog");
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

		// 注册快捷键
		var shortcut = new Shortcut();
		var saveEvent = new InputEventKey()
		{
			CtrlPressed = true, // 按下Ctrl键
			ShiftPressed = false, // 未按下Shift键
			AltPressed = false, // 未按下Alt键
			Keycode = Key.S, // 按下S键
		};
		shortcut.Events.Add(saveEvent);
		_fileMenu.GetPopup().SetItemShortcut(2, shortcut);
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
				_openDialog.PopupCentered();
				break;
			case 2:
				_fileManager.SaveFile();
				break;
			case 3:
				_saveDialog.PopupCentered();
				break;
		}
	}

	private void OnNewFile(string filepath)
	{
		_fileManager.NewFile(_newDialog.CurrentDir, _newDialog.CurrentFile, filepath);
	}

	private void OnOpenFile(string filepath)
	{
		_fileManager.OpenFile(_openDialog.CurrentDir, _openDialog.CurrentFile, filepath);
	}

	private void OnSaveAsFile(string filepath)
	{
		_fileManager.SaveFile(filepath);
	}
	
	private void WorkspaceOnEventTabClosed(ScreenplayResource resource)
	{
		
	}

	private void WorkspaceOnEventTabSelected(ScreenplayResource resource)
	{
		
	}
}
