using Godot;
using System;


// Class for managing the movement of the camera which will follow the player in a circle.
public class CameraFollow : Camera2D
{
	// The player class, for determining where to move the camera.
	private Player m_player;
	// The circle in which, to the perspective of the camera, the player should stay in. 
	private double m_circleRadius = 100.0;
	// Flag for if player is outside of the circle.
	private bool m_outsideCircle = false;

	// Constant factor for centering speed.
	private float m_speed = 1.0F;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		m_player = GetNode<Player>("/root/Node2D/Player");
		Position = m_player.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		// Calculate the distance from the player to the camera center.
		Vector2 playerPosition = m_player.Position;
		double dX = (Position.x - playerPosition.x);
		double dY = (Position.y - playerPosition.y);
		
		double distance = Math.Sqrt(dX * dX + dY * dY);
		
		// If calculated distance is greater than m_circleRadius,
		// move the camera so that the player is inside the circle.  
		if (distance > m_circleRadius) {
			m_outsideCircle = true;
		}
		if (m_outsideCircle) {
			Vector2 difference = Position - playerPosition;
			Vector2 diffNorm = difference.Normalized();
			
			Vector2 step = diffNorm * (float)(-1.0 * (m_speed + (distance / 50)));
			
			if (step.Dot((playerPosition - (Position + step))) < 0.0) {
				m_outsideCircle = false;
				Position = playerPosition;
			} else {
				Position = Position + step;
			}
		}
	}
}
