using Godot;
using System;

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
	
	private FactoryMenu _FactoryMenuInstance;
	private AirportMenu _AirportMenuInstance;
	private PortMenu _PortMenuInstance;
	private Vector2I _TilePosition;
	
	#endregion

	public override void _Ready()
	{
		_FactoryMenuInstance = FactoryMenuScene.Instantiate<FactoryMenu>();
		AddChild(_FactoryMenuInstance);
		_FactoryMenuInstance.HideMenu();
		_FactoryMenuInstance.UnitSelected += OnButtonUnitSelected;
		
		_AirportMenuInstance = AirportMenuScene.Instantiate<AirportMenu>();
		AddChild(_AirportMenuInstance);
		_AirportMenuInstance.HideMenu();
		_AirportMenuInstance.UnitSelected += OnButtonUnitSelected;
		
		_PortMenuInstance = PortMenuScene.Instantiate<PortMenu>();
		AddChild(_PortMenuInstance);
		_PortMenuInstance.HideMenu();
		_PortMenuInstance.UnitSelected += OnButtonUnitSelected;
	}
	
	#region Unit Creation

	public void ShowUnitCreationMenu(Vector2I tilePosition, string buildingType)
	{
		Vector2 mapCenter = GetMapCenter();
		_TilePosition = tilePosition;

		switch (buildingType)
		{
			case "factory":
				_FactoryMenuInstance.ShowMenu(new Vector2(mapCenter.X - 96, mapCenter.Y - 48));
				break;
			case "port":    
				_PortMenuInstance.ShowMenu(new Vector2(mapCenter.X - 48, mapCenter.Y - 36));
				break;
			case "airport":
				_AirportMenuInstance.ShowMenu(new Vector2(mapCenter.X - 48, mapCenter.Y - 36));
				break;
		}
	}
	
	private void OnButtonUnitSelected(string unitType)
	{
		GD.Print("In OnUnitSelected()");
		UnitManagerInstance.CreateUnit(unitType, _TilePosition);
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
