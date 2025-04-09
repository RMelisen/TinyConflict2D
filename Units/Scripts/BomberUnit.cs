using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class BomberUnit : Unit
{
	public static int BasePrice = 22000;

	public BomberUnit()
	{
		MovementType = UnitMovementType.Airborne;
		MovementRange = 7;
	}
}
