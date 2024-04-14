using Godot;
using System;

public class Enemy1Scene : Node2D
{
	private AnimatedSprite m_animatedSprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		m_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		// Connect the AnimationFinished signal to a method
		m_animatedSprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
		
		m_animatedSprite.Play("idle");
	}

	private void OnAnimationFinished()
	{
		// Stop the animation
		m_animatedSprite.Stop();
		m_animatedSprite.Play("idle");

	}
	
	private void AttackAnimation()
	{
		m_animatedSprite.Play("attack");
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		// this is for debug purposes
		if (Input.IsActionPressed("move_left")) {
			AttackAnimation();
		}
	}
}
