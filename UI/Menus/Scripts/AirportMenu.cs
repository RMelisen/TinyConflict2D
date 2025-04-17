using Godot;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Core.Players;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.UI.Menus;

public partial class AirportMenu : ProductionBuildingMenu
{
	#region Properties
	
	public override string MenuPanelType { get; } = Config.AIRPORT_MENU_PANEL;
	
	#endregion
	
	#region Godot Methods
	
	public override void _Ready()
	{
		// Store buttons
		_buttons = new Button[]
		{
			GetNode<Button>("AirportMenuPanel/BCopterButton"),
			GetNode<Button>("AirportMenuPanel/TCopterButton"),
			GetNode<Button>("AirportMenuPanel/BomberButton")
		};
		
		// Store unit prices
		_unitPrices = new int[]
		{
			BCopterUnit.BASE_PRICE,
			TCopterUnit.BASE_PRICE,
			BomberUnit.BASE_PRICE
		};
		
		// Set unit prices in the buttons labels
		GetNode<Label>("AirportMenuPanel/BCopterButton/BCopterContainer/BCopterCost").Text = BCopterUnit.BASE_PRICE.ToString();
		GetNode<Label>("AirportMenuPanel/TCopterButton/TCopterContainer/TCopterCost").Text = TCopterUnit.BASE_PRICE.ToString();
		GetNode<Label>("AirportMenuPanel/BomberButton/BomberContainer/BomberCost").Text = BomberUnit.BASE_PRICE.ToString();

		_buttons[_currentButtonIndex].GrabFocus();
	}
	
	#endregion
}
