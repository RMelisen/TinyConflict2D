using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class MechUnit : Unit
{
	public const int BASE_PRICE = 3000;
	public override int BasePrice { get;  } = BASE_PRICE;
	
	public MechUnit()
	{
		MovementType = UnitMovementType.Mech;
		MovementRange = 2;
		UnitType = "Mech";
	}

	public override string ToString()
	{
		return "Mech";
	}
}
