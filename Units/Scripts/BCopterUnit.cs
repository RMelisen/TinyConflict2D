using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class BCopterUnit : Unit
{
	public const int BASE_PRICE = 9000;
	public override int BasePrice { get;  } = BASE_PRICE;

	public BCopterUnit()
	{
		MovementType = UnitMovementType.Airborne;
		MovementRange = 6;
		UnitType = "BCopter";
	}

	public override string ToString()
	{
		return "BCopter";
	}
}
