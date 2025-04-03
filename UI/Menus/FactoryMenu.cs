using Godot;
using System;

public partial class FactoryMenu : CanvasLayer
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
			GetNode<Button>("FactoryMenuPanel/InfantryButton"),
			GetNode<Button>("FactoryMenuPanel/AAButton")
		};
		
		// Set unit prices in the buttons labels
		GetNode<Label>("FactoryMenuPanel/InfantryButton/InfantryContainer/InfantryCost").Text = InfantryUnit.BasePrice.ToString();
		GetNode<Label>("FactoryMenuPanel/AAButton/AAContainer/AACost").Text = AAUnit.BasePrice.ToString();

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
			else if (@event.IsActionPressed("ui_accept"))
			{
				buttons[currentButtonIndex].EmitSignal("pressed");
			}
			GetViewport().SetInputAsHandled();
		}
	}

	public void OnInfantryButtonPressed()
	{
		GD.Print("Infantry button pressed");
		EmitSignal(nameof(UnitSelected), "Infantry");
		HideMenu();
	}
	
	public void OnAAButtonPressed()
	{
		GD.Print("AA button pressed");
		EmitSignal(nameof(UnitSelected), "AA");
		HideMenu();
	}

	public void ShowMenu(Vector2 position)
	{
		Visible = true;
		currentButtonIndex = 0;
		buttons[currentButtonIndex].GrabFocus();
		GetNode<Control>("FactoryMenuPanel").Position = position;
	}

	public void HideMenu()
	{
		Visible = false;
	}
}
