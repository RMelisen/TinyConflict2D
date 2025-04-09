using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class SupplyUnit : Unit
{
	public static int BasePrice = 5000;
	
	public SupplyUnit()
	{
		MovementType = UnitMovementType.TireA;
		MovementRange = 6;
	}
}
