using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class TankUnit : Unit
{
	public const int BASE_PRICE = 7000;
	public override int BasePrice { get;  } = BASE_PRICE;
	
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
