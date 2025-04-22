using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class BattleshipUnit : Unit
{
	public const int BASE_PRICE = 28000;
	public override int BasePrice { get;  } = BASE_PRICE;
    public override int MaxAmmo { get; set; } = 9;
    public override int MaxFuel { get; set; } = 99;

    public BattleshipUnit()
	{
		MovementType = UnitMovementType.Naval;
		MovementRange = 5;
		UnitType = UnitType.Battleship;
        CurrentAmmo = MaxAmmo;
        CurrentFuel = MaxFuel;
    }

	public override string ToString()
	{
		return "Battleship";
	}
}
