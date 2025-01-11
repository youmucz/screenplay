using Godot;
using System;
namespace Screenplay.Windows.Templates;


[Tool]
public partial class TemplateWindow : PopupPanel
{
	private TemplateTree _templateTree;
	
	private Button _myTempButton;
	private Button _scriptsButton;
	private Button _documentsButton;
	
	private Button _okButton;
	private Button _cancelButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_templateTree = GetNode<TemplateTree>("VBoxContainer/MidContainer/Tree");
		_templateTree.ItemActivated += TemplateTreeOnItemActivated;
		_templateTree.ItemMouseSelected += TemplateTreeOnItemMouseSelected;
		
		_myTempButton = GetNode<Button>("VBoxContainer/TopContainer/MyTemplates");
		_scriptsButton = GetNode<Button>("VBoxContainer/TopContainer/Scripts");
		_documentsButton = GetNode<Button>("VBoxContainer/TopContainer/Documents");
		
		_okButton = GetNode<Button>("VBoxContainer/BotContainer/Ok");
		_cancelButton = GetNode<Button>("VBoxContainer/BotContainer/Cancel");
		
		_myTempButton.Pressed += MyTempButtonOnPressed;
		_scriptsButton.Pressed += ScriptsButtonOnPressed;
		_documentsButton.Pressed += DocumentsButtonOnPressed;
		_okButton.Pressed += OkButtonOnPressed;
		_cancelButton.Pressed += CancelButtonOnPressed;
	}
	
	/// <summary>
	/// Emitted when an item is double-clicked, or selected with a ui_accept input event (e. g. using Enter or Space on the keyboard). 
	/// </summary>
	private void TemplateTreeOnItemActivated()
	{
		Hide();
	}
	
	/// <summary>
	/// Emitted when an item is selected with a mouse button.
	/// </summary>
	/// <param name="mousePosition"></param>
	/// <param name="mouseButtonIndex"></param>
	private void TemplateTreeOnItemMouseSelected(Vector2 mousePosition, long mouseButtonIndex)
	{
		
	}

	private void DocumentsButtonOnPressed()
	{
		_templateTree.AddDocuments();
	}

	private void ScriptsButtonOnPressed()
	{
		_templateTree.AddScripts();
	}

	private void MyTempButtonOnPressed()
	{
		_templateTree.AddTemplates();
	}

	private void OkButtonOnPressed()
	{
		Hide();
	}

	private void CancelButtonOnPressed()
	{
		Hide();
	}
}
