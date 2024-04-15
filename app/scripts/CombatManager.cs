using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class CombatManager : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	private Combat m_combat;
	public bool InCombat { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InCombat = false;
		m_combat = GetNode<Combat>("../Camera2D/Combat Controller");
		GetNode<ColorRect>("../Camera2D/CombatBackground").Visible = false;
		GetNode<Node2D>("/root/movement/UIManager").Call("hide_win");
		
		m_combat.Hide();
	}
	
	public void BeginCombat()
	{
		m_combat.LoadEnemiesWithConstraints(3, 3); // spawn argos (id 3) with max 3 enemies
		m_combat.Reset();
		
		
		
		InCombat = true;
		GetNode<ColorRect>("../Camera2D/CombatBackground").Visible = true;
		m_combat.Show();
	}
	public void WinCombat()
	{
		GetNode<ColorRect>("../Camera2D/CombatBackground").Visible = false;
		m_combat.Hide();
		
		GetNode<Node2D>("/root/movement/UIManager").Call("show_win");
		InCombat = false;
	}

	public void LoseCombat()
	{
		GetNode<ColorRect>("../Camera2D/CombatBackground").Visible = false;
		m_combat.Hide();
		
		
		GetNode<Node2D>("/root/movement/UIManager").Call("show_kill");
		InCombat = false;
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
