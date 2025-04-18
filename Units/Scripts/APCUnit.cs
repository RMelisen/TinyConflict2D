using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class APCUnit : Unit
{
	public const int BASE_PRICE = 5000;
	public override int BasePrice { get;  } = BASE_PRICE;

	public APCUnit()
	{
		MovementType = UnitMovementType.Treads;
		MovementRange = 6;
		UnitType = UnitType.APC;
	}

	public override string ToString()
	{
		return "APC";
	}
}
