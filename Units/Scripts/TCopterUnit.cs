using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class TCopterUnit : Unit
{
	public const int BASE_PRICE = 5000;
	public override int BasePrice { get;  } = BASE_PRICE;
	
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
