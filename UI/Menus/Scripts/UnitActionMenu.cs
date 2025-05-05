using Godot;
using TinyConflict2D.Commons.Config;

namespace TinyConflict2D.UI.Menus;

public partial class UnitActionMenu : CanvasLayer
{
	[Signal]
	public delegate void ButtonSelectedEventHandler(string unitType);

	#region Fields
	
	private Button[] _buttons;
	private int _currentButtonIndex = 0;
	
	#endregion
	
	#region Godot Methods
	
	public override void _Ready()
	{
		// Store buttons
		_buttons = new Button[]
		{
			GetNode<Button>("ButtonsContainer/AttackButton"),
			GetNode<Button>("ButtonsContainer/WaitButton"),
			GetNode<Button>("ButtonsContainer/SupplyButton"),
			GetNode<Button>("ButtonsContainer/CaptureButton")
		};
	}

	public override void _Input(InputEvent @event)
	{
		if (Visible)
		{
			if (@event.IsActionPressed("ui_cancel"))
			{
				EmitSignal(nameof(ButtonSelected), Config.UNITACTION_WAIT);
				HideMenu();
			}
			else if (@event.IsActionPressed("ui_down"))
			{
				_currentButtonIndex = (_currentButtonIndex + 1) % _buttons.Length;
				_buttons[_currentButtonIndex].GrabFocus();
			}
			else if (@event.IsActionPressed("ui_up"))
			{
				_currentButtonIndex = (_currentButtonIndex - 1 + _buttons.Length) % _buttons.Length;
				_buttons[_currentButtonIndex].GrabFocus();
			}
			else if (@event.IsActionPressed("ui_accept"))
			{
				_buttons[_currentButtonIndex].EmitSignal("pressed");
			}
			GetViewport().SetInputAsHandled();
		}
	}
	
	#endregion
	
	#region Button Events
	
	public void OnButtonPressed(string selectedUnitActionButton)
	{
		GD.Print($"{selectedUnitActionButton} button pressed");
		EmitSignal(nameof(ButtonSelected), selectedUnitActionButton);
		HideMenu();
	}
	
	#endregion
	
	#region Visibility
	
	public void ShowMenu(bool enableAttackButton, bool enableSupplybutton, bool enableCaptureButton)
	{
		Visible = true;

		_buttons[0].Disabled = !enableAttackButton;
		_buttons[2].Disabled = !enableSupplybutton;
		_buttons[3].Disabled = !enableCaptureButton;

		_currentButtonIndex = 0;
		_buttons[_currentButtonIndex].GrabFocus();
	}

	public void HideMenu()
	{
		Visible = false;
	}

	#endregion
}
