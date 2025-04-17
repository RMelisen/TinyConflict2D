using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class BattleshipUnit : Unit
{
	public const int BASE_PRICE = 28000;
	public override int BasePrice { get;  } = BASE_PRICE;

	public BattleshipUnit()
	{
		MovementType = UnitMovementType.Naval;
		MovementRange = 5;
		UnitType = "Battleship";
	}

	public override string ToString()
	{
		return "Battleship";
	}
}
