using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class BCopterUnit : Unit
{
	public const int BASE_PRICE = 9000;
	public override int BasePrice { get;  } = BASE_PRICE;
    public override int MaxAmmo { get; set; } = 6;
    public override int MaxFuel { get; set; } = 99;

    public BCopterUnit()
	{
		MovementType = UnitMovementType.Airborne;
		MovementRange = 6;
		UnitType = UnitType.BCopter;
        CurrentAmmo = MaxAmmo;
        CurrentFuel = MaxFuel;
    }

	public override string ToString()
	{
		return "BCopter";
	}
}
