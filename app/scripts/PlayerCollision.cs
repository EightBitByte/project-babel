using Godot;
using System;

public class PlayerCollision : CollisionShape2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	private float width;
	private float height;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RectangleShape2D colliderShape = Shape as RectangleShape2D;
		width = colliderShape.Extents.x * 2;
		height = colliderShape.Extents.y * 2;
	}

	public void GetCollisionDirection() {
		Vector2 topLeft = GlobalPosition + new Vector2(-width, height);
		Vector2 topRight = GlobalPosition + new Vector2(width, height);
		Vector2 botLeft = GlobalPosition + new Vector2(-width, -height);
		Vector2 botRight = GlobalPosition + new Vector2(width, -height);
		
		var spaceState = GetWorld2d().DirectSpaceState;
		
		string[] correspondences = ["left", "left", "top", "top", "right", "right", "down", "down"];
		HashSet<string> stringSet;
		int n = correspondences.Length;

		var[] result = [spaceState.IntersectRay(topLeft, new Vector2(-0.1, 0), new Godot.Collections.Array { this }), 
					spaceState.IntersectRay(botLeft, new Vector2(-0.1, 0), new Godot.Collections.Array { this }),
					spaceState.IntersectRay(topLeft, new Vector2(0, 0.1), new Godot.Collections.Array { this }),
					spaceState.IntersectRay(topRight, new Vector2(0, 0.1), new Godot.Collections.Array { this }),
					spaceState.IntersectRay(topRight, new Vector2(0.1, 0), new Godot.Collections.Array { this }),
					spaceState.IntersectRay(botRight, new Vector2(0.1, 0), new Godot.Collections.Array { this }),
					spaceState.IntersectRay(botRight, new Vector2(0, -0.1), new Godot.Collections.Array { this }),
					spaceState.IntersectRay(botLeft, new Vector2(0, -0.1), new Godot.Collections.Array { this })
					];
		
		for (int i = 0; i < n; ++i) {
			if (result[i].Count > 0 && result[i]["collider"]) {
				GD.Print("Hit at point: ", result["position"]);
			}
		}
	}
}
