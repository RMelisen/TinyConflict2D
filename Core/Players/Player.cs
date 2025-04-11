using Godot;
using System.Collections.Generic;
using TinyConflict2D.Units.Scripts;

namespace TinyConflict2D.Core.Players;

public partial class Player : Node
{
	#region Properties
	
	[Export]
	public Color PlayerColor { get; set; }

	public int PlayerNumber { get; set; }
	public List<Unit> Units { get; private set; } = new List<Unit>();
	public int Money { get; set; } = 0;
	
	#endregion

	public Player(int playerNumber, Color color)
	{
		PlayerNumber = playerNumber;
		PlayerColor = color;
	}
	
	#region Unit Management

	public void AddUnit(Unit unit)
	{
		Units.Add(unit);
	}

	public void RemoveUnit(Unit unit)
	{
		Units.Remove(unit);
	}
	
	#endregion
}
