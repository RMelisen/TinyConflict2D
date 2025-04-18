using Godot;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Core.Players;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.UI.Menus;

public partial class PortMenu : ProductionBuildingMenu
{
	#region Godot Methods
	
	public override void _Ready()
	{
		// Store buttons
		_buttons = new Button[]
		{
			GetNode<Button>("VBoxContainer/LanderButton"),
			GetNode<Button>("VBoxContainer/SubmarineButton"),
			GetNode<Button>("VBoxContainer/BattleshipButton")
		};
		
		// Store unit prices
		_unitPrices = new int[]
		{
			LanderUnit.BASE_PRICE,
			SubmarineUnit.BASE_PRICE,
			BattleshipUnit.BASE_PRICE
		};
		
		// Set unit prices in the buttons labels
		GetNode<Label>("VBoxContainer/LanderButton/LanderContainer/LanderCost").Text = LanderUnit.BASE_PRICE.ToString();
		GetNode<Label>("VBoxContainer/SubmarineButton/SubmarineContainer/SubmarineCost").Text = SubmarineUnit.BASE_PRICE.ToString();
		GetNode<Label>("VBoxContainer/BattleshipButton/BattleshipContainer/BattleshipCost").Text = BattleshipUnit.BASE_PRICE.ToString();

		_buttons[_currentButtonIndex].GrabFocus();
	}
	
	#endregion
}
