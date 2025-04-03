using Godot;
using System;
using System.Collections.Generic;

public partial class UnitManager : Node
{
	[Export]
	public TileMapLayer unitsLayer;
	
	[Export]
	public PlayerManager playerManager;
	
	public List<Node2D> unitList = new List<Node2D>();
	
	public void CreateUnit(string unitType, Vector2I selectedFactoryPosition)
	{
		if (selectedFactoryPosition != new Vector2I(-1, -1))
		{
			var unitScene = GD.Load<PackedScene>("res://Units/" + unitType + "Unit.tscn");
			if (unitScene != null)
			{
				var unitInstance = unitScene.Instantiate();
				unitsLayer.AddChild(unitInstance);
				if (unitInstance is Node2D unitNode2D && unitInstance is Unit unit)
				{
					GD.Print("In Position assignation");
					unitNode2D.GlobalPosition = unitsLayer.MapToLocal(selectedFactoryPosition) + unitsLayer.Position;
					
					// Unit properties initialisation
					unit.PlayerColor = playerManager.CurrentPlayer.PlayerColor;
					unit.UnitOwner = playerManager.CurrentPlayer;
					unit.TilePosition =  selectedFactoryPosition;
					
					unitList.Add(unitNode2D); 
					playerManager.CurrentPlayer.AddUnit(unit);
					
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
		unitsLayer.RemoveChild(unit);
		unit.QueueFree();
	}
	
	public Unit? GetUnitAt(Vector2I tilePosition)
	{
		return playerManager.CurrentPlayer.Units.Find(u => u.TilePosition == tilePosition);
	}
	
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
}
