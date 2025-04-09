using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class APCUnit : Unit
{
	public static int BasePrice = 5000;

	public APCUnit()
	{
		MovementType = UnitMovementType.Treads;
		MovementRange = 6;
		UnitType = "APC";
	}

	public override string ToString()
	{
		return "APC";
	}
}
