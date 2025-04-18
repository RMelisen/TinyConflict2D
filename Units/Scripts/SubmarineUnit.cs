using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class SubmarineUnit : Unit
{
	public const int BASE_PRICE = 20000;
	public override int BasePrice { get;  } = BASE_PRICE;
	
	public SubmarineUnit()
	{
		MovementType = UnitMovementType.Naval;
		MovementRange = 5;
		UnitType = UnitType.Submarine;
	}

	public override string ToString()
	{
		return "Submarine";
	}
}
