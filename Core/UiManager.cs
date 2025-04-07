using Godot;
using System.Collections.Generic;

public partial class UiManager : Node
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

	#endregion
	
	public void UpdatePathVisualization(List<Vector2I> path)
	{
		ClearArrowPath();
		
		if (path == null) return;

		Vector2I previousTile;
			
		for (int i = 0; i < path.Count; i++)
		{
			Vector2I currentTile = path[i];

			if (i < path.Count - 1)
			{
				Vector2I nextTile = path[i + 1];
				Vector2 direction = (nextTile - currentTile);
				float angle = Mathf.Atan2(direction.Y, direction.X);

				// Determine segment type (straight or curved) based on previous and next positions
				PackedScene segmentScene = ArrowBody;
				if (i > 0)
				{
					previousTile = path[i - 1];

					float crossProduct = previousTile.X * direction.Y - previousTile.Y * direction.X;
					if (crossProduct > 0.1f) // Left turn
					{
						segmentScene = ArrowCurveLeft;
					}
					else if (crossProduct < -0.1f) // Right turn
					{
						segmentScene = ArrowCurveRight;
					}
				}

				if (segmentScene.Instantiate() is Sprite2D segmentInstance)
				{
					segmentInstance.Position = currentTile;
					segmentInstance.Rotation = angle;
					UILayer.AddChild(segmentInstance);
					_displayedPath.Add(segmentInstance);
				}
			}
			else // Last segment => Arrow Head
			{
				if (ArrowHead.Instantiate() is Sprite2D headInstance)
				{
					previousTile = path[i - 1];
					headInstance.Position = TerrainLayer.LocalToMap(currentTile);
					Vector2 directionToTarget = (currentTile - previousTile);
					headInstance.Rotation = Mathf.Atan2(directionToTarget.Y, directionToTarget.X);
					UILayer.AddChild(headInstance);
					_displayedPath.Add(headInstance);
				}
			}
		}
	}
	
	private void ClearArrowPath()
	{
		foreach (Node2D segment in _displayedPath)
		{
			segment.QueueFree();
		}
		_displayedPath.Clear();
	}
}
