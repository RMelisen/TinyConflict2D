using Godot;
using TinyConflict2D.UI.Menus;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Core.Players;
using System.Collections.Generic;
using TinyConflict2D.Commons.Interfaces;
using TinyConflict2D.Units.Scripts;

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
	[Export] public PackedScene UnitActionMenuScene; 
	
	#endregion
	
	#region Fields
	
	private FactoryMenu _factoryMenuInstance;
	private AirportMenu _airportMenuInstance;
	private PortMenu _portMenuInstance;
	private GameMenu _gameMenuInstance;
	private UnitActionMenu _unitActionMenuInstance;
	private Vector2I _tilePosition;
	private List<Unit> _inRangeEnemyUnits;
    private List<Unit> _inRangeAllyUnits;

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
		
		_unitActionMenuInstance = UnitActionMenuScene.Instantiate<UnitActionMenu>();
		AddChild(_unitActionMenuInstance);
		_unitActionMenuInstance.HideMenu();
		_unitActionMenuInstance.ButtonSelected += OnUnitActionButtonSelected;
	}
	
	#endregion
	
	#region Unit Creation

	public void ShowUnitCreationMenu(Vector2I tilePosition, string buildingType)
	{
		_tilePosition = tilePosition;

		switch (buildingType)
		{
			case Config.FACTORY_TERRAINTYPE:
				_factoryMenuInstance.ShowMenu(PlayerManagerInstance.CurrentPlayer.Money);
				break;
			case Config.PORT_TERRAINTYPE:    
				_portMenuInstance.ShowMenu(PlayerManagerInstance.CurrentPlayer.Money);
				break;
			case Config.AIRPORT_TERRAINTYPE:
				_airportMenuInstance.ShowMenu(PlayerManagerInstance.CurrentPlayer.Money);
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
		_gameMenuInstance.ShowMenu();
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
	
	#region Unit Action Menu
	
	public void ShowUnitActionMenu(Vector2I tilePosition)
	{
		_inRangeEnemyUnits = UnitManagerInstance.GetInRangeEnemyUnits(CoreManagerInstance.SelectedUnit);		
		bool enableAttackButton = _inRangeEnemyUnits.Count != 0;
		bool enableSupplyButton = false;
        bool enableCaptureButton = false;

        if (CoreManagerInstance.SelectedUnit is APCUnit || CoreManagerInstance.SelectedUnit is SupplyUnit)
		{
            _inRangeAllyUnits = UnitManagerInstance.GetInRangeAllyUnits(CoreManagerInstance.SelectedUnit);
            enableSupplyButton = _inRangeAllyUnits.Count != 0;
		}

        if (CoreManagerInstance.SelectedUnit is ICanCapture)
		{
			// If unit can capture, check if there is a building, and if so, check ownership
			_tilePosition = tilePosition;
			Variant? featureTerrainType = CoreManagerInstance.GetFeatureByCustomData(Config.TERRAINTYPE_CUSTOMDATA, tilePosition);
			if (featureTerrainType.HasValue)
			{
				// Check feature type
				switch (featureTerrainType.Value.ToString())
                {
                    case Config.CITY_TERRAINTYPE:
                    case Config.FACTORY_TERRAINTYPE:
					case Config.PORT_TERRAINTYPE:
					case Config.AIRPORT_TERRAINTYPE:
                    case Config.ANTENNA_TERRAINTYPE:
                    case Config.SILO_TERRAINTYPE:
                    case Config.HEADQUARTERS_TERRAINTYPE:
						enableCaptureButton = !CoreManagerInstance.CheckIfIsOwner(tilePosition);
						break;
                }
			} 
		}        

        _unitActionMenuInstance.ShowMenu(enableAttackButton, enableSupplyButton, enableCaptureButton);
	}
	
	private void OnUnitActionButtonSelected(string selectedUnitAction)
	{
		switch (selectedUnitAction)
		{
			case Config.UNITACTION_WAIT:
				break;
			case Config.UNITACTION_ATTACK:
				break;
			case Config.UNITACTION_SUPPLY:
				break;
			case Config.UNITACTION_CAPTURE:
				break;
		}

        CoreManagerInstance.DeselectUnit();
    }
	
	#endregion
}
