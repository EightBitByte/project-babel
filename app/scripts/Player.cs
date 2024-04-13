using Godot;
using System;

public class Player : KinematicBody2D
{
	// Speed at which the player moves, as a vector.
	private Vector2 m_speed = new Vector2(200, 200);
	// Player sprite
	private Sprite m_sprite;
	private AnimatedSprite m_animatedSprite;
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
		Right,
		None
	}
	private Direction m_direction = Direction.None;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RectangleShape2D rectangle = new RectangleShape2D();
		//m_sprite = GetNode<Sprite>("/root/Node2D/Player/Sprite");
		
		// TODO: set the starting position
		Position = new Vector2(100, 100);
		//m_sprite.Texture = m_front;
		
		m_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		
		//SpriteFrames walkFrames = new SpriteFrames("res://data/front_walk");
		
		//m_animatedSprite.Set_sprite_frames(walkFrames);
	}


	private void SwitchDirection(Direction d) {
		if (d == m_direction) {
			return;
		}
		if (d == Direction.None) {
			m_animatedSprite.Stop();
			m_animatedSprite.Frame = 0;
			/*
			if (m_direction == Direction.Front) {
				m_sprite.Texture = m_front;
			}
			if (m_direction == Direction.Right) {
				m_sprite.Texture = m_right;
			}
			if (m_direction == Direction.Left) {
				m_sprite.Texture = m_left;
			}
			if (m_direction == Direction.Back) {
				m_sprite.Texture = m_back;
			}
			*/
			m_direction = Direction.None;
			return;
		}
		
		// I would use maps in this function with direction as key and texture as value and such,
		// but alas, I will stick with the safe option
		m_direction = d;
		
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
