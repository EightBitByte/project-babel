using Godot;
using System;

public class EnemyScene : Node2D
{
	private AnimatedSprite m_animatedSprite;
	private string m_idleAnimation = "idle";
	private string m_attackAnimation = "attack";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		m_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		// Connect the AnimationFinished signal to a method
		m_animatedSprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
		
		m_animatedSprite.Play(m_idleAnimation);
	}
	
	public void SetAnim(int monsterId) {
		if (monsterId == 3) {
			m_idleAnimation = "ArgosIdle";
			m_attackAnimation = "ArgosAttack";
		}
		if (monsterId == -1) {
			m_idleAnimation = "dead";
			m_attackAnimation = "dead";
		}
		m_animatedSprite.Play(m_idleAnimation);
	}
	
	public void Kill() {
		m_idleAnimation = "dead";
		m_attackAnimation = "dead";
		m_animatedSprite.Play(m_idleAnimation);
	}

	private void OnAnimationFinished()
	{
		// Stop the animation
		m_animatedSprite.Stop();
		m_animatedSprite.Play(m_idleAnimation);
		Position = new Vector2(0, 0);
	}
		
	public void AttackAnimation()
	{
		m_animatedSprite.Play(m_attackAnimation);
		Position = new Vector2(-31, -31);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		/*
		// this is for debug purposes
		if (Input.IsActionPressed("move_left")) {
			AttackAnimation();
		}
		// this is for debug purposes
		if (Input.IsActionPressed("move_right")) {
			SetAnim(99);
		}
		*/
	}
}
