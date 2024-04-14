using Godot;
using System;

public class LevelManager : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	const float chamberWidth = 26;
	const float chamberHeight = 20;
	
	Godot.Collections.Array<PackedScene> chambers;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PackedScene testLevel = GD.Load<PackedScene>("res://levelPrefabs/testLevel1.tscn");
		chambers = new Godot.Collections.Array<PackedScene>{ testLevel };
		
		GenerateChamber(0, 0, "down", 1);
	}

	// Recursively generate a chamber and its neighbors
	public void GenerateChamber(int x, int y, string incomingDir, int depth) {
		PackedScene testLevel = GD.Load<PackedScene>("res://levelPrefabs/testLevel1.tscn");
		Node2D instance = testLevel.Instance() as Node2D;
		AddChild(instance);
		LevelChamber chamber = instance.GetChild(0) as LevelChamber;
		
		instance.Position = new Vector2(x * chamberWidth, y * chamberHeight);
		Godot.Collections.Array<string> outgoingDir = chamber.CreateChamber(incomingDir, depth);
	}
}
