using Godot;
using System;

public class LevelManager : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	const float chamberWidth = 1725;
	const float chamberHeight = 1340;
	
	Godot.Collections.Array<PackedScene> chamberPrefabs;
	
	Godot.Collections.Dictionary<string, Vector2> directionToVector;
	Godot.Collections.Dictionary<string, string> complementDirection;
	
	Godot.Collections.Dictionary<string, LevelChamber> coordToChamber;

	// Makes x and y into string
	private string _ConvertToString(int x, int y) {
		return x.ToString() + ", " + y.ToString();
	}
	
	// Loads all prefabs into the script
	private void _LoadPrefabs() {
		PackedScene level1 = GD.Load<PackedScene>("res://levelPrefabs/basicChamber.tscn");
		PackedScene level2 = GD.Load<PackedScene>("res://levelPrefabs/enemyChamber1.tscn");
		PackedScene level3 = GD.Load<PackedScene>("res://levelPrefabs/mazeChamber1.tscn");
		PackedScene level4 = GD.Load<PackedScene>("res://levelPrefabs/mazeChamber2.tscn");
		chamberPrefabs = new Godot.Collections.Array<PackedScene>{ level1, level2, level3, level4 };
	}
	
	// Returns one of the random prefabs
	private PackedScene _GetRandomPrefab() {
		RandomNumberGenerator random = new RandomNumberGenerator();
		random.Randomize();
		float randomNum = random.Randf() * chamberPrefabs.Count;
		return chamberPrefabs[(int) randomNum];
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
		Node2D instance = _GetRandomPrefab().Instance() as Node2D;
		AddChild(instance);
		LevelChamber chamber = instance.GetChild(0) as LevelChamber;
		coordToChamber[_ConvertToString(x, y)] = chamber;
		
		// Set position and create its chamber
		instance.Position = new Vector2(x * chamberWidth, y * chamberHeight);
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
