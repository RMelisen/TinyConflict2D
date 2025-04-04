using Godot;
using System;

public partial class CursorController : Sprite2D
{
	#region Exports
	
	[Export]
	public int tileSize = 16;
	
	[Export]
	public TileMapLayer terrainLayer;
	
	[Export]
	public TileMapLayer terrainFeaturesLayer;
	
	[Export]
	public UnitManager unitManager;
	
	[Export]
	public PlayerManager playerManager;
	
	[Export]
	public PackedScene factoryMenuScene;
	
	[Export]
	public PackedScene airportMenuScene;
	
	[Export]
	public PackedScene portMenuScene;
	
	#endregion
	
	#region Properties
	
	private FactoryMenu factoryMenuInstance;
	private AirportMenu airportMenuInstance;
	private PortMenu portMenuInstance;
	
	private Vector2I gridPosition = Vector2I.Zero;	
	private Vector2I mapSize;
		
	private Unit selectedUnit = null;
	private bool isUnitSelected = false;

	#endregion
	
	#region Core
	
	public override void _Ready()
	{
		if (terrainLayer != null)
		{
			mapSize = terrainLayer.GetUsedRect().Size;
			gridPosition = terrainLayer.LocalToMap(Position);
		}
		else
		{
			GD.PrintErr("terrainLayer not assigned!");
		}
		
		factoryMenuInstance = factoryMenuScene.Instantiate<FactoryMenu>();
		AddChild(factoryMenuInstance);
		factoryMenuInstance.HideMenu();
		factoryMenuInstance.UnitSelected += OnButtonUnitSelected;
		
		airportMenuInstance = airportMenuScene.Instantiate<AirportMenu>();
		AddChild(airportMenuInstance);
		airportMenuInstance.HideMenu();
		airportMenuInstance.UnitSelected += OnButtonUnitSelected;
		
		portMenuInstance = portMenuScene.Instantiate<PortMenu>();
		AddChild(portMenuInstance);
		portMenuInstance.HideMenu();
		portMenuInstance.UnitSelected += OnButtonUnitSelected;
	}

	public override void _Input(InputEvent @event)
	{
		if (!isUnitSelected)
		{
			if (Input.IsActionJustPressed("ui_right") || Input.IsActionJustPressed("ui_left") ||
				Input.IsActionJustPressed("ui_down") || Input.IsActionJustPressed("ui_up"))
			{
				MoveCursor();
			}
			else
			{
				if (Input.IsActionJustPressed("ui_select"))
				{
					OnCursorSelect();
				}
			}
		}
		else
		{
			// If unit is selected, don't move cursor yet
			if (Input.IsActionJustPressed("ui_select"))
			{
				OnCursorSelect();
			}
		}
	}
	
	#endregion
	
	#region Movements
	
	private void MoveCursor()
	{
		Vector2I newGridPosition = gridPosition;
			
		if (Input.IsActionJustPressed("ui_right"))
		{
			newGridPosition.X++;
		}
		if (Input.IsActionJustPressed("ui_left"))
		{
			newGridPosition.X--;
		}
		if (Input.IsActionJustPressed("ui_down"))
		{
			newGridPosition.Y++;
		}
		if (Input.IsActionJustPressed("ui_up"))
		{
			newGridPosition.Y--;
		}
		
		// Check if the new position is within the map bounds
		if (IsWithinBounds(newGridPosition))
		{
			gridPosition = newGridPosition;
		}

		Position = gridPosition * tileSize;
	}
	
	private bool IsWithinBounds(Vector2I position)
	{
		return position.X >= 0 && position.X < mapSize.X && position.Y >= 0 && position.Y < mapSize.Y;
	}

	#endregion

	#region Selection

