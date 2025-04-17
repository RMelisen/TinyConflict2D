using System.Linq;
using Godot;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Core.Players;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.UI.Menus;

public partial class FactoryMenu : ProductionBuildingMenu
{
	#region Properties
	
	public override string MenuPanelType { get; } = Config.FACTORY_MENU_PANEL;
	
	#endregion
	
	#region Godot Methods
	
	public override void _Ready()
	{
		// Store buttons
		_buttons = new Button[]
		{
			GetNode<Button>("FactoryMenuPanel/InfantryButton"),
			GetNode<Button>("FactoryMenuPanel/MechButton"),
			GetNode<Button>("FactoryMenuPanel/ReconButton"),
			GetNode<Button>("FactoryMenuPanel/AAButton"),
			GetNode<Button>("FactoryMenuPanel/APCButton"),
			GetNode<Button>("FactoryMenuPanel/SupplyButton"),
			GetNode<Button>("FactoryMenuPanel/TankButton")
		};
		
		// Store unit prices
		_unitPrices = new int[]
		{
			InfantryUnit.BASE_PRICE,
			MechUnit.BASE_PRICE,
			ReconUnit.BASE_PRICE,
			AAUnit.BASE_PRICE,
			APCUnit.BASE_PRICE,
			SupplyUnit.BASE_PRICE,
			TankUnit.BASE_PRICE
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
	
	#endregion
}
