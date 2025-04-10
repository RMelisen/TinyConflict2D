using Godot;
using System.Collections.Generic;
using System.Linq;
using TinyConflict2D.Commons.Enums;
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
		Vector2I cellPosition;
		TileData featureTileData;
		TileData terrainTileData;
			
		for (int y = 0; y < _gridRect.Size.Y; y++)
		{
			for (int x = 0; x < _gridRect.Size.X; x++)
			{
				cellPosition = new Vector2I(x, y);
				featureTileData = TerrainFeaturesLayer.GetCellTileData(cellPosition);

				if (featureTileData != null && featureTileData.HasCustomData("MovementCost"))
				{
					var cost = (int)featureTileData.GetCustomData("MovementCost");
					_aStarGrid.SetPointWeightScale(cellPosition, cost);
				}
				else	// No features, look for the terrain tile movement cost
				{
					terrainTileData = TerrainLayer.GetCellTileData(cellPosition);
					
					if (terrainTileData != null && terrainTileData.HasCustomData("MovementCost"))
					{
						var cost = (int)terrainTileData.GetCustomData("MovementCost");
						_aStarGrid.SetPointWeightScale(cellPosition, cost);
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
						GD.Print("res://resources/TinyConflict/Tiles/Units/" + unitType + GetColorName(unit.PlayerColor) + ".png");
						unitSprite.Texture = GD.Load<Texture2D>("res://resources/TinyConflict/Tiles/Units/" + unitType + GetColorName(unit.PlayerColor) + ".png");
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
				mountainCost = 2;
				forestCost = 1;
				break;
			case UnitMovementType.Mech:
				mountainCost = 1;
				forestCost = 1;
				break;
			case UnitMovementType.TireA:
				plainCost = 2;
				forestCost = 3;
				break;
			case UnitMovementType.TireB:
				plainCost = 1;
				forestCost = 3;
				break;
			case UnitMovementType.Treads:
				break;
			case UnitMovementType.Airborne:
				forestCost = 1;
				mountainCost = 1;
				waterCost = 1;
				break;
			case UnitMovementType.Naval:
				plainCost = 999;
				forestCost = 999;
				roadCost = 999;
				waterCost = 1;
				break;
			case UnitMovementType.Lander:
				plainCost = 999;
				forestCost = 999;
				roadCost = 999;
				waterCost = 1;
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

				if (featureTileData != null && featureTileData.HasCustomData("TerrainType"))
				{
					string terrainType = (string)featureTileData.GetCustomData("TerrainType");
					switch (terrainType)
					{
						case "road":
						case "city":
						case "headquarters":
						case "factory":
						case "port":
						case "airport":
						case "antenna":
						case "silo":
						case "bridge":
							ApplyCost(roadCost, cellPosition);
							break;
						case "mountain":
							ApplyCost(mountainCost, cellPosition);
							break;
						case "forest":
							ApplyCost(forestCost, cellPosition);
							break;
						case "water":
							ApplyCost(waterCost, cellPosition);
							break;
					}
				}
				else	// No features, look for the terrain tile movement cost
				{
					terrainTileData = TerrainLayer.GetCellTileData(cellPosition);
					
					if (terrainTileData != null && terrainTileData.HasCustomData("TerrainType"))
					{
						string terrainType = (string)terrainTileData.GetCustomData("TerrainType");
						switch (terrainType)
						{
							case "plain":
								ApplyCost(plainCost, cellPosition);
								break;
							case "water":
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
	
	public List<Vector2I> GetPathBetween(Vector2I startPosition, Vector2I endPosition)
	{
		return _aStarGrid.GetIdPath(startPosition, endPosition).ToList();
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
	
	#endregion
}