	private void OnCursorSelect()
	{
		gridPosition = terrainLayer.LocalToMap(Position);
		
		if (!isUnitSelected)
		{
			// Check if a unit is found first
			Unit unitFound = unitManager.GetUnitAt(gridPosition);
			if (unitFound != null)
			{
				// Unit found
				GD.Print("Selected unit: " + unitFound.GetType());
				SelectUnit(unitFound);
				return;
			}
		}
		else
		{
			DeselectUnit();
			return;
		}

		// Check feature layer second
		TileData featureTileData = terrainFeaturesLayer.GetCellTileData(gridPosition);

		if (featureTileData != null && featureTileData.HasCustomData("TerrainType"))
		{
			GD.Print("Feature Tile terrainType is : " + featureTileData.GetCustomData("TerrainType"));
			
			// Check feature type
			switch (featureTileData.GetCustomData("TerrainType").AsString())
			{
				case "factory":
					if (CheckIfIsOwner(featureTileData))
					{
						ShowFactoryMenu();
					}
					break;
				case "airport":
					if (CheckIfIsOwner(featureTileData))
					{
						ShowAirportMenu();
					}
					break;
				case "port":
					if (CheckIfIsOwner(featureTileData))
					{
						ShowPortMenu();
					}
					break;
			}
			
			// If a terrain feature has been found, no need to look for terrain (for now) 
			return;
		}

		// If no relevant feature, check terrain layer
		TileData terrainTileData = terrainLayer.GetCellTileData(gridPosition);

		if (terrainTileData != null && terrainTileData.HasCustomData("TerrainType"))
		{
			GD.Print("Terrain Tile terrainType is : " + terrainTileData.GetCustomData("TerrainType"));
			return;
		}
	}

	#region Unit Selection
	
	public void SelectUnit(Unit unit)
	{
		selectedUnit = unit;
		isUnitSelected = true;
		ApplySelectionEffects();
	}

	public void DeselectUnit()
	{
		isUnitSelected = false;
		ApplySelectionEffects();
		selectedUnit = null;
	}
	
	private void ApplySelectionEffects()
	{
		if (isUnitSelected)
		{
			selectedUnit.Scale = Vector2.One * 1.15f; // Grow selected unit by 15% to show selection
			Visible = false;
		}
		else
		{
			selectedUnit.Scale = Vector2.One;
			Visible = true;
		}
	}
	
	#endregion
	
	#region Button Selection
	
	private void OnButtonUnitSelected(string unitType)
	{
		GD.Print("In OnUnitSelected()");

		Vector2I selectedFactoryPosition = new Vector2I(-1, -1);
		selectedFactoryPosition = terrainLayer.LocalToMap(Position);

		unitManager.CreateUnit(unitType, selectedFactoryPosition);
	}
	
	#endregion

	#endregion

	#region Unit Creation

	private void ShowFactoryMenu()
	{
		Vector2 mapCenter = GetMapCenter();
		factoryMenuInstance.ShowMenu(new Vector2(mapCenter.X - 96, mapCenter.Y - 48));
	}
	
	private void ShowAirportMenu()
	{
		Vector2 mapCenter = GetMapCenter();
		airportMenuInstance.ShowMenu(new Vector2(mapCenter.X - 48, mapCenter.Y - 48));
	}
	
	private void ShowPortMenu()
	{
		Vector2 mapCenter = GetMapCenter();
		portMenuInstance.ShowMenu(new Vector2(mapCenter.X - 48, mapCenter.Y - 36));
	}

	#endregion
	
	#region Utils

	public bool CheckIfIsOwner(TileData featureTileData)
	{
		if(featureTileData.HasCustomData("PropertyOwner") && featureTileData.GetCustomData("PropertyOwner").AsInt32() == playerManager.CurrentPlayerIndex)
		{
			GD.Print("Property is owned by current player");
			return true;
		}
		else
		{
			GD.Print("Property is not owned by current player");
			return false;
		}
	}
	
	public Vector2 GetMapCenter()
	{
		Vector2 mapCenter = terrainLayer.GetUsedRect().Size * terrainLayer.TileSet.TileSize;;
		return mapCenter / 2;
	}
	
	#endregion
}
