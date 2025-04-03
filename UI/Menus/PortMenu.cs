using Godot;
using System;

public partial class PortMenu : CanvasLayer
{
	[Signal]
	public delegate void UnitSelectedEventHandler(string unitType);

	private Control[] buttons;
	private int currentButtonIndex = 0;
	
	public override void _Ready()
	{
		// Store buttons
		buttons = new Control[]
		{
			GetNode<Button>("PortMenuPanel/LanderButton"),
			GetNode<Button>("PortMenuPanel/SubmarineButton"),
			GetNode<Button>("PortMenuPanel/BattleshipButton")
		};
		
		// Set unit prices in the buttons labels
		GetNode<Label>("PortMenuPanel/LanderButton/LanderContainer/LanderCost").Text = LanderUnit.BasePrice.ToString();
		GetNode<Label>("PortMenuPanel/SubmarineButton/SubmarineContainer/SubmarineCost").Text = SubmarineUnit.BasePrice.ToString();
		GetNode<Label>("PortMenuPanel/BattleshipButton/BattleshipContainer/BattleshipCost").Text = BattleshipUnit.BasePrice.ToString();

		buttons[currentButtonIndex].GrabFocus();
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
				currentButtonIndex = (currentButtonIndex + 1) % buttons.Length;
				buttons[currentButtonIndex].GrabFocus();
			}
			else if (@event.IsActionPressed("ui_up"))
			{
				currentButtonIndex = (currentButtonIndex - 1 + buttons.Length) % buttons.Length;
				buttons[currentButtonIndex].GrabFocus();
			}
			else if (@event.IsActionPressed("ui_left"))
			{
				if (currentButtonIndex - 4 >= 0)
				{
					currentButtonIndex = (currentButtonIndex - 4 + buttons.Length) % buttons.Length;
					buttons[currentButtonIndex].GrabFocus();
				}
			}
			else if (@event.IsActionPressed("ui_right"))
			{
				if (currentButtonIndex + 4 < buttons.Length)
				{
					currentButtonIndex = (currentButtonIndex + 4) % buttons.Length;
					buttons[currentButtonIndex].GrabFocus();
				}
			}
			else if (@event.IsActionPressed("ui_accept"))
			{
				buttons[currentButtonIndex].EmitSignal("pressed");
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
		currentButtonIndex = 0;
		buttons[currentButtonIndex].GrabFocus();
		GetNode<Control>("PortMenuPanel").Position = position;
	}

	public void HideMenu()
	{
		Visible = false;
	}
	
	#endregion
}
