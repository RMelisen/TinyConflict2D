using Godot;
using System.Collections.Generic;
using TinyConflict2D.Core.Players;

namespace TinyConflict2D.Units.Scripts;

public partial class Unit : CharacterBody2D
{
	#region Properties
	
	[Export]
	public int Health { get; set; } = 100;

	[Export]
	public int MovementRange { get; set; } = 3;
	
	[Export] 
	public float Speed = 100.0f; // Speed in units per second
	
	[Export] 
	public int TileSize = 16;
	
	public Color PlayerColor { get; set; } = Colors.Gray;
	public Player UnitOwner { get; set; }
	public Vector2I TilePosition { get; set; }

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
					GD.Print("Reached destination.");
					GD.Print(Position);
				}
			}
		}
	}
}
