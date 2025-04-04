using Godot;
using System;

public partial class CursorController : Sprite2D
{
	#region Properties
	
	[Export]
	public int TileSize = 16;
	
	[Export]
	public TileMapLayer TerrainLayer;
	
	[Export]
	public TileMapLayer TerrainFeaturesLayer;
	
	[Export]
	public UnitManager UnitManagerInstance;
	
	[Export]
	public PlayerManager PlayerManagerInstance;
	
	[Export]
	public MenuManager MenuManagerInstance;
	
	#endregion
	
	#region Fields
	
	private Vector2I _GridPosition = Vector2I.Zero;	
	private Vector2I _MapSize;
		
	private Unit _SelectedUnit = null;
	private bool _IsUnitSelected = false;

	#endregion
	
	#region Core
	
	public override void _Ready()
	{
		if (TerrainLayer != null)
		{
			_MapSize = TerrainLayer.GetUsedRect().Size;
			_GridPosition = TerrainLayer.LocalToMap(Position);
		}
		else
		{
			GD.PrintErr("terrainLayer not assigned!");
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (!_IsUnitSelected)
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
		Vector2I newGridPosition = _GridPosition;
			
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
			_GridPosition = newGridPosition;
		}

		Position = _GridPosition * TileSize;
	}
	
	private bool IsWithinBounds(Vector2I position)
	{
		return position.X >= 0 && position.X < _MapSize.X && position.Y >= 0 && position.Y < _MapSize.Y;
	}

	#endregion

	#region Selection

	private void OnCursorSelect()
	{
		_GridPosition = TerrainLayer.LocalToMap(Position);
		
		if (!_IsUnitSelected)
		{
			// Check if a unit is found first
			Unit unitFound = UnitManagerInstance.GetUnitAt(_GridPosition);
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
		TileData featureTileData = TerrainFeaturesLayer.GetCellTileData(_GridPosition);

		if (featureTileData != null && featureTileData.HasCustomData("TerrainType"))
		{
			GD.Print("Feature Tile terrainType is : " + featureTileData.GetCustomData("TerrainType"));
			
			// Check feature type
			switch (featureTileData.GetCustomData("TerrainType").AsString())
			{
				case "factory":
				case "port":
				case "airport":
					if (CheckIfIsOwner(featureTileData))
					{
						MenuManagerInstance.ShowUnitCreationMenu(_GridPosition, featureTileData.GetCustomData("TerrainType").AsString());
					}
					break;
			}
			
			// If a terrain feature has been found, no need to look for terrain (for now) 
			return;
		}

		// If no relevant feature, check terrain layer
		TileData terrainTileData = TerrainLayer.GetCellTileData(_GridPosition);

		if (terrainTileData != null && terrainTileData.HasCustomData("TerrainType"))
		{
			GD.Print("Terrain Tile terrainType is : " + terrainTileData.GetCustomData("TerrainType"));
			return;
		}
	}

	#region Unit Selection
	
	public void SelectUnit(Unit unit)
	{
		_SelectedUnit = unit;
		_IsUnitSelected = true;
		ApplySelectionEffects();
	}

	public void DeselectUnit()
	{
		_IsUnitSelected = false;
		ApplySelectionEffects();
		_SelectedUnit = null;
	}
	
	private void ApplySelectionEffects()
	{
		if (_IsUnitSelected)
		{
			_SelectedUnit.Scale = Vector2.One * 1.15f; // Grow selected unit by 15% to show selection
			Visible = false;
		}
		else
		{
			_SelectedUnit.Scale = Vector2.One;
			Visible = true;
		}
	}
	
	#endregion

	#endregion
	
	#region Utils

	public bool CheckIfIsOwner(TileData featureTileData)
	{
		if(featureTileData.HasCustomData("PropertyOwner") && featureTileData.GetCustomData("PropertyOwner").AsInt32() == PlayerManagerInstance.CurrentPlayerIndex)
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
	
	#endregion
}
