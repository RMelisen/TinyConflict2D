using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class TankUnit : Unit
{
	public const int BASE_PRICE = 7000;
	public override int BasePrice { get;  } = BASE_PRICE;
	public override int MaxAmmo { get; set; } = 9;
	public override int MaxFuel { get; set; } = 70;

	public TankUnit()
	{
		MovementType = UnitMovementType.Treads;
		MovementRange = 6;
		UnitType = UnitType.Tank;
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
	}

	public override string ToString()
	{
		return "Tank";
	}
}
