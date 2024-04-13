// CombatPlayer.cs
//
// Implements the CombatPlayer class to represent the player in combat scenes.

using System;
using System.Collections.Generic;

class CombatPlayer {
    public CombatPlayer(int health, int maxHP, Dictionary<int, Attack> aDict) {
        Health = health;
        maxHealth = maxHP;
        attackDict = aDict;
    }

    /// <summary>
    /// Gets the damage done for a given attackID. 
    /// Factors in crit chance and upgraded attacks. 
    /// </summary>
    /// <param name="attackID"></param>
    /// <returns>The amount of damage that attackID will do.</returns>
    public int GetDamage(int attackID) {
        Random rand = new Random();
        Attack currentAttack = attackDict[attackID];

        bool isCrit = rand.NextDouble() <= currentAttack.CritChance;
        int damage = rand.Next(currentAttack.MinDamage, currentAttack.MaxDamage);

        // If crit, double damage
        return isCrit ? damage * 2 : damage;
    }
    
    /// <summary>
    /// Deducts the specified amount of damage from the player's health.
    /// Specify negative damage to heal.
    /// </summary>
    /// <param name="amount">Amount of damage to deduct from the players health.</param>
    /// <returns>True if the player is killed.</returns>
    public bool Damage(int amount) {
        // If the damage we do to the player heals past maxHealth, just heal to max.
        Health -= Health - amount > maxHealth ? (Health - maxHealth) : amount;

        return Health <= 0;
    }

    private int Health;
    private readonly int maxHealth;
    private readonly Dictionary<int, Attack> attackDict;
}