using Godot.Collections;
using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class AAUnit : Unit
{
	public const int BASE_PRICE = 12000;
	public override int BasePrice { get;  } = BASE_PRICE;

	public AAUnit()
	{
		MovementType = UnitMovementType.Treads;
		MovementRange = 6;
		UnitType = UnitType.AA;
	}

	public override string ToString()
	{
		return "AA";
	}
}
