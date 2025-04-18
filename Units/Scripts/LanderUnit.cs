using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class LanderUnit : Unit
{
	public const int BASE_PRICE = 12000;
	public override int BasePrice { get;  } = BASE_PRICE;

	public LanderUnit()
	{
		MovementType = UnitMovementType.Lander;
		MovementRange = 6;
		UnitType = UnitType.Lander;
	}

	public override string ToString()
	{
		return "Lander";
	}
}
