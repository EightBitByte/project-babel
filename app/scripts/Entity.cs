// Entity.cs
//
// The super class of CombatPlayer and enemy.

using System.Collections.Generic;
using System;
using Godot;

class Entity {
    public Entity (int health, int maxHP, int speed, Dictionary<int, Attack> aDict) {
        Health = health;
        maxHealth = maxHP;
        Speed = speed;
        attackDict = aDict;
    }

    /// <summary>
    /// Gets the damage done for a given attackID. 
    /// Factors in crit chance and upgraded attacks. 
    /// </summary>
    /// <param name="attackID"></param>
    /// <returns>The amount of damage that attackID will do.</returns>
    public int GetDamage(int attackID) {
        Random rand = new();
        Attack currentAttack = attackDict[attackID];

        bool isCrit = rand.NextDouble() <= currentAttack.CritChance;
        int damage = rand.Next(currentAttack.MinDamage, currentAttack.MaxDamage);

        // If crit, double damage
        return isCrit ? damage * 2 : damage;
    }


    /// <summary>
    /// Deducts the specified amount of damage from the entity's health.
    /// Specify negative damage to heal.
    /// </summary>
    /// <param name="amount">Amount of damage to deduct from the entity's health.</param>
    /// <returns>True if the entity is killed.</returns>
    public bool Damage(int amount) {
        // If the damage we do to the player heals past maxHealth, just heal to max.
        Health -= Health - amount > maxHealth ? (Health - maxHealth) : amount;

        return Health <= 0;
    }

    /// <summary>
    /// Prints the details of the entity. For debug purposes only.
    /// </summary>
    public void DEBUG_PRINT() {
        GD.Print("Health: ", Health, 
                 "\nMaxHealth: ", maxHealth, 
                 "\nSpeed: ", Speed,
                 "\nattackDict: {");

        foreach (KeyValuePair<int, Attack> kv in attackDict) {
            GD.Print("Attack #", kv.Key, ": ");
            kv.Value.DEBUG_PRINT();
        }
        GD.Print("}\n");
    }

    public int Health { get; private set; }
    public int Speed  { get; private set; }
    protected readonly int maxHealth;
    private readonly Dictionary<int, Attack> attackDict;
}