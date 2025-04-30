using Godot;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.Core;

public partial class CursorController : Sprite2D
{
	#region Properties
	
	[Export] public CoreManager CoreManagerInstance; 
	[Export] public UnitManager UnitManagerInstance;
	[Export] public MenuManager MenuManagerInstance;
	[Export] public UIManager UIManagerInstance;
	
	#endregion
	
	#region Fields
	
	private Vector2I _gridPosition = Vector2I.Zero;	

	#endregion
	
	#region Godot Methods
	
	public override void _Ready()
	{
		_gridPosition = CoreManagerInstance.LocalToMap(Position);
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_right") || Input.IsActionJustPressed("ui_left") ||
			Input.IsActionJustPressed("ui_down") || Input.IsActionJustPressed("ui_up"))
		{
			MoveCursor();

			if (CoreManagerInstance.IsUnitSelected)
			{
				UIManagerInstance.UpdatePathVisualization(UnitManagerInstance.GetPathBetween(CoreManagerInstance.SelectedUnit.TilePosition, _gridPosition, CoreManagerInstance.SelectedUnit.MovementPointsLeft));
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
					if (CoreManagerInstance.IsUnitSelected)
					{
						CoreManagerInstance.DeselectUnit();
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
			
		if (Input.IsActionJustPressed(Config.INPUT_UI_RIGHT))
		{
			newGridPosition.X++;
		}
		if (Input.IsActionJustPressed(Config.INPUT_UI_LEFT))
		{
			newGridPosition.X--;
		}
		if (Input.IsActionJustPressed(Config.INPUT_UI_DOWN))
		{
			newGridPosition.Y++;
		}
		if (Input.IsActionJustPressed(Config.INPUT_UI_UP))
		{
			newGridPosition.Y--;
		}
		
		// Check if the new position is within the map bounds
		if (CoreManagerInstance.IsWithinBounds(newGridPosition))
		{
			_gridPosition = newGridPosition;
			Position = _gridPosition * Config.TILE_SIZE;
		}
	}
	

	#endregion

	#region Selection

	private void OnCursorSelect()
	{
		if (!CoreManagerInstance.IsUnitSelected)
		{
			// Check if a unit is found first
			Unit unitFound = UnitManagerInstance.GetAllyUnitAt(_gridPosition);
			if (unitFound != null)
			{
				// Unit found
				GD.Print("Selected unit: " + unitFound.GetType());
				CoreManagerInstance.SelectUnit(unitFound);
				return;
			}
		}
		else
		{
			if (_gridPosition == CoreManagerInstance.SelectedUnit.TilePosition)
			{
				// If selected unit is selected again
				UIManagerInstance.ClearHighlighting();
				UIManagerInstance.ClearArrowPath();
				MenuManagerInstance.ShowUnitActionMenu(_gridPosition);	
			}
			else
			{
				// Check if a unit is found first
				Unit unitFound = UnitManagerInstance.GetAllyUnitAt(_gridPosition);
				if (unitFound != null)
				{
					// TODO: Manage unit transportation (eg. land units in lander, infantry in TCopter/APC, etc...
					return;
				}
				
				// If no other unit on target tile, move selected unit 
				CoreManagerInstance.SelectedUnit.Move(UnitManagerInstance.GetPathBetween(CoreManagerInstance.SelectedUnit.TilePosition, _gridPosition, CoreManagerInstance.SelectedUnit.MovementPointsLeft));
				UIManagerInstance.ClearHighlighting();
				UIManagerInstance.ClearArrowPath();
				MenuManagerInstance.ShowUnitActionMenu(_gridPosition);
			}
			return;
		}

		// Check feature layer second
		Variant? featureTerrainType = CoreManagerInstance.GetFeatureByCustomData(Config.TERRAINTYPE_CUSTOMDATA, _gridPosition);
		if (featureTerrainType.HasValue)
		{
			// Check feature type
			switch (featureTerrainType.Value.ToString())
			{
				case Config.FACTORY_TERRAINTYPE:
				case Config.PORT_TERRAINTYPE:
				case Config.AIRPORT_TERRAINTYPE:
					if (CoreManagerInstance.CheckIfIsOwner(_gridPosition))
					{
						MenuManagerInstance.ShowUnitCreationMenu(_gridPosition, featureTerrainType.Value.ToString());
						// If a terrain feature has been found, no need to look for terrain (for now) 
						return;
					}
					break;
			}
		}

		// If no relevant feature, check terrain layer
		Variant? tileTerrainType = CoreManagerInstance.GetTerrainByCustomData(Config.TERRAINTYPE_CUSTOMDATA, _gridPosition);
		if (tileTerrainType.HasValue)
		{
			// TODO: Show terrain information (defense level, cost, etc...)
		}
		
		// Open the game menu if no unit selected nor feature selected (when a empty tile is selected)
		MenuManagerInstance.ShowGameMenu();
	}
	
	#endregion
}
