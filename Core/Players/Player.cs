using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node
{
	[Export]
	public Color PlayerColor { get; set; }

	public List<Unit> Units { get; private set; } = new List<Unit>();

	public int Money { get; set; } = 1000;

	public Player(Color color)
	{
		PlayerColor = color;
	}

	public void AddUnit(Unit unit)
	{
		Units.Add(unit);
	}

	public void RemoveUnit(Unit unit)
	{
		Units.Remove(unit);
	}
}
