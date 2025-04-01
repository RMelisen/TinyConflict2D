using Godot;
using System;

public partial class FactoryMenu : CanvasLayer
{
	[Signal]
	public delegate void UnitSelectedEventHandler(string unitType);

	public override void _Ready()
	{
		// Set unit prices in the buttons labels
		GetNode<Label>("FactoryMenuPanel/InfantryButton/InfantryContainer/InfantryCost").Text = InfantryUnit.BasePrice.ToString();
		// GetNode<Label>("Panel/TankButton/HBoxContainer/TankPrice").Text = TankUnit.BasePrice.ToString();

		
	}

	public void OnInfantryButtonPressed()
	{
		EmitSignal(nameof(UnitSelected), "Infantry");
		Hide();
	}

	public void ShowMenu(Vector2 position)
	{
		Visible = true;
		GetNode<Control>("FactoryMenuPanel").Position = position;
	}

	public void Hide()
	{
		Visible = false;
	}
}
