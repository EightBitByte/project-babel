using Godot;
using System;

public class LevelManager : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	const float chamberWidth = 1725;
	const float chamberHeight = 1340;
	
	const int maxDepth = 8;
	const int minDepth = 5;
	
	Godot.Collections.Array<PackedScene> chamberPrefabs;
	
	Godot.Collections.Dictionary<string, Vector2> directionToVector;
	Godot.Collections.Dictionary<string, string> complementDirection;
	
	Godot.Collections.Dictionary<string, LevelChamber> coordToChamber;
	
	private bool hasBossRoom = false;

	// Makes x and y into string
	private string _ConvertToString(int x, int y) {
		return x.ToString() + ", " + y.ToString();
	}
	
	// Loads all prefabs into the script
	private void _LoadPrefabs() {
		// By convention, last stage is boss stage
		PackedScene level1 = GD.Load<PackedScene>("res://levelPrefabs/enemyChamber1.tscn");
		PackedScene level2 = GD.Load<PackedScene>("res://levelPrefabs/mazeChamber1.tscn");
		PackedScene level3 = GD.Load<PackedScene>("res://levelPrefabs/mazeChamber2.tscn");
		PackedScene bossLevel = GD.Load<PackedScene>("res://levelPrefabs/bossChamber.tscn");
		chamberPrefabs = new Godot.Collections.Array<PackedScene>{ level1, level2, level3, bossLevel };
	}
	
	// Returns one of the random prefabs
	private int _GetRandomChamberType(int depth) {
		if (!hasBossRoom && depth >= minDepth) {
			hasBossRoom = true;
			return chamberPrefabs.Count - 1;
		}
		
		RandomNumberGenerator random = new RandomNumberGenerator();
		random.Randomize();
		float randomNum;
		if (depth < minDepth || hasBossRoom)
			randomNum = random.Randf() * (chamberPrefabs.Count - 1);
		else
			randomNum = random.Randf() * (chamberPrefabs.Count);
		
		if (randomNum == chamberPrefabs.Count - 1)
			hasBossRoom = true;
			
		return (int) randomNum;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_LoadPrefabs();
		
		directionToVector = new Godot.Collections.Dictionary<string, Vector2>{
			{"left", new Vector2(-1, 0)},
			{"up", new Vector2(0, -1)},
			{"right", new Vector2(1, 0)},
			{"down", new Vector2(0, 1)}
		};
		
		complementDirection = new Godot.Collections.Dictionary<string, string>{
			{"left", "right"},
			{"up", "down"},
			{"right", "left"},
			{"down", "up"}
		};
		
		coordToChamber = new Godot.Collections.Dictionary<string, LevelChamber>();
		
		GenerateChamber(0, -1, "down", 1);
	}

	// Recursively generate a chamber and its neighbors
	public void GenerateChamber(int x, int y, string incomingDir, int depth) {
		// If this chamber already exists, don't create!
		string coord = _ConvertToString(x, y);
		if (coordToChamber.ContainsKey(coord)) {
			// Updates a chamber to link doors with its neigher from the incoming edge 
			coordToChamber[coord].GenerateDoor(incomingDir);
			return;
		}
		
		// Instantiate a chamberPrefab and add it to the dictionary
		int chamberType = _GetRandomChamberType(depth);
		
		Node2D instance = chamberPrefabs[chamberType].Instance() as Node2D;
		instance.GlobalPosition = new Vector2(x * chamberWidth, y * chamberHeight);
		AddChild(instance);
		LevelChamber chamber = instance.GetChild(0) as LevelChamber;
		chamber.Init(minDepth, maxDepth);
		coordToChamber[_ConvertToString(x, y)] = chamber;
		
		// Set position and create its chamber
		if (chamberType == chamberPrefabs.Count - 1) { // If it's a boss room 
			depth = maxDepth;
		}
		Godot.Collections.Array<string> outgoingDirs = chamber.CreateChamber(incomingDir, depth, x, y);
		
		// Generate neighbors nearby
		_GenerateNeighbors(outgoingDirs, x, y, depth);
	}
	
	private void _GenerateNeighbors(Godot.Collections.Array<string> outgoingDirs, int x, int y, int currDepth) {
		foreach (string dir in outgoingDirs) {
			Vector2 dirVector = directionToVector[dir]; 
			int newX = x + (int) dirVector.x;
			int newY = y + (int) dirVector.y;
			
			GenerateChamber(newX, newY, complementDirection[dir], currDepth + 1);
		}
	}
}
