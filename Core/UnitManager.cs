using Godot;
using System.Collections.Generic;
using System.Linq;
using TinyConflict2D.Commons.Enums;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.Core;

public partial class UnitManager : Node
{
	#region Properties
	
	[Export]
	public TileMapLayer UnitsLayer;
	
	[Export]
	public TileMapLayer TerrainLayer;
	
	[Export]
	public TileMapLayer TerrainFeaturesLayer;
	
	[Export]
	public Players.PlayerManager PlayerManager;
	
	#endregion
	
	#region Fields
	
	public List<Node2D> unitList = new List<Node2D>();
	private AStarGrid2D _aStarGrid;
	private Rect2I _gridRect;

	#endregion
	
	#region Initialization
	
	public override void _Ready()
	{
		InitializeAStarGrid();
	}

	private void InitializeAStarGrid()
	{
		_aStarGrid = new AStarGrid2D();
		_aStarGrid.SetDiagonalMode(AStarGrid2D.DiagonalModeEnum.Never);
		_aStarGrid.Region = _gridRect = TerrainLayer.GetUsedRect();
		_aStarGrid.CellSize = TerrainLayer.TileSet.TileSize;
		_aStarGrid.Update();
		
		// Initialize all points with a default weight (1)
		for (int y = 0; y < _gridRect.Size.Y; y++)
		{
			for (int x = 0; x < _gridRect.Size.X; x++)
			{
				_aStarGrid.SetPointWeightScale(new Vector2I(x, y), 1);
			}
		}
		
		// Apply terrain-specific weights
		UpdateTerrainWeights();
		_aStarGrid.Update();
	}
	
	private void UpdateTerrainWeights()
	{
		Vector2I tilePosition;
		TileData featureTileData;
		TileData terrainTileData;
			
		for (int y = 0; y < _gridRect.Size.Y; y++)
		{
			for (int x = 0; x < _gridRect.Size.X; x++)
			{
				tilePosition = new Vector2I(x, y);
				featureTileData = TerrainFeaturesLayer.GetCellTileData(tilePosition);

				if (featureTileData != null && featureTileData.HasCustomData(Config.MOVEMENTCOST_CUSTOMDATA))
				{
					var cost = (int)featureTileData.GetCustomData(Config.MOVEMENTCOST_CUSTOMDATA);
					_aStarGrid.SetPointWeightScale(tilePosition, cost);
				}
				else	// No features, look for the terrain tile movement cost
				{
					terrainTileData = TerrainLayer.GetCellTileData(tilePosition);
					
					if (terrainTileData != null && terrainTileData.HasCustomData(Config.MOVEMENTCOST_CUSTOMDATA))
					{
						var cost = (int)terrainTileData.GetCustomData(Config.MOVEMENTCOST_CUSTOMDATA);
						_aStarGrid.SetPointWeightScale(tilePosition, cost);
					}
				}
			}
		}
	}
	
	#endregion
	
	#region Unit Creation
	
	public void CreateUnit(string unitType, Vector2I selectedFactoryPosition)
	{
		if (selectedFactoryPosition != new Vector2I(-1, -1))
		{
			var unitScene = GD.Load<PackedScene>("res://Units/" + unitType + "Unit.tscn");
			if (unitScene != null)
			{
				var unitInstance = unitScene.Instantiate();
				UnitsLayer.AddChild(unitInstance);
				if (unitInstance is Node2D unitNode2D && unitInstance is Unit unit)
				{
					GD.Print("In Position assignation");
					unitNode2D.GlobalPosition = UnitsLayer.MapToLocal(selectedFactoryPosition) + UnitsLayer.Position;
					
					// Unit properties initialisation
					unit.PlayerColor = PlayerManager.CurrentPlayer.PlayerColor;
					unit.UnitOwner = PlayerManager.CurrentPlayer;
					unit.TilePosition =  selectedFactoryPosition;
					
					unitList.Add(unitNode2D); 
					PlayerManager.CurrentPlayer.AddUnit(unit);
					
					// Change the sprite of the unit according to its color
					Sprite2D unitSprite = unitInstance.GetNode<Sprite2D>(unitType + "Sprite");
					if (unitSprite != null)
					{
						GD.Print(Config.UNIT_SPRITES_PATH + unitType + GetColorName(unit.PlayerColor) + ".png");
						unitSprite.Texture = GD.Load<Texture2D>(Config.UNIT_SPRITES_PATH + unitType + GetColorName(unit.PlayerColor) + ".png");
					}
				}
			}
			else
			{
				GD.PrintErr("Unit scene not found: " + "res://scenes/units/" + unitType + ".tscn");
			}
		}
	}	
	
	public void RemoveUnit(Node2D unit)
	{
		unitList.Remove(unit);
		UnitsLayer.RemoveChild(unit);
		unit.QueueFree();
	}
	
	#endregion
	
	#region Unit Selection
	
	public Unit GetUnitAt(Vector2I tilePosition)
	{
		return PlayerManager.CurrentPlayer.Units.Find(u => u.TilePosition == tilePosition);
	}
	
	#endregion
	
	#region Unit Movement

