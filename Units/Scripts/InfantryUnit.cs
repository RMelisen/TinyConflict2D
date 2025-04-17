using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class InfantryUnit : Unit
{
	public const int BASE_PRICE = 1000;
	public override int BasePrice { get;  } = BASE_PRICE;
	
	public InfantryUnit()
	{
		MovementType = UnitMovementType.Infantry;
		MovementRange = 3;
		UnitType = "Infantry";
	}

	public override string ToString()
	{
		return "Infantry";
	}
}
