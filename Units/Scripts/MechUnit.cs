using TinyConflict2D.Commons.Enums;
using TinyConflict2D.Commons.Interfaces;

namespace TinyConflict2D.Units.Scripts;

public partial class MechUnit : Unit, ICanCapture
{
	#region Properties
	
	public const int BASE_PRICE = 3000;
	public override int BasePrice { get;  } = BASE_PRICE;
	public override int MaxAmmo { get; set; } = 3;
	public override int MaxFuel { get; set; } = 70;
	
	#endregion

	#region Fields
	
	private int _captureProgress = 0;
	
	#endregion
	
	public MechUnit()
	{
		MovementType = UnitMovementType.Mech;
		MovementRange = 2;
		UnitType = UnitType.Mech;
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
	}

	public override string ToString()
	{
		return "Mech";
	}
	
	#region ICanCapture

	public void CaptureBuilding()
	{
		_captureProgress += CurrentHealth;
		MovementPointsLeft = 0;
	}
	
	public void CancelCapture()
	{
		_captureProgress = 0;
	}

	public int GetCaptureProgress()
	{
		return  _captureProgress;
	}
	
	#endregion
}
