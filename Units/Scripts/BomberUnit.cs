using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class BomberUnit : Unit
{
	public const int BASE_PRICE = 22000;
	public override int BasePrice { get;  } = BASE_PRICE;

	public BomberUnit()
	{
		MovementType = UnitMovementType.Airborne;
		MovementRange = 7;
		UnitType = UnitType.Bomber;
	}

	public override string ToString()
	{
		return "Bomber";
	}
}
