using Godot;
using System.Collections.Generic;
using System.Linq;
using TinyConflict2D.Core.Players;
using TinyConflict2D.Commons.Enums;

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
	public UnitMovementType MovementType { get; set; }
	public Color PlayerColor { get; set; } = Colors.Gray;
	public Player UnitOwner { get; set; }
	public Vector2I TilePosition { get; set; }
	public string UnitType { get; set; }

	#endregion
	
	#region Fields
	
	private List<Vector2I> _path;
	private int _pathIndex = 0;
 
	#endregion
	
	public virtual void Attack(Unit target)
	{
		
	}

	public virtual void Move(List<Vector2I> newPath)
	{
		_path = newPath;
		_pathIndex = 0;
		TilePosition = newPath.Last();
	}
	
	public override void _Process(double delta)
	{
		if (_path != null && _pathIndex < _path.Count)
		{
			Vector2 targetPosition = new Vector2(_path[_pathIndex].X * TileSize + TileSize / 2, _path[_pathIndex].Y * TileSize + TileSize / 2);
			float distanceToTarget = Position.DistanceTo(targetPosition);
			
			if (distanceToTarget > 0)
			{
				Position = Position.MoveToward(targetPosition, Speed * (float)delta);
			}
			else
			{
				_pathIndex++;
				if (_pathIndex >= _path.Count)
				{
					_path = null;
					_pathIndex = 0;
					GD.Print($"Reached destination : {Position}");
				}
			}
		}
	}
}
