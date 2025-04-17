using Godot;
using System.Collections.Generic;
using TinyConflict2D.Commons.Config;

namespace TinyConflict2D.Core.Players;

public partial class PlayerManager : Node
{
	#region Properties

	[Export] public CoreManager CoreManagerInstance;
	public List<Player> Players { get; private set; } = new List<Player>();
	public int CurrentPlayerIndex { get; private set; } = 0;
	public Player CurrentPlayer => Players[CurrentPlayerIndex];
	public int TurnNumber { get; private set; } = 1;
	
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
		if (CurrentPlayerIndex == 0)
			TurnNumber++;
		
		CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
		GainMoney();
		GD.Print($"Player {CurrentPlayerIndex + 1} ({CurrentPlayer.PlayerColor}) turn. Money = {CurrentPlayer.Money}");
	}

	private void GainMoney()
	{
		if (TurnNumber != 1)
		{
			int ownedBuildings = CoreManagerInstance.GetNumberOfOwnedBuildings(CurrentPlayer);
			CurrentPlayer.Money += Config.MONEY_PER_TURN + ownedBuildings * Config.MONEY_PER_BUILDING;
		}
	}

	#endregion
}
