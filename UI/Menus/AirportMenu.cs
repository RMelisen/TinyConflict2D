using Godot;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.UI.Menus;

public partial class AirportMenu : CanvasLayer
{
	[Signal]
	public delegate void UnitSelectedEventHandler(string unitType);

	private Control[] _buttons;
	private int _currentButtonIndex = 0;
	
	public override void _Ready()
	{
		// Store buttons
		_buttons = new Control[]
		{
			GetNode<Button>("AirportMenuPanel/BCopterButton"),
			GetNode<Button>("AirportMenuPanel/TCopterButton"),
			GetNode<Button>("AirportMenuPanel/BomberButton")
		};
		
		// Set unit prices in the buttons labels
		GetNode<Label>("AirportMenuPanel/BCopterButton/BCopterContainer/BCopterCost").Text = BCopterUnit.BasePrice.ToString();
		GetNode<Label>("AirportMenuPanel/TCopterButton/TCopterContainer/TCopterCost").Text = TCopterUnit.BasePrice.ToString();
		GetNode<Label>("AirportMenuPanel/BomberButton/BomberContainer/BomberCost").Text = BomberUnit.BasePrice.ToString();

		_buttons[_currentButtonIndex].GrabFocus();
	}

	public override void _Input(InputEvent @event)
	{
		if (Visible)
		{
			if (@event.IsActionPressed("ui_cancel"))
			{
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
			else if (@event.IsActionPressed("ui_left"))
			{
				if (_currentButtonIndex - 4 >= 0)
				{
					_currentButtonIndex = (_currentButtonIndex - 4 + _buttons.Length) % _buttons.Length;
					_buttons[_currentButtonIndex].GrabFocus();
				}
			}
			else if (@event.IsActionPressed("ui_right"))
			{
				if (_currentButtonIndex + 4 < _buttons.Length)
				{
					_currentButtonIndex = (_currentButtonIndex + 4) % _buttons.Length;
					_buttons[_currentButtonIndex].GrabFocus();
				}
			}
			else if (@event.IsActionPressed("ui_accept"))
			{
				_buttons[_currentButtonIndex].EmitSignal("pressed");
			}
			GetViewport().SetInputAsHandled();
		}
	}
	
	#region Button Events
	
	public void OnUnitButtonPressed(string unitType)
	{
		GD.Print($"{unitType} button pressed");
		EmitSignal(nameof(UnitSelected), unitType);
		HideMenu();
	}
	
	#endregion
	
	#region Visibility
	
	public void ShowMenu(Vector2 position)
	{
		Visible = true;
		_currentButtonIndex = 0;
		_buttons[_currentButtonIndex].GrabFocus();
		GetNode<Control>("AirportMenuPanel").Position = position;
	}

	public void HideMenu()
	{
		Visible = false;
	}
	
	#endregion
}
