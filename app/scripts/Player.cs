using Godot;
using System;

public class Player : Node2D
{
	// Declare member variables here. Examples:
	private Vector2 m_speed = new Vector2(200, 200);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RectangleShape2D rectangle = new RectangleShape2D();
		
		// TODO: set the starting position
		Position = new Vector2(100, 100);
		
	}

	// Move the player according to the current input state.
	private void MovePlayer(float delta) {
		// Get input from the keyboard
		var inputVector = new Vector2();

		if (Input.IsActionPressed("move_right")) {
			inputVector.x += 1;
		}
		if (Input.IsActionPressed("move_left")) {
			inputVector.x -= 1;
		}
		if (Input.IsActionPressed("move_down")) {
			inputVector.y += 1;
		}
		if (Input.IsActionPressed("move_up")) {
			inputVector.y -= 1;
		}

		// Normalize the input vector so diagonal movement isn't faster
		inputVector = inputVector.Normalized();

		// Move the square
		Position += inputVector * m_speed * delta;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		MovePlayer(delta);
	}
	
	private void _on_Area2D_body_entered(object body)
	{
		GetCollisionDirection();
	}
}
