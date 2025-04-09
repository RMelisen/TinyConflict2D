using TinyConflict2D.Commons.Enums;

namespace TinyConflict2D.Units.Scripts;

public partial class ReconUnit : Unit
{
	public static int BasePrice = 4000;
	
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
