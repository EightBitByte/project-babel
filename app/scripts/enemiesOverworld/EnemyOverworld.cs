using Godot;
using System;

public class EnemyOverworld : KinematicBody2D
{
	private float m_speed = 300;
	
	const float randomness = 0.5f;
	
	private int tileWidth = 50;
	private string currentMotion;
	
	private Godot.Collections.Dictionary<string, Vector2> directionToVector;
	private Godot.Collections.Dictionary<string, bool> isDirOpen;
	
	RandomNumberGenerator random;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Add collision layer and masks
		CollisionLayer = 1 << 2;
		CollisionMask = 1 << 1;
		
		// Scale speed and tileWidth vars to this object's scale
		m_speed *= Scale.x;
		tileWidth = (int) (tileWidth * Scale.x);
		
		// Makes direction strings to vectors
		directionToVector = new Godot.Collections.Dictionary<string, Vector2> {
			{"left", new Vector2(-1, 0)},
			{"up", new Vector2(0, -1)},
			{"right", new Vector2(1, 0)},
			{"down", new Vector2(0, 1)}
		};
		
		// Stores if each direction has a wall in front of it or not
		isDirOpen = new Godot.Collections.Dictionary<string, bool> {
			{"left", false},
			{"up", false},
			{"right", false},
			{"down", false}
		};
		
		currentMotion = "right";
		
		random = new RandomNumberGenerator();
		random.Randomize();
	}
	
	// Checks each direction of collisions using raycasts and
	// updates the collision dictionary
	private void _CheckCollisions() {
		var spaceState = GetWorld2d().DirectSpaceState;
		var rightColision = spaceState.IntersectRay(GlobalPosition, GlobalPosition + new Vector2(tileWidth, 0), new Godot.Collections.Array { this }, 2);
		var downColision = spaceState.IntersectRay(GlobalPosition, GlobalPosition + new Vector2(0, tileWidth), new Godot.Collections.Array { this }, 2);
		var leftColision = spaceState.IntersectRay(GlobalPosition, GlobalPosition + new Vector2(-tileWidth, 0), new Godot.Collections.Array { this }, 2);
		var upColision = spaceState.IntersectRay(GlobalPosition, GlobalPosition + new Vector2(0, -tileWidth), new Godot.Collections.Array { this }, 2);
		
		isDirOpen["right"] = rightColision.Count == 0;
		isDirOpen["down"] = downColision.Count == 0;
		isDirOpen["left"] = leftColision.Count == 0;
		isDirOpen["up"] = upColision.Count == 0;
	}
	
	// Chooses a random direction to move in from available directions
	private string _ChooseAvailableDirection() {
		Godot.Collections.Array<string> availableDirs = new Godot.Collections.Array<string>();
		foreach (string dir in isDirOpen.Keys) {
			if (isDirOpen[dir]) {
				availableDirs.Add(dir);
			}
		}
		
		float randomNum = random.Randf() * (availableDirs.Count);
		return availableDirs[(int) randomNum];
	}
	
	// Called once per frame
	public override void _PhysicsProcess(float delta)
	{
		_CheckCollisions();
		
		if (!isDirOpen[currentMotion]) {
			currentMotion = _ChooseAvailableDirection();
		}
		
		float randomNum = random.Randf();
		
		if (randomNum < delta * randomness) {
			currentMotion = _ChooseAvailableDirection();
		}
		
		MoveAndSlide(directionToVector[currentMotion] * m_speed);
	}
}
