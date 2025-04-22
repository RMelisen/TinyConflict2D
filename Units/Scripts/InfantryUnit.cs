using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class InfantryUnit : Unit
{
	public const int BASE_PRICE = 1000;
	public override int BasePrice { get;  } = BASE_PRICE;
    public override int MaxAmmo { get; set; } = 0;	// Only Secondary weapon
    public override int MaxFuel { get; set; } = 99;

    public InfantryUnit()
	{
		MovementType = UnitMovementType.Infantry;
		MovementRange = 3;
		UnitType = UnitType.Infantry;
        CurrentAmmo = MaxAmmo;
        CurrentFuel = MaxFuel;
    }

	public override string ToString()
	{
		return "Infantry";
	}
}
