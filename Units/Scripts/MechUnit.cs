using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class MechUnit : Unit
{
	public static int BasePrice = 3000;
	
	public MechUnit()
	{
		MovementType = UnitMovementType.Mech;
		MovementRange = 2;
	}
}
