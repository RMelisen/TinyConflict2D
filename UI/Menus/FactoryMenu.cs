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
			GetNode<Button>("FactoryMenuPanel/MechButton"),
			GetNode<Button>("FactoryMenuPanel/ReconButton"),
			GetNode<Button>("FactoryMenuPanel/AAButton"),
			GetNode<Button>("FactoryMenuPanel/APCButton"),
			GetNode<Button>("FactoryMenuPanel/SupplyButton"),
			GetNode<Button>("FactoryMenuPanel/TankButton"),
		};
		
		// Set unit prices in the buttons labels
		GetNode<Label>("FactoryMenuPanel/InfantryButton/InfantryContainer/InfantryCost").Text = InfantryUnit.BasePrice.ToString();
		GetNode<Label>("FactoryMenuPanel/MechButton/MechContainer/MechCost").Text = MechUnit.BasePrice.ToString();
		GetNode<Label>("FactoryMenuPanel/ReconButton/ReconContainer/ReconCost").Text = ReconUnit.BasePrice.ToString();
		GetNode<Label>("FactoryMenuPanel/AAButton/AAContainer/AACost").Text = AAUnit.BasePrice.ToString();
		GetNode<Label>("FactoryMenuPanel/APCButton/APCContainer/APCCost").Text = APCUnit.BasePrice.ToString();
		GetNode<Label>("FactoryMenuPanel/SupplyButton/SupplyContainer/SupplyCost").Text = SupplyUnit.BasePrice.ToString();
		GetNode<Label>("FactoryMenuPanel/TankButton/TankContainer/TankCost").Text = TankUnit.BasePrice.ToString();

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
	
	public void OnAAButtonPressed()
	{
		GD.Print("AA button pressed");
		EmitSignal(nameof(UnitSelected), "AA");
		HideMenu();
	}
	
	public void OnAPCButtonPressed()
	{
		GD.Print("APC button pressed");
		EmitSignal(nameof(UnitSelected), "APC");
		HideMenu();
	}
	
	public void OnInfantryButtonPressed()
	{
		GD.Print("Infantry button pressed");
		EmitSignal(nameof(UnitSelected), "Infantry");
		HideMenu();
	}
	
	public void OnMechButtonPressed()
	{
		GD.Print("Mech button pressed");
		EmitSignal(nameof(UnitSelected), "Mech");
		HideMenu();
	}
	
	public void OnReconButtonPressed()
	{
		GD.Print("Recon button pressed");
		EmitSignal(nameof(UnitSelected), "Recon");
		HideMenu();
	}
	
	public void OnSupplyButtonPressed()
	{
		GD.Print("Supply button pressed");
		EmitSignal(nameof(UnitSelected), "Supply");
		HideMenu();
	}
	
	public void OnTankButtonPressed()
	{
		GD.Print("Tank button pressed");
		EmitSignal(nameof(UnitSelected), "Tank");
		HideMenu();
	}
	
	#endregion
	
	#region Visibility
	
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
	
	#endregion
}
