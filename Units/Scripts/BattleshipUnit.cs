using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class BattleshipUnit : Unit
{
	public static int BasePrice = 28000;

	public BattleshipUnit()
	{
		MovementType = UnitMovementType.Naval;
		MovementRange = 5;
	}
}
