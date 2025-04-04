using Godot;
using TinyConflict2D.UI.Menus;

namespace TinyConflict2D.Core;

public partial class MenuManager : Node
{
	#region Properties
	
	[Export]
	public TileMapLayer TerrainLayer;
	
	[Export]
	public UnitManager UnitManagerInstance;
	
	[Export]
	public PackedScene FactoryMenuScene;
	
	[Export]
	public PackedScene AirportMenuScene;
	
	[Export]
	public PackedScene PortMenuScene;
	
	#endregion
	
	#region Fields
	
	private FactoryMenu _factoryMenuInstance;
	private AirportMenu _airportMenuInstance;
	private PortMenu _portMenuInstance;
	private Vector2I _tilePosition;
	
	#endregion

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
	}
	
	#region Unit Creation

	public void ShowUnitCreationMenu(Vector2I tilePosition, string buildingType)
	{
		Vector2 mapCenter = GetMapCenter();
		_tilePosition = tilePosition;

		switch (buildingType)
		{
			case "factory":
				_factoryMenuInstance.ShowMenu(new Vector2(mapCenter.X - 96, mapCenter.Y - 48));
				break;
			case "port":    
				_portMenuInstance.ShowMenu(new Vector2(mapCenter.X - 48, mapCenter.Y - 36));
				break;
			case "airport":
				_airportMenuInstance.ShowMenu(new Vector2(mapCenter.X - 48, mapCenter.Y - 36));
				break;
		}
	}
	
	private void OnButtonUnitSelected(string unitType)
	{
		GD.Print("In OnUnitSelected()");
		UnitManagerInstance.CreateUnit(unitType, _tilePosition);
	}

	#endregion
	
	#region Utils
	
	public Vector2 GetMapCenter()
	{
		Vector2 mapCenter = TerrainLayer.GetUsedRect().Size * TerrainLayer.TileSet.TileSize;;
		return mapCenter / 2;
	}
	
	#endregion
}