	public void UpdateTerrainWeightsByMovementType(UnitMovementType  movementType)
	{
		// Default values are : Plains 1, Forests 2, Mountains 999 (impassable)
		int plainCost = 1;
		int forestCost = 2;
		int mountainCost = 999;
		int roadCost = 1;
		int waterCost = 999;

		switch (movementType)
		{
			case UnitMovementType.Infantry:
				mountainCost = Config.INFANTRY_MOUNTAIN_COST;
				forestCost = Config.INFANTRY_FOREST_COST;
				break;
			case UnitMovementType.Mech:
				mountainCost = Config.MECH_MOUNTAIN_COST;
				forestCost = Config.MECH_FOREST_COST;
				break;
			case UnitMovementType.TireA:
				plainCost = Config.TIREA_PLAIN_COST;
				forestCost = Config.TIREA_FOREST_COST;
				break;
			case UnitMovementType.TireB:
				plainCost = Config.TIREB_PLAIN_COST;
				forestCost = Config.TIREB_FOREST_COST;
				break;
			case UnitMovementType.Treads:
				break;
			case UnitMovementType.Airborne:
				forestCost = Config.AIRBORNE_FOREST_COST;
				mountainCost = Config.AIRBORNE_MOUNTAIN_COST;
				waterCost = Config.AIRBORNE_WATER_COST;
				break;
			case UnitMovementType.Naval:
				plainCost = Config.NAVAL_PLAIN_COST;
				forestCost = Config.NAVAL_FOREST_COST;
				roadCost = Config.NAVAL_ROAD_COST;
				waterCost = Config.NAVAL_WATER_COST;
				break;
			case UnitMovementType.Lander:
				plainCost = Config.LANDER_PLAIN_COST;
				forestCost = Config.LANDER_FOREST_COST;
				roadCost = Config.LANDER_PLAIN_COST;
				waterCost = Config.LANDER_WATER_COST;
				// beachCost = 1;	// Will be added in V2
				break;
		}

		ApplyTerrainWeightsModifications(plainCost, forestCost, mountainCost, roadCost, waterCost);
	}

	public void ApplyTerrainWeightsModifications(int plainCost, int forestCost, int mountainCost, int roadCost, int waterCost)
	{
		Vector2I cellPosition;
		TileData featureTileData;
		TileData terrainTileData;
		
		for (int y = 0; y < _gridRect.Size.Y; y++)
		{
			for (int x = 0; x < _gridRect.Size.X; x++)
			{
				cellPosition = new Vector2I(x, y);
				featureTileData = TerrainFeaturesLayer.GetCellTileData(cellPosition);

				if (featureTileData != null && featureTileData.HasCustomData(Config.TERRAINTYPE_CUSTOMDATA))
				{
					string terrainType = (string)featureTileData.GetCustomData(Config.TERRAINTYPE_CUSTOMDATA);
					switch (terrainType)
					{
						case Config.ROAD_TERRAINTYPE:
						case Config.CITY_TERRAINTYPE:
						case Config.HEADQUARTERS_TERRAINTYPE:
						case Config.FACTORY_TERRAINTYPE:
						case Config.PORT_TERRAINTYPE:
						case Config.AIRPORT_TERRAINTYPE:
						case Config.ANTENNA_TERRAINTYPE:
						case Config.SILO_TERRAINTYPE:
						case Config.BRIDGE_TERRAINTYPE:
							ApplyCost(roadCost, cellPosition);
							break;
						case Config.MOUNTAIN_TERRAINTYPE:
							ApplyCost(mountainCost, cellPosition);
							break;
						case Config.FOREST_TERRAINTYPE:
							ApplyCost(forestCost, cellPosition);
							break;
						case Config.WATER_TERRAINTYPE:
							ApplyCost(waterCost, cellPosition);
							break;
					}
				}
				else	// No features, look for the terrain tile movement cost
				{
					terrainTileData = TerrainLayer.GetCellTileData(cellPosition);
					
					if (terrainTileData != null && terrainTileData.HasCustomData(Config.TERRAINTYPE_CUSTOMDATA))
					{
						string terrainType = (string)terrainTileData.GetCustomData(Config.TERRAINTYPE_CUSTOMDATA);
						switch (terrainType)
						{
							case Config.PLAIN_TERRAINTYPE:
								ApplyCost(plainCost, cellPosition);
								break;
							case Config.WATER_TERRAINTYPE:
								ApplyCost(waterCost, cellPosition);
								break;
						}
					}
				}
			}
		}
		
		_aStarGrid.Update();
	}

	public void ApplyCost(int cost, Vector2I cellPosition)
	{
		if (cost != 999)
		{
			_aStarGrid.SetPointSolid(cellPosition, false);
			_aStarGrid.SetPointWeightScale(cellPosition, cost);
		}
		else
		{
			_aStarGrid.SetPointSolid(cellPosition, true);
		}
	}
	
	public List<Vector2I> GetPathBetween(Vector2I startPosition, Vector2I endPosition, int maxMovementPoints)
	{
		List<Vector2I> path = _aStarGrid.GetIdPath(startPosition, endPosition).ToList();
		List<Vector2I> limitedPath = new List<Vector2I>();

		if (path == null || path.Count < 2) return null;
		
		limitedPath.Add(path[0]);
		
		for (int i = 1; i < path.Count; i++)
		{
			maxMovementPoints -= GetTileWeightScale(path[i]);
			if (maxMovementPoints >= 0)
			{
				limitedPath.Add(path[i]);
			}
			else
			{
				break;
			}
		}
		return limitedPath;
	}
	
	#endregion
	
	#region Utils
	
	// Godot Color strangely doesn't have a .Name property ...
	private string GetColorName(Color color)	
	{
		if (color == Colors.Red)
		{
			return "Red";
		}
		else if(color == Colors.Blue){
			return "Blue";
		}
		else if (color == Colors.Green){
			return "Green";
		}
		else if (color == Colors.Orange){
			return "Orange";
		}
		else
		{
			return "Gray";
		}
	}
	
	public int GetTileWeightScale(Vector2I tilePosition)
	{
		return (int)_aStarGrid.GetPointWeightScale(tilePosition);
	}
	
	#endregion
}
