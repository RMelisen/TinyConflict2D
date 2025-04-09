using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class InfantryUnit : Unit
{
	public static int BasePrice = 1000;
	
	public InfantryUnit()
	{
		MovementType = UnitMovementType.Infantry;
		MovementRange = 3;
	}
}
