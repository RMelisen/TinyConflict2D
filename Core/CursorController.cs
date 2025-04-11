using Godot;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.UI.Menus;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.Core;

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
	public Players.PlayerManager PlayerManagerInstance;
	
	[Export]
	public MenuManager MenuManagerInstance;
	
	[Export]
	public UIManager UIManagerInstance;
	
	#endregion
	
	#region Fields
	
	private Vector2I _gridPosition = Vector2I.Zero;	
	private Vector2I _mapSize;
		
	private Unit _selectedUnit = null;
	private bool _isUnitSelected = false;

	#endregion
	
	#region Godot Methods
	
	public override void _Ready()
	{
		if (TerrainLayer != null)
		{
			_mapSize = TerrainLayer.GetUsedRect().Size;
			_gridPosition = TerrainLayer.LocalToMap(Position);
		}
		else
		{
			GD.PrintErr("terrainLayer not assigned!");
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_right") || Input.IsActionJustPressed("ui_left") ||
			Input.IsActionJustPressed("ui_down") || Input.IsActionJustPressed("ui_up"))
		{
			MoveCursor();

			if (_isUnitSelected)
			{
				UIManagerInstance.UpdatePathVisualization(UnitManagerInstance.GetPathBetween(_selectedUnit.TilePosition, _gridPosition, _selectedUnit.MovementPointsLeft));
			}
		}
		else
		{
			if (Input.IsActionJustPressed("ui_select"))
			{
				OnCursorSelect();
			}
			else
			{
				if (Input.IsActionJustReleased("ui_cancel"))
				{
					if (_isUnitSelected)
					{
						DeselectUnit();
					}
				}
			}
		}
	}
	
	#endregion
	
	#region Movements
	
	private void MoveCursor()
	{
		Vector2I newGridPosition = _gridPosition;
			
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
			_gridPosition = newGridPosition;
		}

		Position = _gridPosition * TileSize;
	}
	
	private bool IsWithinBounds(Vector2I position)
	{
		return position.X >= 0 && position.X < _mapSize.X && position.Y >= 0 && position.Y < _mapSize.Y;
	}

	#endregion

	#region Selection

	private void OnCursorSelect()
	{
		_gridPosition = TerrainLayer.LocalToMap(Position);
		
		if (!_isUnitSelected)
		{
			// Check if a unit is found first
			Unit unitFound = UnitManagerInstance.GetUnitAt(_gridPosition);
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
			if (_gridPosition == _selectedUnit.TilePosition)
			{
				// If selected unit is selected again
				DeselectUnit();
			}
			else
			{
				_selectedUnit.Move(UnitManagerInstance.GetPathBetween(_selectedUnit.TilePosition, _gridPosition, _selectedUnit.MovementPointsLeft));
				DeselectUnit();
			}
			return;
		}

		// Check feature layer second
		TileData featureTileData = TerrainFeaturesLayer.GetCellTileData(_gridPosition);

		if (featureTileData != null && featureTileData.HasCustomData(Config.TERRAINTYPE_CUSTOMDATA))
		{
			GD.Print("Feature Tile terrainType is : " + featureTileData.GetCustomData(Config.TERRAINTYPE_CUSTOMDATA));
			
			// Check feature type
			switch (featureTileData.GetCustomData(Config.TERRAINTYPE_CUSTOMDATA).AsString())
			{
				case Config.FACTORY_TERRAINTYPE:
				case Config.PORT_TERRAINTYPE:
				case Config.AIRPORT_TERRAINTYPE:
					if (CheckIfIsOwner(featureTileData))
					{
						MenuManagerInstance.ShowUnitCreationMenu(_gridPosition, featureTileData.GetCustomData(Config.TERRAINTYPE_CUSTOMDATA).AsString());
						// If a terrain feature has been found, no need to look for terrain (for now) 
						return;
					}
					break;
			}
		}

		// If no relevant feature, check terrain layer
		TileData terrainTileData = TerrainLayer.GetCellTileData(_gridPosition);

		if (terrainTileData != null && terrainTileData.HasCustomData(Config.TERRAINTYPE_CUSTOMDATA))
		{
			GD.Print("Terrain Tile terrainType is : " + terrainTileData.GetCustomData(Config.TERRAINTYPE_CUSTOMDATA));
		}
		
		// Open the game menu if no unit selected nor feature selected (when a empty tile is selected)
		MenuManagerInstance.ShowGameMenu();
	}

	#region Unit Selection
	
	public void SelectUnit(Unit unit)
	{
		if (unit.MovementPointsLeft > 0)
		{
			_selectedUnit = unit;
			_isUnitSelected = true;
			UnitManagerInstance.UpdateTerrainWeightsByMovementType(unit.MovementType);
			ApplySelectionEffects();
		}
	}

	public void DeselectUnit()
	{
		UIManagerInstance.ClearArrowPath();
		_isUnitSelected = false;
		ApplySelectionEffects();
		_selectedUnit = null;
	}
	
	private void ApplySelectionEffects()
	{
		if (_isUnitSelected)
		{
			_selectedUnit.Scale = Vector2.One * 1.15f; // Grow selected unit by 15% to show selection
		}
		else
		{
			_selectedUnit.Scale = Vector2.One;
		}
	}
	
	#endregion

	#endregion
	
	#region Utils

	public bool CheckIfIsOwner(TileData featureTileData)
	{
		if(featureTileData.HasCustomData(Config.PROPERTYOWNER_CUSTOMDATA) && featureTileData.GetCustomData(Config.PROPERTYOWNER_CUSTOMDATA).AsInt32() == PlayerManagerInstance.CurrentPlayerIndex)
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
