using Godot;
using System.Collections.Generic;
using System.Linq;
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
	public Players.PlayerManager PlayerManager;
	
	#endregion
	
	#region Fields
	
	public List<Node2D> unitList = new List<Node2D>();
	private AStarGrid2D aStarGrid;

	#endregion
	
	public override void _Ready()
	{
		aStarGrid = new AStarGrid2D();
		aStarGrid.Region = TerrainLayer.GetUsedRect();
		aStarGrid.CellSize = TerrainLayer.TileSet.TileSize;
		aStarGrid.Update();
		
		// This line to set solid point (obstacle)
		//aStarGrid.SetPointSolid(new Vector2I(5, 7));
	}
	
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
	
	public List<Vector2> GetPathBetween(Vector2I startPosition, Vector2I endPosition)
	{
		return aStarGrid.GetPointPath(startPosition, endPosition).ToList();
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
