// Entity.cs
//
// The super class of CombatPlayer and enemy.

using System.Collections.Generic;
using System;
using Godot;

class Entity {
	public Entity (int health, int maxHP, int speed, Dictionary<int, Attack> aDict) {
		Health = health;
		MaxHealth = maxHP;
		Speed = speed;
		attackDict = aDict;
		Status = StatusEffect.None;
	}


	/// <summary>
	/// Deducts the specified amount of damage from the entity's health.
	/// Specify negative damage to heal.
	/// </summary>
	/// <param name="amount">Amount of damage to deduct from the entity's health.</param>
	/// <returns>True if the entity is killed.</returns>
	public bool TakeDamage(int amount) {
		// If the damage we do to the player heals past MaxHealth, just heal to max.
		Health -= Health - amount > MaxHealth ? (Health - MaxHealth) : amount;

		return Health <= 0;
	}


	/// <summary>
	/// Prints the details of the entity. For debug purposes only.
	/// </summary>
	public void DEBUG_PRINT() {
		GD.Print("Health: ", Health, 
				 "\nMaxHealth: ", MaxHealth, 
				 "\nSpeed: ", Speed,
				 "\nattackDict: {");

		foreach (KeyValuePair<int, Attack> kv in attackDict) {
			GD.Print("Attack #", kv.Key, ": ");
			kv.Value.DEBUG_PRINT();
		}
		GD.Print("}\n");
	}


	/// <summary>
	/// Compares two Entities based on speed alone.
	/// </summary>
	/// <param name="left">The first entity to compare.</param>
	/// <param name="right">The second entity to compare.</param>
	/// <returns>An integer representing the inequality (-1 for >, 0 for ==, 1 for <)</returns>
	public static int Compare(Entity left, Entity right) {
		if (left.Speed > right.Speed)
			return -1;
		else if (left.Speed == right.Speed)
			return 0;
		else
			return 1;
	}

	/// <returns>Returns the fractional health of the entity, 0-64.</returns>
	public int GetFractionalHealth() {
		return (int)Math.Floor(1.0 * Health / MaxHealth * 64);
	}


	public int Health { get; private set; }
	public int Speed  { get; private set; }
	public int MaxHealth {get; private set; }
	public StatusEffect Status { get; set; }
	protected readonly Dictionary<int, Attack> attackDict;
}
