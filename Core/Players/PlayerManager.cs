using Godot;
using System.Collections.Generic;

namespace TinyConflict2D.Core.Players;

public partial class PlayerManager : Node
{
	#region Properties
	
	public List<Player> Players { get; private set; } = new List<Player>();
	public int CurrentPlayerIndex { get; private set; } = 0;
	public Player CurrentPlayer => Players[CurrentPlayerIndex];
	
	#endregion

	#region Godot Methods
	
	public override void _Ready()
	{
		Players.Add(new Player(0, Colors.Red));
		Players.Add(new Player(1, Colors.Blue));
		//Players.Add(new Player(2, Colors.Green));
		//Players.Add(new Player(3, Colors.Orange));
	}
	
	#endregion

	#region Turn Management
	
	public void NextTurn()
	{
		CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
		GD.Print($"Player {CurrentPlayerIndex + 1} ({CurrentPlayer.PlayerColor}) turn");
	}

	private void GainMoney()
	{
		// Flat 1000 + 1000 per building
		CurrentPlayer.Money += 1000;
	}
	
	#endregion
}
