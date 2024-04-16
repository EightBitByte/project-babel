using Godot;
using System;


public class Interactable : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	private string m_entry = "";
	
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
		//if (Input.IsActionPressed("move_right") && interactText.Visible) {
		//}
		if (interactText.Visible && Input.IsActionPressed("interact")) {
			if (m_notLoaded) {
				m_entry = GetNode<JournalManager>("/root/movement/JournalManager").GetNextEntry();
				m_notLoaded = false;
			}
			GetNode<PanelContainer>("/root/movement/Camera2D/DialogueBox").Call("show_dialogue", m_entry);
			GD.Print(GetNode<PanelContainer>("/root/movement/Camera2D/DialogueBox"));
			m_messageShowing = true;
			return;
		}
		else if (m_messageShowing && !interactText.Visible) {
			m_messageShowing = false;
			GetNode<PanelContainer>("/root/movement/Camera2D/DialogueBox").Call("hide_dialogue");
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
	
	
	private bool m_notLoaded = true;
	private bool m_messageShowing = false;
}
