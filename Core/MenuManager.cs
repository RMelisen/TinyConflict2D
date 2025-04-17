using Godot;
using TinyConflict2D.UI.Menus;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Core.Players;

namespace TinyConflict2D.Core;

public partial class MenuManager : Node
{
	#region Properties
	
	[Export] public CoreManager CoreManagerInstance;
	[Export] public UnitManager UnitManagerInstance; 
	[Export] public PlayerManager PlayerManagerInstance; 
	[Export] public PackedScene FactoryMenuScene; 
	[Export] public PackedScene AirportMenuScene; 
	[Export] public PackedScene GameMenuScene; 
	[Export] public PackedScene PortMenuScene;
	
	#endregion
	
	#region Fields
	
	private FactoryMenu _factoryMenuInstance;
	private AirportMenu _airportMenuInstance;
	private PortMenu _portMenuInstance;
	private GameMenu _gameMenuInstance;
	private Vector2I _tilePosition;
	
	#endregion
	
	#region Godot Methods

	public override void _Ready()
	{
		_factoryMenuInstance = FactoryMenuScene.Instantiate<FactoryMenu>();
		AddChild(_factoryMenuInstance);
		_factoryMenuInstance.HideMenu();
		_factoryMenuInstance.UnitSelected += OnButtonUnitSelected;
		
		_airportMenuInstance = AirportMenuScene.Instantiate<AirportMenu>();
		AddChild(_airportMenuInstance);
		_airportMenuInstance.HideMenu();
		_airportMenuInstance.UnitSelected += OnButtonUnitSelected;
		
		_portMenuInstance = PortMenuScene.Instantiate<PortMenu>();
		AddChild(_portMenuInstance);
		_portMenuInstance.HideMenu();
		_portMenuInstance.UnitSelected += OnButtonUnitSelected;
		
		_gameMenuInstance = GameMenuScene.Instantiate<GameMenu>();
		AddChild(_gameMenuInstance);
		_gameMenuInstance.HideMenu();
		_gameMenuInstance.ButtonSelected += OnGameButtonSelected;
	}
	
	#endregion
	
	#region Unit Creation

	public void ShowUnitCreationMenu(Vector2I tilePosition, string buildingType)
	{
		Vector2 mapCenter = CoreManagerInstance.GetMapCenter();
		_tilePosition = tilePosition;

		switch (buildingType)
		{
			case Config.FACTORY_TERRAINTYPE:
				_factoryMenuInstance.ShowMenu(new Vector2(mapCenter.X - 96, mapCenter.Y - 48));
				break;
			case Config.PORT_TERRAINTYPE:    
				_portMenuInstance.ShowMenu(new Vector2(mapCenter.X - 48, mapCenter.Y - 36));
				break;
			case Config.AIRPORT_TERRAINTYPE:
				_airportMenuInstance.ShowMenu(new Vector2(mapCenter.X - 48, mapCenter.Y - 36));
				break;
		}
	}
	
	private void OnButtonUnitSelected(string unitType)
	{
		UnitManagerInstance.CreateUnit(unitType, _tilePosition);
	}

	#endregion
	
	#region Game Menu
	
	public void ShowGameMenu()
	{
		Vector2 mapCenter = CoreManagerInstance.GetMapCenter();
		_gameMenuInstance.ShowMenu(new Vector2(mapCenter.X - 48, mapCenter.Y - 36));
	}
	
	private void OnGameButtonSelected(string selectedGameMenuButton)
	{
		switch (selectedGameMenuButton)
		{
			case Config.GAMEMENU_INFORMATION:
				break;
			case Config.GAMEMENU_SETTINGS:
				break;
			case Config.GAMEMENU_ENDTURN:
				UnitManagerInstance.NextTurn();
				PlayerManagerInstance.NextTurn();
				break;
		}
	}
	
	#endregion
}
