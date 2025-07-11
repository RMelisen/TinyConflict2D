using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TinyConflict2D.Commons.Enums;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Commons.Interfaces;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.Core;

public partial class UnitManager : Node
{
	#region Properties
	
	[Export] public CoreManager CoreManagerInstance;
	[Export] public TileMapLayer UnitsLayer;
	[Export] public TileMapLayer TerrainLayer;
	[Export] public TileMapLayer TerrainFeaturesLayer;
	[Export] public Players.PlayerManager PlayerManager;
	
	#endregion
	
	#region Fields
	
	public List<Node2D> unitNodeList = new List<Node2D>();
	public List<Unit> unitsList = new List<Unit>();
	private AStarGrid2D _aStarGrid;
	private Rect2I _gridRect;
	private Vector2I _mapSize;

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
		
		_mapSize = _gridRect.Size;
		
		// Initialize all points with a default weight (1)
		for (int y = 0; y < _mapSize.Y; y++)
		{
			for (int x = 0; x < _mapSize.X; x++)
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
			
		for (int y = 0; y < _mapSize.Y; y++)
		{
			for (int x = 0; x < _mapSize.X; x++)
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
					
					unitNodeList.Add(unitNode2D); 
					PlayerManager.CurrentPlayer.AddUnit(unit);
					unitsList.Add(unit);
					
					PlayerManager.CurrentPlayer.Money -= unit.BasePrice;
					
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
		unitNodeList.Remove(unit);
		UnitsLayer.RemoveChild(unit);
		unit.QueueFree();
	}
	
	#endregion
	
	#region Unit Selection
	
	public Unit GetAllyUnitAt(Vector2I tilePosition)
	{
		return PlayerManager.CurrentPlayer.Units.Find(u => u.TilePosition == tilePosition);
	}

	public Unit GetEnemyUnitAt(Vector2I tilePosition)
	{
		foreach (var player in PlayerManager.Players.Where(p => p != PlayerManager.CurrentPlayer))
		{
			Unit unitFound = player.Units.Find(u => u.TilePosition == tilePosition);
			if (unitFound != null)
			{
				return unitFound;
			}
		}

		return null;
	}
	
	#endregion
	
	#region Unit Movement

	public void UpdateTerrainWeightsByMovementType(UnitMovementType  movementType)
	{
		// Default values are : Plains 1, Forests 2, Mountains 999 (impassable)
		int plainCost = Config.DEFAULT_PLAIN_COST;
		int forestCost = Config.DEFAULT_FOREST_COST;
		int mountainCost = Config.DEFAULT_MOUNTAIN_COST;
		int roadCost = Config.DEFAULT_ROAD_COST;
		int waterCost = Config.DEFAULT_WATER_COST;
		int enemyOccupiedCost = Config.DEFAULT_ENEMY_OCCUPIED_COST;

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

		ApplyTerrainWeightsModifications(plainCost, forestCost, mountainCost, roadCost, waterCost, enemyOccupiedCost);
	}

	public void ApplyTerrainWeightsModifications(int plainCost, int forestCost, int mountainCost, int roadCost, int waterCost, int enemyOccupiedCost)
	{
		Vector2I cellPosition;
		TileData featureTileData;
		TileData terrainTileData;
		
		for (int y = 0; y < _mapSize.Y; y++)
		{
			for (int x = 0; x < _mapSize.X; x++)
			{
				cellPosition = new Vector2I(x, y);
				
				// First check if the tile is occupied by an enemy
				Unit unitFound = GetEnemyUnitAt(cellPosition);
				if (unitFound != null)
				{
					ApplyCost(enemyOccupiedCost, cellPosition);	
					continue;
				}
				
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

		if (endPosition == limitedPath.Last())
			return limitedPath;
		
		// Destination tile not reachable
		return null;
	}
	
	#region Reachable Tiles Highlighting
	
	// I use Dijkstra algorithm to get all reachable tiles. (Better than Breadth-First search because tile cost is not always 1)
	public HashSet<Vector2I> GetReachableTiles(Unit selectedUnit)
	{
		// Stores the minimum cost found so far to reach each tile.
		Dictionary<Vector2I, int> costSoFar = new Dictionary<Vector2I, int>();
		PriorityQueue<Vector2I, int> queue = new PriorityQueue<Vector2I, int>(); // (cell, cost)
		Vector2I startCell = selectedUnit.TilePosition;

		queue.Enqueue(startCell, 0);	// Because cost to reach the starting cell is 0
		costSoFar[startCell] = 0;

		while (queue.TryDequeue(out Vector2I currentCell, out int currentCost))
		{
			// If cell cost is higher than a cost I've already recorded, I can skip it
			if (costSoFar.ContainsKey(currentCell) && currentCost > costSoFar[currentCell])
				continue; // Already found a cheaper path to this cell

			Vector2I[] neighbors = GetNeighbors(currentCell);

			foreach (Vector2I neighbor in neighbors)
			{
				Unit enemyUnitFound = GetEnemyUnitAt(neighbor);
				if (enemyUnitFound != null)
					continue; // Cannot move on a tile if an enemy occupy it
				
				int moveCost = GetTileWeightScale(neighbor);
				int newCost = currentCost + moveCost;
				
				if (newCost <= selectedUnit.MovementPointsLeft)
				{
					if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
					{
						// Record the new or cheaper cost for this tile
						costSoFar[neighbor] = newCost;
						// Enqueue the tile to explore from it later
						queue.Enqueue(neighbor, newCost);
					}
				}
			}
		}
		
		// Keys are reachable tiles
		return new HashSet<Vector2I>(costSoFar.Keys);
	}
	
	#endregion
	
	#endregion
	
	#region Unit Combat

	public void Attack(Unit attackerUnit, Unit defenderUnit)
	{
		// Damage calculation formula is : Damage = (B*Av/100 + L) * (HPa/10) * ((100 - (Dtr*HPd))/100)
		// Where :
		// B = Base damage against that unit (from CombatData)
		// Av = 100 + 10 for each owned antenna
		// L = Luck damage, a random number between 0 and 9
		// HPa = HP of attacker divided by 10 (1 to 10)
		// Dtr = Defending terrain level
		// HPD = HP of defender (1 to 100)
		
		// Need to test this formula
		
		Random dice = new Random();

		int numberOfAntenna = 0;
		int luckDamage = dice.Next(0,9);
		int terrainDefenseLevel = 0;
		int baseDamage = CombatData.GetBaseDamagePercentage(attackerUnit.UnitType, defenderUnit.UnitType);

		int damage = (baseDamage * (100 + numberOfAntenna) / 100 + luckDamage) * (attackerUnit.CurrentHealth / 10) * ((100 - terrainDefenseLevel * defenderUnit.CurrentHealth) / 100);
		
		defenderUnit.CurrentHealth -= damage;
		if (defenderUnit.CurrentHealth <= 0)
		{
			defenderUnit.UnitOwner.RemoveUnit(defenderUnit);
		}
	}
	
	public List<Unit> GetInRangeAllyUnits(Unit selectedUnit)
	{
		List<Unit> inRangeUnitList = new List<Unit>();
		Vector2I[] neighbors = GetNeighbors(selectedUnit.TilePosition);
		foreach (Vector2I neighbor in neighbors)
		{
			Unit unitFound = GetAllyUnitAt(neighbor);
			if  (unitFound != null)
				inRangeUnitList.Add(unitFound);
		}

		return inRangeUnitList;
	}

	public List<Unit> GetInRangeEnemyUnits(Unit selectedUnit)
	{
		List<Unit> inRangeUnitList = new List<Unit>();
		Vector2I[] neighbors = GetNeighbors(selectedUnit.TilePosition);
		foreach (Vector2I neighbor in neighbors)
		{
			Unit unitFound = GetEnemyUnitAt(neighbor);
			if (unitFound != null)
				inRangeUnitList.Add(unitFound);
		}

		return inRangeUnitList;
	}

	#endregion
	
	#region Capture

	public void CaptureProcess(ICanCapture capturingUnit, Vector2I tilePosition)
	{
		capturingUnit.CaptureBuilding();
		if (capturingUnit.GetCaptureProgress() >= Config.BUILDING_CAPTURE_TRESHOLD)
		{
			CoreManagerInstance.TerrainFeatureLayerSetCell(tilePosition, GetCorrespondingAtlasCoord(tilePosition));
			capturingUnit.CancelCapture();
		}
	}
	
	#endregion

	#region Turn Management

	public void NextTurn()
	{
		foreach (Unit playerUnit in PlayerManager.CurrentPlayer.Units)
		{
			playerUnit.ResetMovementPoints();
		}
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
	
	private Vector2I[] GetNeighbors(Vector2I cell)
	{
		List<Vector2I> neighbors = new List<Vector2I>();
		int x = cell.X;
		int y = cell.Y;

		neighbors.Add(new Vector2I(x + 1, y));
		neighbors.Add(new Vector2I(x - 1, y));
		neighbors.Add(new Vector2I(x, y + 1));
		neighbors.Add(new Vector2I(x, y - 1));

		return neighbors.Where(n => IsWithinBounds(n) && !_aStarGrid.IsPointSolid(n)).ToArray();
	}
	
	public int GetTileWeightScale(Vector2I tilePosition)
	{
		return (int)_aStarGrid.GetPointWeightScale(tilePosition);
	}
	
	private bool IsWithinBounds(Vector2I position)
	{
		return position.X >= 0 && position.X < _mapSize.X && position.Y >= 0 && position.Y < _mapSize.Y;
	}

	private Vector2I GetCorrespondingAtlasCoord(Vector2I tilePosition)
	{
		int xCoord = 5;	// Depends on building type
		int yCoord = 1;	// Depends on player Color

		switch (PlayerManager.CurrentPlayer.PlayerNumber)
		{
			case Config.RED_PLAYER_NUMBER:
				yCoord = 3;
				break;
			case Config.BLUE_PLAYER_NUMBER:
				yCoord = 2;
				break;
			case Config.GREEN_PLAYER_NUMBER:
				yCoord = 1;
				break;
			case Config.ORANGE_PLAYER_NUMBER:
				yCoord = 4;
				break;
		}
		
		Variant? featureTerrainType = CoreManagerInstance.GetFeatureByCustomData(Config.TERRAINTYPE_CUSTOMDATA, tilePosition);
		if (featureTerrainType.HasValue)
		{
			switch (featureTerrainType.Value.ToString())
			{
				case Config.CITY_TERRAINTYPE:
					xCoord = 8;
					break;
				case Config.FACTORY_TERRAINTYPE:
					xCoord = 11;
					break;
				case Config.PORT_TERRAINTYPE:
					xCoord = 12;
					break;
				case Config.AIRPORT_TERRAINTYPE:
					xCoord = 15;
					break;
				case Config.ANTENNA_TERRAINTYPE:
					xCoord = 13;
					break;
				case Config.SILO_TERRAINTYPE:
					xCoord = 14;
					break;
				case Config.HEADQUARTERS_TERRAINTYPE:
					xCoord = 10;
					break;
			}
		}
		else
		{
			// Default value if nothing was found (error)
			xCoord = 5;
			yCoord = 1;
		}

		return new Vector2I(xCoord, yCoord);
	}
	
	#endregion
}
