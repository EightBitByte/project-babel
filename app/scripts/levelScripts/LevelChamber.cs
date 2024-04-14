using Godot;
using System;

public class LevelChamber : TileMap
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	const int halfWidth = 13;
	const int halfHeight = 10;
	
	const int groundTileIdx = 0;
	const int wallTileIdx = 8;
	const int topWallTileIdx = 4;
	
	private RandomNumberGenerator random;
	
	public Godot.Collections.Dictionary<string, bool> corridor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//         { left, up, right, down }
		
	}
	
	// Based on the depth, randomly determines if the door is to be generated or not
	private bool _HasDoor(int depth, int x, int y) {
		if (depth == 0)
			return false;
		
		if (y >= 0)
			return false;
		
		float randomNum = random.Randf() * depth;
		return randomNum <= 1;
	}
	
	// Generates a door in the direction specified
	public void GenerateDoor(string direction) {
		if (direction == "left")
			GenerateLeftDoor();
		else if (direction == "up")
			GenerateUpDoor();
		else if (direction == "right")
			GenerateRightDoor();
		else if (direction == "down")
			GenerateDownDoor();
	}

	// Creates the chamber and initializes doors
	public Godot.Collections.Array<string> CreateChamber(string incomingDir, int currDepth, int x, int y) {
		
		corridor = new Godot.Collections.Dictionary<string, bool>(){
			{"left", false},
			{"up", false},
			{"right", false},
			{"down", false},
		};
		
		Godot.Collections.Dictionary<string, Vector2> directionToVector
		= new Godot.Collections.Dictionary<string, Vector2> {
			{"left", new Vector2(-1, 0)},
			{"up", new Vector2(0, -1)},
			{"right", new Vector2(1, 0)},
			{"down", new Vector2(0, 1)}
		};

		random = new RandomNumberGenerator();
		random.Randomize();
		
		GenerateDoor(incomingDir);
		Godot.Collections.Array<string> outputDirections = new Godot.Collections.Array<string>();
		
		foreach (string direction in corridor.Keys) {
			if (direction == incomingDir)
				continue;
			
			Vector2 dirVector = directionToVector[direction];
			if (_HasDoor(currDepth, x + (int) dirVector.x, y + (int) dirVector.y)) {
				GenerateDoor(direction);
				outputDirections.Add(direction);
			}
		}
		
		return outputDirections;
	}

	// Generate single door functions:
	
	public void GenerateLeftDoor() {
		if (corridor["left"])
			return;
		
		int currentX = -halfWidth;
		while (GetCell(currentX, 0) == -1) {
			SetCell(currentX, -3, topWallTileIdx);
			SetCell(currentX, -2, wallTileIdx);
			SetCell(currentX, -1, groundTileIdx);
			SetCell(currentX, 0, groundTileIdx);
			SetCell(currentX, 1, groundTileIdx);
			SetCell(currentX, 2, topWallTileIdx);
			SetCell(currentX, 3, wallTileIdx);
			
			++currentX;
		}
		
		SetCell(currentX, -3, topWallTileIdx);
		SetCell(currentX, -2, wallTileIdx);
		SetCell(currentX, -1, groundTileIdx);
		SetCell(currentX, 0, groundTileIdx);
		SetCell(currentX, 1, groundTileIdx);
		SetCell(currentX, 2, topWallTileIdx);
		SetCell(currentX, 3, topWallTileIdx);
		
		corridor["left"] = true;
	}

	public void GenerateUpDoor() {
		if (corridor["up"])
			return;
			
		int currentY = -halfHeight;
		while (GetCell(0, currentY) == -1) {
			SetCell(-2, currentY, topWallTileIdx);
			SetCell(-1, currentY, groundTileIdx);
			SetCell(0, currentY, groundTileIdx);
			SetCell(1, currentY, groundTileIdx);
			SetCell(2, currentY, topWallTileIdx);
			
			++currentY;
		}
		
		SetCell(-2, currentY, topWallTileIdx);
		SetCell(-1, currentY, groundTileIdx);
		SetCell(0, currentY, groundTileIdx);
		SetCell(1, currentY, groundTileIdx);
		SetCell(2, currentY, topWallTileIdx);
		
		++currentY;
		
		SetCell(-1, currentY, groundTileIdx);
		SetCell(0, currentY, groundTileIdx);
		SetCell(1, currentY, groundTileIdx);
		
		corridor["up"] = true;
	}
	
	public void GenerateRightDoor() {
		if (corridor["right"])
			return;
			
		int currentX = halfWidth;
		while (GetCell(currentX, 0) == -1) {
			SetCell(currentX, -3, topWallTileIdx);
			SetCell(currentX, -2, wallTileIdx);
			SetCell(currentX, -1, groundTileIdx);
			SetCell(currentX, 0, groundTileIdx);
			SetCell(currentX, 1, groundTileIdx);
			SetCell(currentX, 2, topWallTileIdx);
			SetCell(currentX, 3, wallTileIdx);
			
			--currentX;
		}
		
		SetCell(currentX, -3, topWallTileIdx);
		SetCell(currentX, -2, wallTileIdx);
		SetCell(currentX, -1, groundTileIdx);
		SetCell(currentX, 0, groundTileIdx);
		SetCell(currentX, 1, groundTileIdx);
		SetCell(currentX, 2, topWallTileIdx);
		SetCell(currentX, 3, topWallTileIdx);
		
		corridor["right"] = true;
	}
	
	public void GenerateDownDoor() {
		if (corridor["down"])
			return;
			
		int currentY = halfHeight;
		while (GetCell(0, currentY) == -1) {
			SetCell(-2, currentY, topWallTileIdx);
			SetCell(-1, currentY, groundTileIdx);
			SetCell(0, currentY, groundTileIdx);
			SetCell(1, currentY, groundTileIdx);
			SetCell(2, currentY, topWallTileIdx);
			
			--currentY;
		}
		
		for (int i = 0; i < 2; ++i) {
			SetCell(-2, currentY, topWallTileIdx);
			SetCell(-1, currentY, groundTileIdx);
			SetCell(0, currentY, groundTileIdx);
			SetCell(1, currentY, groundTileIdx);
			SetCell(2, currentY, topWallTileIdx);
			--currentY;
		}
		
		corridor["down"] = true;
	}
}
