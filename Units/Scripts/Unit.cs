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
 
	#endregion

	#region Godot Methods
	
	public override void _Ready()
	{
		MovementPointsLeft = MovementRange;
		CurrentHealth = MaxHealth;
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
	
	public void HideHealthIcon()
	{
		
	}
	
	public void ShowAmmoIcon()
	{
		ShowIcon(Config.ICON_AMMO_PATH, Config.ICON_AMMO_NAME);
	}
	
	public void HideAmmoIcon()
	{
		
	}
	
	public void ShowFuelIcon()
	{
		ShowIcon(Config.ICON_FUEL_PATH, Config.ICON_FUEL_NAME);
	}
	
	public void HideFuelIcon()
	{
		
	}

	public void ShowCaptureIcon()
	{
		ShowIcon(Config.ICON_CAPTURE_PATH, Config.ICON_CAPTURE_NAME);
	}
	
	public void HideCaptureIcon()
	{
		
	}
	
	
	public void ShowIcon(string path, string name)
	{
		// TODO : Add the icons directly in the Unit Scene and show/hide it dynamically would be better than create/destroy nodes
		
		// Sprite2D icon = new Sprite2D();
		//
		// Texture2D texture = GD.Load<Texture2D>(path);
		// if (texture != null)
		// {
		// 	icon.Texture = texture;
		// }
		// else
		// {
		// 	GD.PrintErr($"Error while trying to load texture at : {path}");
		// 	return;
		// }
		//
		// icon.Position = new Vector2(8, 8);	// A little down/right of the unit
		// icon.Scale = Vector2.One * 0.7f;	// A bit smaller
		//
		// icon.Name = $"{name}Icon";
		//
		// AddChild(icon);
	}
	
	#endregion
}
