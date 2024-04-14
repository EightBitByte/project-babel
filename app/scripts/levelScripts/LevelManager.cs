using Godot;
using System;

public class LevelManager : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	const float chamberWidth = 1560;
	const float chamberHeight = 1200;
	
	Godot.Collections.Array<PackedScene> chamberPrefabs;
	
	Godot.Collections.Dictionary<string, Vector2> directionToVector;
	Godot.Collections.Dictionary<string, string> complementDirection;
	
	Godot.Collections.Dictionary<string, LevelChamber> coordToChamber;

	// Makes x and y into string
	private string convertToString(int x, int y) {
		return x.ToString() + ", " + y.ToString();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PackedScene testLevel = GD.Load<PackedScene>("res://levelPrefabs/testLevel1.tscn");
		chamberPrefabs = new Godot.Collections.Array<PackedScene>{ testLevel };
		
		directionToVector = new Godot.Collections.Dictionary<string, Vector2>{
			{"left", new Vector2(-1, 0)},
			{"up", new Vector2(0, 1)},
			{"right", new Vector2(1, 0)},
			{"down", new Vector2(0, -1)}
		};
		
		complementDirection = new Godot.Collections.Dictionary<string, string>{
			{"left", "right"},
			{"up", "down"},
			{"right", "left"},
			{"down", "up"}
		};
		
		coordToChamber = new Godot.Collections.Dictionary<string, LevelChamber>();
		
		GenerateChamber(0, 0, "down", 2);
	}

	// Recursively generate a chamber and its neighbors
	public void GenerateChamber(int x, int y, string incomingDir, int depth) {
		// Instantiate a chamberPrefab and add it to the dictionary
		PackedScene testLevel = GD.Load<PackedScene>("res://levelPrefabs/testLevel1.tscn");
		Node2D instance = testLevel.Instance() as Node2D;
		AddChild(instance);
		LevelChamber chamber = instance.GetChild(0) as LevelChamber;
		coordToChamber[convertToString(x, y)] = chamber;
		
		// Set position and create its chamber
		instance.Position = new Vector2(x * chamberWidth, y * chamberHeight);
		GD.Print(instance.Position);
		Godot.Collections.Array<string> outgoingDirs = chamber.CreateChamber(incomingDir, depth);
		
		// Update nearby chambers to link their doors
		Godot.Collections.Array<string> newDirections = _UpdateAdjacencies(outgoingDirs, x, y);
		
		// Generate neighbors
		_GenerateNeighbors(newDirections, x, y, depth);
	}
	
	// Updates all adjacent neighbors to link doors and returns
	// a list of all outgoing directions that didn't have neighbors
	private Godot.Collections.Array<string> _UpdateAdjacencies(Godot.Collections.Array<string> outgoingDirs, int x, int y) {
		Godot.Collections.Array<string> newDirections = new Godot.Collections.Array<string>();
		
		foreach (string dir in outgoingDirs) {
			Vector2 dirVector = directionToVector[dir]; 
			int modifyX = x + (int) dirVector.x;
			int modifyY = y + (int) dirVector.y;
			string coord = convertToString(modifyX, modifyY);
			
			// Check if there is something in x, y
			if (coordToChamber.ContainsKey(coord)) {
				// if there, generate a door in the opposite direction
				coordToChamber[coord].GenerateDoor(complementDirection[dir]);
			}
			else {
				newDirections.Add(dir);
			}
		}
		
		return newDirections;
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
