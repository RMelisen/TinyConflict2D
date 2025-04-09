using Godot.Collections;
using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class AAUnit : Unit
{
	public static int BasePrice = 12000;

	public AAUnit()
	{
		MovementType = UnitMovementType.Treads;
		MovementRange = 6;
		UnitType = "AA";
	}

	public override string ToString()
	{
		return "AA";
	}
}
