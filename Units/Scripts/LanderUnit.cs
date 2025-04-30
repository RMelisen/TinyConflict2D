using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class LanderUnit : Unit
{
	public const int BASE_PRICE = 12000;
	public override int BasePrice { get;  } = BASE_PRICE;
	public override int MaxAmmo { get; set; } = 0;
	public override int MaxFuel { get; set; } = 99;

	public LanderUnit()
	{
		MovementType = UnitMovementType.Lander;
		MovementRange = 6;
		UnitType = UnitType.Lander;
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
	}

	public override string ToString()
	{
		return "Lander";
	}
}
