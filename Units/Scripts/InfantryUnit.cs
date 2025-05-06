using TinyConflict2D.Commons.Enums;
using TinyConflict2D.Commons.Interfaces;
using Godot;
using TinyConflict2D.Commons.Config;

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
	private Sprite2D _captureIcon;
	
	#endregion
	
	public InfantryUnit()
	{
		MovementType = UnitMovementType.Infantry;
		MovementRange = 3;
		UnitType = UnitType.Infantry;
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
		
		_captureIcon = GetNode<Sprite2D>(Config.AMMO_ICON_NAME);
		HideAllIcons();
	}

	public override string ToString()
	{
		return "Infantry";
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
	
	public void ShowCaptureIcon()
	{
		ShowIcon(_captureIcon);
	}
	
	public void HideCaptureIcon()
	{
		HideIcon(_captureIcon);
	}

	public override void HideAllIcons()
	{
		base.HideAllIcons();
		HideCaptureIcon();
	}
	
	#endregion
}
