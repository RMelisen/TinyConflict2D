using TinyConflict2D.Commons.Enums;
using TinyConflict2D.Commons.Interfaces;

namespace TinyConflict2D.Units.Scripts;

public partial class InfantryUnit : Unit, ICanCapture
{
	#region Properties
	
	public const int BASE_PRICE = 1000;
	public override int BasePrice { get;  } = BASE_PRICE;
    public override int MaxAmmo { get; set; } = 0;	// Only Secondary weapon
    public override int MaxFuel { get; set; } = 99;
    
    #endregion

    #region Fields
	
    private int _captureProgress = 0;
	
    #endregion
    
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
	
	#region ICanCapture

	public void CaptureBuilding()
	{
		_captureProgress += CurrentHealth;
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
