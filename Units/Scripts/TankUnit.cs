using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class TankUnit : Unit
{
	public static int BasePrice = 7000;
	
	public TankUnit()
	{
		MovementType = UnitMovementType.Treads;
		MovementRange = 6;
		UnitType = "Tank";
	}

	public override string ToString()
	{
		return "Tank";
	}
}
