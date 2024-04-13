// Enemy.cs
//
// Defines the Enemy class, which represents an enemy in combat scenes.

using System.Collections.Generic;
using System.Dynamic;
using System.Security;
using Godot;

class Enemy : Entity {
    public Enemy (string name, int health, int maxHP, int speed, Dictionary<int, Attack> aDict)
           : base (health, maxHP, speed, aDict)  {
            Name = name;
    }

    public new void DEBUG_PRINT() {
        GD.Print("Name: ", Name);
        base.DEBUG_PRINT();
    }

    public string Name { get; }
}