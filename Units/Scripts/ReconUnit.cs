using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class ReconUnit : Unit
{
	public const int BASE_PRICE = 4000;
	public override int BasePrice { get;  } = BASE_PRICE;
	public override int MaxAmmo { get; set; } = 0;
	public override int MaxFuel { get; set; } = 80;	// Only secondary weapon

	public ReconUnit()
	{
		MovementType = UnitMovementType.TireA;
		MovementRange = 8;
		UnitType = UnitType.Recon;
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
	}

	public override string ToString()
	{
		return "Recon";
	}
}
