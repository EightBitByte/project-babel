using Godot;
using System;

public class Player : KinematicBody2D
{
	// Speed at which the player moves, as a vector.
	private Vector2 m_speed = new Vector2(1000, 1000);
	// Player sprite
	private AnimatedSprite m_animatedSprite;

	// Represents direction of the player
	private enum Direction 
	{
		Front,
		Back,
		Left,
		Right,
		None
	}
	private Direction m_direction = Direction.None;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RectangleShape2D rectangle = new RectangleShape2D();
		
		// TODO: set the starting position
		Position = new Vector2(100, 100);
		
		m_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	// Switch direction to d (if necessary) and set animation accordingly.
	private void SwitchDirection(Direction d) {
		// If we're in the same direction, do nothing.
		if (d == m_direction) {
			return;
		}
		// If we stopped moving, stop the animation and set frame to the idle frame.
		if (d == Direction.None) {
			m_animatedSprite.Stop();
			m_animatedSprite.Frame = 0;
			m_direction = Direction.None;
			return;
		}
		
		// I would use maps in this function with direction as key and texture as value and such,
		// but alas, I will stick with the safe option
		m_direction = d;
		// Set animation according to movement direction.
		if (m_direction == Direction.Front) {
			m_animatedSprite.Play("front_walk");
		}
		if (m_direction == Direction.Right) {
			m_animatedSprite.Play("right_walk");
		}
		if (m_direction == Direction.Left) {
			m_animatedSprite.Play("left_walk");
		}
		if (m_direction == Direction.Back) {
			m_animatedSprite.Play("back_walk");
		}
	}

	// Move the player according to the current input state.
	private void MovePlayer() {
		// Get input from the keyboard
		var inputVector = new Vector2();
		Direction newDir = Direction.None;

		// Set local variables depending on input;
		if (Input.IsActionPressed("move_right")) {
			newDir = Direction.Right;
			inputVector.x += 1;
		}
		if (Input.IsActionPressed("move_left")) {
			newDir = Direction.Left;
			inputVector.x -= 1;
		}
		if (Input.IsActionPressed("move_down")) {
			newDir = Direction.Front;
			inputVector.y += 1;
		}
		if (Input.IsActionPressed("move_up")) {
			newDir = Direction.Back;
			inputVector.y -= 1;
		}
		// Switch the direction to the received input direction.
		SwitchDirection(newDir);

		// Normalize the input vector so diagonal movement isn't faster
		inputVector = inputVector.Normalized();

		// Move the square
		MoveAndSlide(inputVector * m_speed);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		MovePlayer();
	}
}
