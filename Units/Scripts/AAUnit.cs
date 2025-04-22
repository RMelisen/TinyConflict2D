using Godot.Collections;
using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class AAUnit : Unit
{
	public const int BASE_PRICE = 12000;
	public override int BasePrice { get;  } = BASE_PRICE;
    public override int MaxAmmo { get; set; } = 9;
    public override int MaxFuel { get; set;  } = 60;

    public AAUnit()
	{
		MovementType = UnitMovementType.Treads;
		MovementRange = 6;
		UnitType = UnitType.AA;
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
	}

	public override string ToString()
	{
		return "AA";
	}
}
