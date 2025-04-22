using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class TCopterUnit : Unit
{
	public const int BASE_PRICE = 5000;
	public override int BasePrice { get;  } = BASE_PRICE;
    public override int MaxAmmo { get; set; } = 0;
    public override int MaxFuel { get; set; } = 99;

    public TCopterUnit()
	{
		MovementType = UnitMovementType.Airborne;
		MovementRange = 6;
		UnitType = UnitType.TCopter;
        CurrentAmmo = MaxAmmo;
        CurrentFuel = MaxFuel;
    }

	public override string ToString()
	{
		return "TCopter";
	}
}
