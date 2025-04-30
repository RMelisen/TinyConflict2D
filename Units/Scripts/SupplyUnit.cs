using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class SupplyUnit : Unit
{
	public const int BASE_PRICE = 5000;
	public override int BasePrice { get;  } = BASE_PRICE;
	public override int MaxAmmo { get; set; } = 0;
	public override int MaxFuel { get; set; } = 60;

	public SupplyUnit()
	{
		MovementType = UnitMovementType.TireA;
		MovementRange = 6;
		UnitType = UnitType.Supply;
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
	}

	public override string ToString()
	{
		return "Supply";
	}
}
