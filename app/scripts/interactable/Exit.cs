using Godot;
using System;

public class Exit : Area2D
{	
	private void _on_ExitStairs_body_entered(Node2D body)
	{
		if (body.Name == "Player") {
			GetTree().ReloadCurrentScene();
		}
	}
}
