using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class LanderUnit : Unit
{
	public static int BasePrice = 12000;

	public LanderUnit()
	{
		MovementType = UnitMovementType.Lander;
		MovementRange = 6;
	}
}
