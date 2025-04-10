using Godot;
using System.Collections.Generic;

namespace TinyConflict2D.Core;

public partial class UIManager : Node
{
	#region Properties
	
	[Export] public PackedScene ArrowBody;
	[Export] public PackedScene ArrowCurveLeft;
	[Export] public PackedScene ArrowCurveRight; 
	[Export] public PackedScene ArrowHead;
	[Export] public TileMapLayer TerrainLayer;
	[Export] public TileMapLayer UILayer;
	
	#endregion
	
	#region Fields
	
	private List<Node2D> _displayedPath = new List<Node2D>();
	private Vector2I _cellSize;

	#endregion

	public override void _Ready()
	{
		_cellSize = TerrainLayer.TileSet.TileSize;
	}

	public void UpdatePathVisualization(List<Vector2I> path)
	{
		ClearArrowPath();
		
		if (path == null || path.Count < 2) return;
			
		for (int i = 0; i < path.Count; i++)
		{
			Vector2I previousTile;
			Vector2I currentTile = path[i];
			
			// Convert tile coordinates to world positions
			Vector2 currentMapPosition = TerrainLayer.MapToLocal(currentTile);


			if (i < path.Count - 1)
			{
				Vector2I nextTile = path[i + 1];
				Vector2 nextMapPosition = TerrainLayer.MapToLocal(nextTile);
				Vector2 direction = (nextMapPosition - currentMapPosition);
				float angle = Mathf.Atan2(direction.Y, direction.X);

				// Determine segment type (straight or curved) based on previous and next positions
				PackedScene segmentScene = ArrowBody;
				if (i > 0)
				{
					previousTile = path[i - 1];
					Vector2 previousMapPosition = TerrainLayer.MapToLocal(previousTile);
					Vector2 previousDirection = (currentMapPosition - previousMapPosition);

					float crossProduct = previousDirection.X * direction.Y - previousDirection.Y * direction.X;
					if (crossProduct > 0.1f) // Left turn
					{
						segmentScene = ArrowCurveLeft;
					}
					else if (crossProduct < -0.1f) // Right turn
					{
						segmentScene = ArrowCurveRight;
					}
				}

				if (segmentScene.Instantiate() is Node2D segmentInstance)
				{
					segmentInstance.Position = currentMapPosition;
					segmentInstance.Rotation = angle;
					UILayer.AddChild(segmentInstance);
					_displayedPath.Add(segmentInstance);
				}
			}
			else // Last segment => Arrow Head
			{
				if (ArrowHead.Instantiate() is Node2D headInstance)
				{
					previousTile = path[i - 1];
					headInstance.Position = currentMapPosition;
					Vector2 previousMapPosition = TerrainLayer.MapToLocal(previousTile);
					Vector2 directionToTarget = (currentMapPosition - previousMapPosition);
					headInstance.Rotation = Mathf.Atan2(directionToTarget.Y, directionToTarget.X);
					UILayer.AddChild(headInstance);
					_displayedPath.Add(headInstance);
				}
			}
		}
	}
	
	public void ClearArrowPath()
	{
		foreach (Node2D segment in _displayedPath)
		{
			segment.QueueFree();
		}
		_displayedPath.Clear();
	}
}
