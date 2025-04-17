using Godot;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.UI.Menus;

public partial class FactoryMenu : CanvasLayer
{
	[Signal]
	public delegate void UnitSelectedEventHandler(string unitType);

	#region Fields
	
	private Control[] _buttons;
	private int _currentButtonIndex = 0;
	
	#endregion
	
	#region Godot Methods
	
	public override void _Ready()
	{
		// Store buttons
		_buttons = new Control[]
		{
			GetNode<Button>("FactoryMenuPanel/InfantryButton"),
			GetNode<Button>("FactoryMenuPanel/MechButton"),
			GetNode<Button>("FactoryMenuPanel/ReconButton"),
			GetNode<Button>("FactoryMenuPanel/AAButton"),
			GetNode<Button>("FactoryMenuPanel/APCButton"),
			GetNode<Button>("FactoryMenuPanel/SupplyButton"),
			GetNode<Button>("FactoryMenuPanel/TankButton")
		};
		
		// Set unit prices in the buttons labels
		GetNode<Label>("FactoryMenuPanel/InfantryButton/InfantryContainer/InfantryCost").Text = InfantryUnit.BASE_PRICE.ToString();
		GetNode<Label>("FactoryMenuPanel/MechButton/MechContainer/MechCost").Text = MechUnit.BASE_PRICE.ToString();
		GetNode<Label>("FactoryMenuPanel/ReconButton/ReconContainer/ReconCost").Text = ReconUnit.BASE_PRICE.ToString();
		GetNode<Label>("FactoryMenuPanel/AAButton/AAContainer/AACost").Text = AAUnit.BASE_PRICE.ToString();
		GetNode<Label>("FactoryMenuPanel/APCButton/APCContainer/APCCost").Text = APCUnit.BASE_PRICE.ToString();
		GetNode<Label>("FactoryMenuPanel/SupplyButton/SupplyContainer/SupplyCost").Text = SupplyUnit.BASE_PRICE.ToString();
		GetNode<Label>("FactoryMenuPanel/TankButton/TankContainer/TankCost").Text = TankUnit.BASE_PRICE.ToString();

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
	
	public void ShowMenu(Vector2 position)
	{
		Visible = true;
		_currentButtonIndex = 0;
		_buttons[_currentButtonIndex].GrabFocus();
		GetNode<Control>("FactoryMenuPanel").Position = position;
	}

	public void HideMenu()
	{
		Visible = false;
	}
	
	#endregion
}
