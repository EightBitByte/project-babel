// Enemy.cs
//
// Defines the Enemy class, which represents an enemy in combat scenes.

// Define to debug randomness.
// # define DEBUG_RANDOMNESS

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

class Enemy : Entity {
    public Enemy (string name, int health, int maxHP, int speed, Dictionary<int, Attack> aDict)
           : base (health, maxHP, speed, aDict)  {
            Name = name;

            // Initializing randomizability...
            rand = new();

            // Loading favorability...
            totalFavorability = 0.0;
            int attackIndex = 0;
            attackFavorability = new double[attackDict.Count];

            #if DEBUG_RANDOMNESS
                GD.Print("Instantiating enemy ", Name);
            #endif
            foreach (Attack a in attackDict.Values) {
                #if DEBUG_RANDOMNESS
                    GD.Print("attackFavorability[", attackIndex, "] = ", totalFavorability);
                #endif

                attackFavorability[attackIndex++] = totalFavorability;
                totalFavorability += a.Favorability;
            }
            #if DEBUG_RANDOMNESS
                GD.Print("Finish initalization, totalFavorability: ", totalFavorability, "\n");
            #endif

    }

    public new void DEBUG_PRINT() {
        GD.Print("Name: ", Name);
        base.DEBUG_PRINT();
    }

    /// <summary>
    /// </summary>
    /// <returns>A random attack, chosen randomly based on favorability.</returns>
    public Attack GetAttack() {
        double randomSelection = rand.NextDouble() * totalFavorability;
        int attackSelection = 0;

        while (attackSelection < attackDict.Count - 1 && attackFavorability[attackSelection + 1] < randomSelection)
            ++attackSelection;

        #if DEBUG_RANDOMNESS
            GD.Print("Select attack ", attackDict.Values.ElementAt(attackSelection).Name, "\n");
        #endif

        return attackDict.Values.ElementAt(attackSelection);
    }

    public string Name { get; }
    private readonly double totalFavorability;
    private readonly Random rand;
    private readonly double[] attackFavorability;
}