using Godot;
using System.Collections.Generic;
using System.Linq;
using TinyConflict2D.Core.Players;
using TinyConflict2D.Commons.Enums;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Core;

namespace TinyConflict2D.Units.Scripts;

public partial class Unit : CharacterBody2D
{
	#region Properties
	
	[Export] 
	public float Speed = 150.0f; // Speed in units per second
	
	[Export] 
	public int TileSize = 16;
	
	public int Health { get; set; } = 100;
	public int MovementRange { get; set; } = 3;
	public int MovementPointsLeft;
	public UnitMovementType MovementType { get; set; }
	public Color PlayerColor { get; set; } = Colors.Gray;
	public Player UnitOwner { get; set; }
	public Vector2I TilePosition { get; set; }
	public string UnitType { get; set; }

	#endregion
	
	#region Fields
	
	private List<Vector2I> _path;
	private int _pathIndex = 1;
 
	#endregion

	public override void _Ready()
	{
		MovementPointsLeft = MovementRange;
	}

	public void Attack(Unit target)
	{
		
	}

	public void Move(List<Vector2I> newPath)
	{
		if (newPath != null && newPath.Count > 0 && MovementPointsLeft > 0)
		{
			_path = newPath;
			_pathIndex = 1;		// Start at 1 to skip starting tile at index 0 
			TilePosition = newPath.Last();
		}
	}

	public override void _Process(double delta)
	{
		if (_path != null && _pathIndex < _path.Count)
		{
			Vector2 targetPosition = new Vector2(_path[_pathIndex].X * TileSize + TileSize / 2, _path[_pathIndex].Y * TileSize + TileSize / 2);
			float distanceToTarget = Position.DistanceTo(targetPosition);

			// I need to move the movement points left logic in the pathfinding methods => I should return only the path passable
			
			if (MovementPointsLeft > 0 /*&& _movementPointsLeft - GetTileWeightScale(_path[_pathIndex]) >= 0*/)
			{
				if (distanceToTarget > 0)
				{
					Position = Position.MoveToward(targetPosition, Speed * (float)delta);
				}
				else
				{
					MovementPointsLeft -= GetTileWeightScale(_path[_pathIndex]);
					_pathIndex++;
					if (_pathIndex >= _path.Count)
					{
						_path = null;
						_pathIndex = 0;
						GD.Print($"Reached destination : {Position}");
					}
				}
			}
			else
			{
				GD.Print("Out of movement points.");
				_path = null;
				_pathIndex = 1;
			}
		}
	}
	
	#region Utils
	
	public int GetTileWeightScale(Vector2I tilePosition)
	{
		if (GetTree().Root.GetNode<Node>(Config.UIMANAGER_NODE_PATH) is UnitManager unitManager)
		{
			return unitManager.GetTileWeightScale(tilePosition);
		}
		GD.PrintErr($"UnitManager node not found !");
		return 1;
	}
	
	#endregion
}
