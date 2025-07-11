using Godot;
using System.Collections.Generic;
using System.Linq;
using TinyConflict2D.Core.Players;
using TinyConflict2D.Commons.Enums;
using TinyConflict2D.Commons.Config;
using TinyConflict2D.Commons.Interfaces;
using TinyConflict2D.Core;

namespace TinyConflict2D.Units.Scripts;

public partial class Unit : CharacterBody2D
{
	#region Properties
	
	[Export] 
	public float Speed = 150.0f; // Speed in units per second
	
	public int MaxHealth { get; set; } = 100;
	public int CurrentHealth { get; set; }

	public int MovementRange { get; set; } = 3;

	public int MovementPointsLeft
	{
		get
		{
			return _movementPointsLeft;
		}
		set
		{
			_movementPointsLeft = value;
			if (value == 0) 
				Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);	// Gray to make sprite dimmer.
		}
	}
	public UnitMovementType MovementType { get; set; }
	public Vector2I TilePosition { get; set; }

	public Color PlayerColor { get; set; } = Colors.Gray;
	public Player UnitOwner { get; set; }

	public virtual int MaxAmmo { get; set; } = 0;
	public int CurrentAmmo { get; set; } = 0;
	public virtual int MaxFuel { get; set; } = 0;
	public int CurrentFuel { get; set; } = 0;

	public UnitType UnitType { get; set; } = UnitType.None;
	public virtual int BasePrice { get; } = 0;

	#endregion
	
	#region Fields

	private int _movementPointsLeft;
	private List<Vector2I> _path;
	private int _pathIndex = 1;
	
	private Sprite2D _ammoIcon;
	private Sprite2D _fuelIcon;
	private Sprite2D _healthIcon1;
	private Sprite2D _healthIcon2;
	private Sprite2D _healthIcon3;
	private Sprite2D _healthIcon4;
	private Sprite2D _healthIcon5;
	private Sprite2D _healthIcon6;
	private Sprite2D _healthIcon7;
	private Sprite2D _healthIcon8;
	private Sprite2D _healthIcon9;
 
	#endregion

	#region Godot Methods
	
	public override void _Ready()
	{
		MovementPointsLeft = MovementRange;
		CurrentHealth = MaxHealth;
		
		#region Icon Variable Initialization
		
		_ammoIcon = GetNode<Sprite2D>(Config.AMMO_ICON_NAME);
		_fuelIcon = GetNode<Sprite2D>(Config.FUEL_ICON_NAME);
		_healthIcon1 = GetNode<Sprite2D>(Config.HEALTH1_ICON_NAME);
		_healthIcon2 = GetNode<Sprite2D>(Config.HEALTH2_ICON_NAME);
		_healthIcon3 = GetNode<Sprite2D>(Config.HEALTH3_ICON_NAME);
		_healthIcon4 = GetNode<Sprite2D>(Config.HEALTH4_ICON_NAME);
		_healthIcon5 = GetNode<Sprite2D>(Config.HEALTH5_ICON_NAME);
		_healthIcon6 = GetNode<Sprite2D>(Config.HEALTH6_ICON_NAME);
		_healthIcon7 = GetNode<Sprite2D>(Config.HEALTH7_ICON_NAME);
		_healthIcon8 = GetNode<Sprite2D>(Config.HEALTH8_ICON_NAME);
		_healthIcon9 = GetNode<Sprite2D>(Config.HEALTH9_ICON_NAME);
		
		HideAllIcons();
		
		#endregion
	}
	
	public override void _Process(double delta)
	{
		if (_path != null && _pathIndex < _path.Count)
		{
			Vector2 targetPosition = new Vector2(_path[_pathIndex].X * Config.TILE_SIZE + Config.TILE_SIZE / 2, _path[_pathIndex].Y * Config.TILE_SIZE + Config.TILE_SIZE / 2);
			float distanceToTarget = Position.DistanceTo(targetPosition);

			if (distanceToTarget > 0)
			{
				Position = Position.MoveToward(targetPosition, Speed * (float)delta);
			}
			else
			{
				MovementPointsLeft -= GetTileWeightScale(_path[_pathIndex]);
				_pathIndex++;
				if (_pathIndex >= _path.Count)
				{
					TilePosition = _path.Last();
					_path = null;
					_pathIndex = 1;
					GD.Print($"Reached destination : {Position}");
				}
			}
		}
	}
	
	#endregion

	#region Methods
	
	public void Attack(Unit target)
	{
		if (this is ICanCapture capturingUnit)
			capturingUnit.CancelCapture();
	}

	public void Move(List<Vector2I> newPath)
	{
		if (newPath != null && newPath.Count > 0 && MovementPointsLeft > 0)
		{
			_path = newPath;
			_pathIndex = 1;		// Start at 1 to skip starting tile at index 0
			
			if (this is ICanCapture capturingUnit)
				capturingUnit.CancelCapture();
		}
	}

	public void Resupply()
	{
		CurrentAmmo = MaxAmmo;
		CurrentFuel = MaxFuel;
	}
	
	#endregion
	
	#region Utils

	public void ResetMovementPoints()
	{
		MovementPointsLeft = MovementRange;
		Modulate = Colors.White;	// White to get original sprite color.
	}
	
	public int GetTileWeightScale(Vector2I tilePosition)
	{
		if (GetTree().Root.GetNode<Node>(Config.UIMANAGER_NODE_PATH) is UnitManager unitManager)
		{
			return unitManager.GetTileWeightScale(tilePosition);
		}
		GD.PrintErr($"UnitManager node not found !");
		return 1;
	}
	
	#endregion
	
	#region UI
	
	public void ShowHealthIcon()
	{
		
	}
	
	public void HideHealthIcons()
	{
		HideIcon(_healthIcon1);
		HideIcon(_healthIcon2);
		HideIcon(_healthIcon3);
		HideIcon(_healthIcon4);
		HideIcon(_healthIcon5);
		HideIcon(_healthIcon6);
		HideIcon(_healthIcon7);
		HideIcon(_healthIcon8);
		HideIcon(_healthIcon9);
	}
	
	public void ShowAmmoIcon()
	{
		ShowIcon(_ammoIcon);
	}
	
	public void HideAmmoIcon()
	{
		HideIcon(_ammoIcon);
	}
	
	public void ShowFuelIcon()
	{
		ShowIcon(_fuelIcon);
	}
	
	public void HideFuelIcon()
	{
		HideIcon(_fuelIcon);
	}
	
	public void ShowIcon(Sprite2D icon)
	{
		icon.Visible = true;
	}
	
	public void HideIcon(Sprite2D icon)
	{
		icon.Visible = false;
	}

	public virtual void HideAllIcons()
	{
		HideHealthIcons();
		HideAmmoIcon();
		HideFuelIcon();
	}
	
	#endregion
}
