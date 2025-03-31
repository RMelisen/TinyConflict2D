using Godot;
using System;

public partial class CursorController : Sprite2D
{
	[Export]
	public int tileSize = 16;
	
	[Export]
	public TileMapLayer terrainLayer;
	
	[Export]
	public TileMapLayer terrainFeaturesLayer;
	
	private Vector2I gridPosition = Vector2I.Zero;	
	private Vector2I mapSize;

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
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_right") || Input.IsActionJustPressed("ui_left") || Input.IsActionJustPressed("ui_down") || Input.IsActionJustPressed("ui_up"))
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

		// Check feature layer first
		TileData featureTileData = terrainFeaturesLayer.GetCellTileData(gridPosition);

		if (featureTileData != null && featureTileData.HasCustomData("TerrainType"))
		{
			GD.Print("Feature Tile terrainType is : " + featureTileData.GetCustomData("TerrainType"));
			return;
		}

		//// If no relevant feature, check terrain layer
		TileData terrainTileData = terrainLayer.GetCellTileData(gridPosition);

		if (terrainTileData != null && terrainTileData.HasCustomData("TerrainType"))
		{
			GD.Print("Terrain Tile terrainType is : " + terrainTileData.GetCustomData("TerrainType"));
			return;
		}
	}

	#endregion
}
