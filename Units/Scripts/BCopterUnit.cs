using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class BCopterUnit : Unit
{
	public static int BasePrice = 9000;

	public BCopterUnit()
	{
		MovementType = UnitMovementType.Airborne;
		MovementRange = 6;
	}
}
