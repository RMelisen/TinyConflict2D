using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class SubmarineUnit : Unit
{
	public static int BasePrice = 20000;
	
	public SubmarineUnit()
	{
		MovementType = UnitMovementType.Naval;
		MovementRange = 5;
		UnitType = "Submarine";
	}

	public override string ToString()
	{
		return "Submarine";
	}
}
