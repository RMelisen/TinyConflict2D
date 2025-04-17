using System.Linq;
using Godot;
using TinyConflict2D.Core.Players;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.UI.Menus;

public partial class ProductionBuildingMenu : CanvasLayer
{
	[Signal]
	public delegate void UnitSelectedEventHandler(string unitType);

	#region Fields

	internal Button[] _buttons;
	internal int[] _unitPrices;
	internal int _currentButtonIndex = 0;

	public virtual string MenuPanelType { get; } = "";

	#endregion
	
	#region Godot Methods
	
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
				if (!_buttons[_currentButtonIndex].Disabled) 
					_buttons[_currentButtonIndex].EmitSignal("pressed");
			}
			GetViewport().SetInputAsHandled();
		}
	}
	
	#endregion
	
	#region Button Events
	
	public void OnUnitButtonPressed(string unitType)
	{
		GD.Print($"{unitType} button pressed");
		EmitSignal(nameof(UnitSelected), unitType);
		HideMenu();
	}
	
	#endregion
	
	#region Visibility
	
	public void ShowMenu(Vector2 position, int playerMoney)
	{
		Visible = true;
		_currentButtonIndex = 0;
		_buttons[_currentButtonIndex].GrabFocus();
		GetNode<Control>(MenuPanelType).Position = position;

		for (int i = 0; i < _buttons.Length; i++)
		{
			if (_unitPrices[i] > playerMoney)
				_buttons[i].Disabled = true;
			else
				_buttons[i].Disabled = false;
		}
	}

	public void HideMenu()
	{
		Visible = false;
	}
	
	#endregion
}