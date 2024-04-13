using Godot;
using System;

public class Player : Node2D
{
	// Speed at which the player moves, as a vector.
	private Vector2 m_speed = new Vector2(200, 200);
	// Player sprite
	private Sprite m_sprite;
	private PlayerCollision m_player_collider;
	// Textures for the multiple directions player faces.
	private Texture m_front = (Texture)GD.Load("res://data/MC_front.png");
	private Texture m_back = (Texture)GD.Load("res://data/MC_back.png");
	private Texture m_left = (Texture)GD.Load("res://data/MC_left.png");
	private Texture m_right = (Texture)GD.Load("res://data/MC_right.png");

	// Represents direction of the player
	private enum Direction 
	{
		Front,
		Back,
		Left,
		Right
	}
	private Direction m_direction = Direction.Front;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RectangleShape2D rectangle = new RectangleShape2D();
		m_sprite = GetNode<Sprite>("/root/Node2D/Player/Sprite");
		
		m_player_collider = GetNode<PlayerCollision>("./Area2D/CollisionShape2D");
		
		// TODO: set the starting position
		Position = new Vector2(100, 100);
		m_sprite.Texture = m_front;
		
	}

	// Move the player according to the current input state.
	private void MovePlayer(float delta) {
		// Get input from the keyboard
		var inputVector = new Vector2();

		if (Input.IsActionPressed("move_right")) {
			if (m_direction != Direction.Right) {
				m_direction = Direction.Right;
				m_sprite.Texture = m_right;
			}
			inputVector.x += 1;
		}
		if (Input.IsActionPressed("move_left")) {
			if (m_direction != Direction.Left) {
				m_direction = Direction.Left;
				m_sprite.Texture = m_left;
			}
			inputVector.x -= 1;
		}
		if (Input.IsActionPressed("move_down")) {
			if (m_direction != Direction.Front) {
				m_direction = Direction.Front;
				m_sprite.Texture = m_front;
			}
			inputVector.y += 1;
		}
		if (Input.IsActionPressed("move_up")) {
			if (m_direction != Direction.Back) {
				m_direction = Direction.Back;
				m_sprite.Texture = m_back;
			}
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
		m_player_collider.GetCollisionDirection();
	}
}
