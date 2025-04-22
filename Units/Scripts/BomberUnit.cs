using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class BomberUnit : Unit
{
	public const int BASE_PRICE = 22000;
	public override int BasePrice { get;  } = BASE_PRICE;
    public override int MaxAmmo { get; set; } = 9;
    public override int MaxFuel { get; set; } = 99;

    public BomberUnit()
	{
		MovementType = UnitMovementType.Airborne;
		MovementRange = 7;
		UnitType = UnitType.Bomber;
        CurrentAmmo = MaxAmmo;
        CurrentFuel = MaxFuel;
    }

	public override string ToString()
	{
		return "Bomber";
	}
}
