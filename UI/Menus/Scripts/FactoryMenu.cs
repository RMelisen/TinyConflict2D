using System.Linq;
using Godot;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Core.Players;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.UI.Menus;

public partial class FactoryMenu : ProductionBuildingMenu
{
	#region Godot Methods
	
	public override void _Ready()
	{
		// Store buttons
		_buttons = new Button[]
		{
			GetNode<Button>("HBoxContainer/VBoxContainer/InfantryButton"),
			GetNode<Button>("HBoxContainer/VBoxContainer/MechButton"),
			GetNode<Button>("HBoxContainer/VBoxContainer/ReconButton"),
			GetNode<Button>("HBoxContainer/VBoxContainer/AAButton"),
			GetNode<Button>("HBoxContainer/VBoxContainer2/APCButton"),
			GetNode<Button>("HBoxContainer/VBoxContainer2/SupplyButton"),
			GetNode<Button>("HBoxContainer/VBoxContainer2/TankButton")
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
		GetNode<Label>("HBoxContainer/VBoxContainer/InfantryButton/InfantryContainer/InfantryCost").Text = InfantryUnit.BASE_PRICE.ToString();
		GetNode<Label>("HBoxContainer/VBoxContainer/MechButton/MechContainer/MechCost").Text = MechUnit.BASE_PRICE.ToString();
		GetNode<Label>("HBoxContainer/VBoxContainer/ReconButton/ReconContainer/ReconCost").Text = ReconUnit.BASE_PRICE.ToString();
		GetNode<Label>("HBoxContainer/VBoxContainer/AAButton/AAContainer/AACost").Text = AAUnit.BASE_PRICE.ToString();
		GetNode<Label>("HBoxContainer/VBoxContainer2/APCButton/APCContainer/APCCost").Text = APCUnit.BASE_PRICE.ToString();
		GetNode<Label>("HBoxContainer/VBoxContainer2/SupplyButton/SupplyContainer/SupplyCost").Text = SupplyUnit.BASE_PRICE.ToString();
		GetNode<Label>("HBoxContainer/VBoxContainer2/TankButton/TankContainer/TankCost").Text = TankUnit.BASE_PRICE.ToString();
	}
	
	#endregion
}
