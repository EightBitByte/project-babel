using Godot;
using System;

public class Interactable : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	Label interactText;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		interactText = GetChild(2) as Label;
		interactText.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("move_right") && interactText.Visible) {
		}
	}

	private void _on_Interactable_body_entered(object body)
	{
		interactText.Show();
	}
	
	private void _on_Interactable_body_exited(object body)
	{
		interactText.Hide();
	}
}
