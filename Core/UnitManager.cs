using Godot;
using System;
using System.Collections.Generic;

public partial class UnitManager : Node
{
	[Export]
	public TileMapLayer unitsLayer;
	
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
				if (unitInstance is Node2D unitNode2D) // Cast to Node2D
				{
					GD.Print("In Position assignation");
					unitNode2D.GlobalPosition = unitsLayer.MapToLocal(selectedFactoryPosition) + unitsLayer.Position;
					unitList.Add(unitNode2D); 
				}
			}
			else
			{
				GD.PrintErr("Unit scene not found: " + "res://scenes/units/" + unitType + ".tscn");
			}
		}
	}
}
