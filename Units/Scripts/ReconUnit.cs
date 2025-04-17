using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class ReconUnit : Unit
{
	public const int BASE_PRICE = 4000;
	public override int BasePrice { get;  } = BASE_PRICE;
	
	public ReconUnit()
	{
		MovementType = UnitMovementType.TireA;
		MovementRange = 8;
		UnitType = "Recon";
	}

	public override string ToString()
	{
		return "Recon";
	}
}
