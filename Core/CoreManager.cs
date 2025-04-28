using Godot;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Core.Players;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.Core;

public partial class CoreManager : Node
{
	#region Properties
	
	[Export] public TileMapLayer TerrainLayer;
	[Export] public TileMapLayer TerrainFeaturesLayer;
	[Export] public UnitManager UnitManagerInstance;
	[Export] public Players.PlayerManager PlayerManagerInstance;
	[Export] public MenuManager MenuManagerInstance;
	[Export] public UIManager UIManagerInstance;
	
	public Unit SelectedUnit = null;
	public bool IsUnitSelected = false;
	
	#endregion
	
	#region Fields
	
	private Vector2I _mapSize;

	#endregion
	
	#region Godot Methods

	public override void _Ready()
	{
		if (TerrainLayer != null)
		{
			_mapSize = TerrainLayer.GetUsedRect().Size;
		}
		else
		{
			GD.PrintErr("terrainLayer not assigned!");
		}
	}

	#endregion
	
	#region Unit Selection
	
	public void SelectUnit(Unit unit)
	{
		if (unit.MovementPointsLeft > 0)
		{
			SelectedUnit = unit;
			IsUnitSelected = true;
			UnitManagerInstance.UpdateTerrainWeightsByMovementType(unit.MovementType);
            SelectedUnit.Scale = Vector2.One * 1.15f; // Grow selected unit by 15% to show selection
            UIManagerInstance.HighlightReachableTiles(SelectedUnit);
		}
	}

	public void DeselectUnit()
	{
		IsUnitSelected = false;
        SelectedUnit.Scale = Vector2.One;
        SelectedUnit = null;
    }
	
	#endregion
	
	#region Terrain Management
	
	public Variant? GetTerrainByCustomData(string customData, Vector2I position)
	{
		TileData terrainTileData = TerrainLayer.GetCellTileData(position);
		if (terrainTileData != null && terrainTileData.HasCustomData(customData))
		{
			Variant variant = terrainTileData.GetCustomData(customData);
			return variant;
		}
		return null;
	}

	public Variant? GetFeatureByCustomData(string customData, Vector2I position)
	{
		TileData featureTileData = TerrainFeaturesLayer.GetCellTileData(position);
		if (featureTileData != null && featureTileData.HasCustomData(customData))
		{
			Variant variant = featureTileData.GetCustomData(customData);
			return variant;
		}
		return null;
	}
	
	#endregion
	
	#region Utils
	
	#region Map
	
	public bool IsWithinBounds(Vector2I position)
	{
		return position.X >= 0 && position.X < _mapSize.X && position.Y >= 0 && position.Y < _mapSize.Y;
	}

	public Vector2I LocalToMap(Vector2 position)
	{
		return TerrainLayer.LocalToMap(position);
	}
	
	public Vector2 MapToLocal(Vector2I position)
	{
		return TerrainLayer.MapToLocal(position);
	}
	
	public Vector2 GetMapCenter()
	{
		Vector2 mapCenter = TerrainLayer.GetUsedRect().Size * TerrainLayer.TileSet.TileSize;
		return mapCenter / 2;
	}
	
	#endregion
	
	#region Buildings
	
	public bool CheckIfIsOwner(Vector2I position)
	{
		TileData featureTileData = TerrainFeaturesLayer.GetCellTileData(position);
		Variant? ownerId = GetFeatureByCustomData(Config.PROPERTYOWNER_CUSTOMDATA, position);
		if(featureTileData.HasCustomData(Config.PROPERTYOWNER_CUSTOMDATA) && ownerId.HasValue && ownerId.Value.AsInt32() == PlayerManagerInstance.CurrentPlayerIndex)
		{
			return true;
		}
	
		return false;
	}

	public int GetNumberOfOwnedBuildings(Player player)
	{
		int ownedBuildings = 0;
		
		for (int y = 0; y < _mapSize.Y; y++)
		{
			for (int x = 0; x < _mapSize.X; x++)
			{
				Vector2I cellPosition = new Vector2I(x, y);
				Variant? featureTerrainType = GetFeatureByCustomData(Config.TERRAINTYPE_CUSTOMDATA, cellPosition);
				
				if (featureTerrainType.HasValue)
				{
					// Check feature type
					switch (featureTerrainType.Value.ToString())
					{
						case Config.HEADQUARTERS_TERRAINTYPE:
						case Config.CITY_TERRAINTYPE:
						case Config.FACTORY_TERRAINTYPE:
						case Config.PORT_TERRAINTYPE:
						case Config.AIRPORT_TERRAINTYPE:
						case Config.ANTENNA_TERRAINTYPE:
							if (CheckIfIsOwner(cellPosition))
								ownedBuildings++;
							break;
					}
				}
			}
		}
		
		return ownedBuildings;
	}
	
	#endregion
	
	#endregion
}
