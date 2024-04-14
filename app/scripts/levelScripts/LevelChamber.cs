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
	const int wallTileIdx = 2;
	const int topWallTileIdx = 1;
	
	public int depth;
	public Godot.Collections.Dictionary corridor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//         { left, up, right, down }
		corridor = new Godot.Collections.Dictionary();
		corridor["left"] = false;
		corridor["up"] = false;
		corridor["right"] = false;
		corridor["down"] = false;
	}

	public void CreateChamber()

	public void GenerateLeftDoor() {
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
		SetCell(currentX, 3, wallTileIdx);
		
		corridor["left"] = true;
	}

	public void GenerateUpDoor() {
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
		
		SetCell(-2, currentY, wallTileIdx);
		SetCell(-1, currentY, groundTileIdx);
		SetCell(0, currentY, groundTileIdx);
		SetCell(1, currentY, groundTileIdx);
		SetCell(2, currentY, wallTileIdx);
		
		corridor["up"] = true;
	}
	
	public void GenerateRightDoor() {
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
		SetCell(currentX, 3, wallTileIdx);
		
		corridor["right"] = true;
	}
	
	public void GenerateDownDoor() {
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
