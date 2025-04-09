using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class TCopterUnit : Unit
{
	public static int BasePrice = 5000;
	
	public TCopterUnit()
	{
		MovementType = UnitMovementType.Airborne;
		MovementRange = 6;
		UnitType = "TCopter";
	}

	public override string ToString()
	{
		return "TCopter";
	}
}
