using Godot;
using System;

public partial class Unit : CharacterBody2D
{
	[Export]
	public int Health { get; set; } = 100;

	[Export]
	public int MovementRange { get; set; } = 3;

	public Color PlayerColor { get; set; } = Colors.Gray;
	public Player Owner { get; set; }

	public virtual void Move(Vector2 direction)
	{
		
	}

	public virtual void Attack(Unit target)
	{
		
	}
}
