using TinyConflict2D.Commons.Enums;
using TinyConflict2D.Commons.Interfaces;
using Godot;
using TinyConflict2D.Commons.Config;

namespace TinyConflict2D.Units.Scripts;

public partial class MechUnit : Unit, ICanCapture
{
	#region Properties
	
	[Export] 
	public Sprite2D CaptureIcon;
	
	public const int BASE_PRICE = 3000;
	public override int BasePrice { get;  } = BASE_PRICE;
	public override int MaxAmmo { get; set; } = 3;
	public override int MaxFuel { get; set; } = 70;
	
	#endregion

	#region Fields
	
	private int _captureProgress = 0;
	private Sprite2D _captureIcon;
	
	#endregion
	
	public MechUnit()
	{
		MovementType = UnitMovementType.Mech;
		MovementRange = 2;
		UnitType = UnitType.Mech;
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
		
		_captureIcon = GetNode<Sprite2D>(Config.AMMO_ICON_NAME);
		HideAllIcons();
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
