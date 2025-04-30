using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class SubmarineUnit : Unit
{
	public const int BASE_PRICE = 20000;
	public override int BasePrice { get;  } = BASE_PRICE;
	public override int MaxAmmo { get; set; } = 6;
	public override int MaxFuel { get; set; } = 60;

	public SubmarineUnit()
	{
		MovementType = UnitMovementType.Naval;
		MovementRange = 5;
		UnitType = UnitType.Submarine;
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
	}

	public override string ToString()
	{
		return "Submarine";
	}
}
