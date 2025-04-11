using Godot;
using System.Collections.Generic;
using System.Linq;
using TinyConflict2D.Units.Scripts;
using TinyConflict2D.Commons.Config;

namespace TinyConflict2D.Core.Players;

public partial class Player : Node
{
	#region Properties
	
	public Color PlayerColor { get; private set; }
	public int PlayerNumber { get; private set; }
	public List<Unit> Units { get; private set; } = new List<Unit>();
	public int Money { get; set; } = Config.STARTING_MONEY;
	
	#endregion
	
	#region Constructors
	
	public Player(int playerNumber, Color color)
	{
		PlayerNumber = playerNumber;
		PlayerColor = color;
	}
	
	#endregion
	
	#region Unit Management

	public void AddUnit(Unit unit)
	{
		Units.Add(unit);
	}

	public void RemoveUnit(Unit unit)
	{
		Units.Remove(unit);
	}
	
	public List<T> GetUnitsOfType<T>() where T : Unit
	{
		return Units.OfType<T>().ToList();
	}
	
	#endregion
}
