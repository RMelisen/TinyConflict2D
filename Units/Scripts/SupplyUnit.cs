using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class SupplyUnit : Unit
{
	public const int BASE_PRICE = 5000;
	public override int BasePrice { get;  } = BASE_PRICE;
	
	public SupplyUnit()
	{
		MovementType = UnitMovementType.TireA;
		MovementRange = 6;
		UnitType = UnitType.Supply;
	}

	public override string ToString()
	{
		return "Supply";
	}
}
