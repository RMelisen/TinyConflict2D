using Godot;
using System.Collections.Generic;

namespace TinyConflict2D.Core.Players;

public partial class PlayerManager : Node
{
	public List<Player> Players { get; private set; } = new List<Player>();
	public int CurrentPlayerIndex { get; private set; } = 0;

	public Player CurrentPlayer => Players[CurrentPlayerIndex];

	public override void _Ready()
	{
		Players.Add(new Player(0, Colors.Red));
		Players.Add(new Player(1, Colors.Blue));
		//Players.Add(new Player(2, Colors.Green));
		//Players.Add(new Player(3, Colors.Orange));
		GD.Print("Players Created");
	}

	public void NextTurn()
	{
		CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
		GD.Print($"Tour du joueur {CurrentPlayerIndex + 1} ({CurrentPlayer.PlayerColor})");
	}
}
