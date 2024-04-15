using Godot;
using System;

public class EnemyOverworld : KinematicBody2D
{
	private float m_speed = 400;
	const float randomness = 0.5f;
	private float halfWidth;
	private float halfHeight;
	
	private string currentMotion;
	private bool followingPlayer = false;
	private Node2D player;
	private float seenTimer = 0;
	
	private Godot.Collections.Dictionary<string, Vector2> directionToVector;
	private Godot.Collections.Dictionary<string, bool> isDirOpen;
	
	RandomNumberGenerator random;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get Collision size
		RectangleShape2D rectShape = GetChild<CollisionShape2D>(0).Shape as RectangleShape2D;
		halfWidth = rectShape.Extents.x / 2;
		halfHeight = rectShape.Extents.y / 2;
		
		// Add collision layer and masks
		CollisionLayer = 1 << 2;
		CollisionMask = 1 << 1;
		
		// Get player object
		player = GetNode<Node2D>("/root/movement/Player");
		
		// Scale speed and tileWidth vars to this object's scale
		m_speed *= Scale.x;
		
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
		var topLeft = GlobalPosition + new Vector2(-halfWidth, -halfHeight);
		var topRight = GlobalPosition + new Vector2(halfWidth, -halfHeight);
		var botLeft = GlobalPosition + new Vector2(-halfWidth, halfHeight);
		var botRight =  GlobalPosition + new Vector2(halfWidth, halfHeight);
		
		var rightTRColision = spaceState.IntersectRay(topRight, topRight + new Vector2(10, 0), new Godot.Collections.Array { this }, 2);
		var rightBRColision = spaceState.IntersectRay(botRight, botRight + new Vector2(10, 0), new Godot.Collections.Array { this }, 2);
		var downBLColision = spaceState.IntersectRay(botLeft, botLeft + new Vector2(0, 10), new Godot.Collections.Array { this }, 2);
		var downBRColision = spaceState.IntersectRay(botRight, botRight + new Vector2(0, 10), new Godot.Collections.Array { this }, 2);
		var leftBLColision = spaceState.IntersectRay(botLeft, botLeft + new Vector2(-10, 0), new Godot.Collections.Array { this }, 2);
		var leftTLColision = spaceState.IntersectRay(topLeft, topLeft + new Vector2(-10, 0), new Godot.Collections.Array { this }, 2);
		var upTRColision = spaceState.IntersectRay(topRight, topRight + new Vector2(0, -10), new Godot.Collections.Array { this }, 2);
		var upTLColision = spaceState.IntersectRay(topLeft, topLeft + new Vector2(0, -10), new Godot.Collections.Array { this }, 2);
		
		isDirOpen["right"] = rightTRColision.Count == 0 || rightBRColision.Count == 0;
		isDirOpen["down"] = downBLColision.Count == 0 || downBRColision.Count == 0;
		isDirOpen["left"] = leftBLColision.Count == 0 || leftTLColision.Count == 0;
		isDirOpen["up"] = upTRColision.Count == 0 || upTLColision.Count == 0;
	}
	
	private bool _CanSeePlayer() {
		var spaceState = GetWorld2d().DirectSpaceState;
		var rayToPlayer = spaceState.IntersectRay(GlobalPosition, player.GlobalPosition, new Godot.Collections.Array { this });
		if (rayToPlayer.Count > 0){
			Node2D otherCollider = rayToPlayer["collider"] as Node2D;
			return otherCollider.Name == "Player";
		}
		
		return false;
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
	
	private void _IdleMovement(float delta) {
		if (!isDirOpen[currentMotion]) {
			currentMotion = _ChooseAvailableDirection();
		}
		
		float randomNum = random.Randf();
		
		if (randomNum < delta * randomness) {
			currentMotion = _ChooseAvailableDirection();
		}
		
		MoveAndSlide(directionToVector[currentMotion] * m_speed);
	}
	
	private void _FollowPlayerMovement() {
		Vector2 direction = (player.GlobalPosition - GlobalPosition);
		
		if (!isDirOpen["right"] || !isDirOpen["left"])
			direction = new Vector2(0, direction.y);
		if (!isDirOpen["up"] || !isDirOpen["down"])
			direction = new Vector2(direction.x, 0);
			
		MoveAndSlide(direction.Normalized() * m_speed);
	}
	
	// Called once per frame
	public override void _PhysicsProcess(float delta)
	{
		if (_CanSeePlayer()) {
			seenTimer = 2;
			followingPlayer = true;
		}
		
		_CheckCollisions();
		
		if (followingPlayer) {
			if (seenTimer > 0) {
				seenTimer -= delta;
			}
			else {
				followingPlayer = false;
			}
			
			_FollowPlayerMovement();
		}
		else {
			if (followingPlayer) {
				seenTimer = 2;
			}
			
			_IdleMovement(delta);
		}
	}
	
	// Checks if the player collides
	private void _on_PlayerCheck_body_entered(Node2D body)
	{
		if (body.Name == "Player") {
			// Enter battle!
			GetNode<CombatManager>("../CombatManager").BeginCombat();
			GD.Print("Battle Enter!");
			QueueFree();
		}
	}
}
